using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }
    private string leaderboardFileName = "leaderboard.json";

    [Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public float timeElapsed;
        public int rollCount;
    }

    [Serializable]
    public class LeaderboardData
    {
        public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    }

    private LeaderboardData leaderboard = new LeaderboardData();

    public Transform leaderboardContainer; // Assign a Vertical Layout Group in Inspector
    public GameObject leaderboardEntryPrefab; // Assign a Text prefab in Inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderboard();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEntry(string playerName, float timeElapsed, int rollCount)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry
        {
            playerName = playerName,
            timeElapsed = timeElapsed,
            rollCount = rollCount
        };

        leaderboard.entries.Add(newEntry);
        SortAndTrimLeaderboard();
        SaveLeaderboard();
        DisplayLeaderboard();
    }

    private void SortAndTrimLeaderboard()
    {
        leaderboard.entries = leaderboard.entries
            .OrderBy(e => e.timeElapsed)  // Sort by least time
            .ThenBy(e => e.rollCount)     // Then by least rolls
            .Take(10)                     // Keep only top 10
            .ToList();
    }

    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboard, true);
        File.WriteAllText(Application.persistentDataPath + "/" + leaderboardFileName, json);
    }

    public void LoadLeaderboard()
    {
        string filePath = Application.persistentDataPath + "/" + leaderboardFileName;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            leaderboard = JsonUtility.FromJson<LeaderboardData>(json);
        }
    }

    public List<LeaderboardEntry> GetTopEntries()
    {
        return leaderboard.entries;
    }

    public void DisplayLeaderboard()
    {
        // Clear previous entries
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        List<LeaderboardEntry> topEntries = GetTopEntries();

        foreach (var entry in topEntries)
        {
            GameObject newEntry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
            TextMeshProUGUI entryText = newEntry.GetComponent<TextMeshProUGUI>();
            entryText.text = $"{entry.playerName} - Time: {entry.timeElapsed:F2}s - Rolls: {entry.rollCount}";
        }
    }
}
