using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLogic : MonoBehaviour
{
    public TMP_Text rolledNumberText;
    public DiceRollScript diceRollScript;
    public float moveSpeed = 3f;
    public GameObject platformsParent;
    public GameObject winScreen; // Reference to the win screen UI
    public TMP_Text winMessageText; // Displays win/loss message
    public TMP_Text rollsCountText; // Displays roll count
    public TMP_Text timeElapsedText; // Displays time elapsed

    private bool playerCanRoll = true;
    private List<GameObject> players = new List<GameObject>();
    private List<Transform> waypoints = new List<Transform>();
    private int[] playerTilePositions;
    private bool isMoving = false;
    private Animator characterAnimator;
    private SpriteRenderer characterSprite;
    private bool facingRight = true;
    private string characterName;
    private HashSet<int> turnTiles = new HashSet<int> { 4, 9, 14, 19 };
    private int currentPlayerIndex = 0;
    private int rollCount = 0;
    private float startTime;
    private bool gameOver = false; // Prevent further actions after the game ends

    void Start()
    {
        FindWaypoints();
        FindPlayers();
        playerTilePositions = new int[players.Count];
        SetupCurrentPlayer();
        startTime = Time.time;
    }

    void FindWaypoints()
    {
        waypoints.Clear();
        if (platformsParent != null)
        {
            foreach (Transform tile in platformsParent.transform)
            {
                if (tile.childCount > 0)
                {
                    Transform waypoint = tile.GetChild(0);
                    waypoints.Add(waypoint);
                }
            }
        }
        Debug.Log($"Waypoints found: {waypoints.Count}");
    }

    void FindPlayers()
    {
        players.Clear();
        PlayerScript playerScript = FindObjectOfType<PlayerScript>();
        if (playerScript != null)
        {
            players.AddRange(playerScript.GetAllPlayers());
        }
        Debug.Log($"Players found: {players.Count}");
    }

    void SetupCurrentPlayer()
    {
        if (players.Count > 0)
        {
            GameObject playerObject = players[currentPlayerIndex];
            characterAnimator = playerObject.GetComponent<Animator>();
            characterSprite = playerObject.GetComponent<SpriteRenderer>();
            characterName = playerObject.name.Replace("(Clone)", "");
        }
        playerCanRoll = (currentPlayerIndex == 0);
    }

    public void MovePlayer(int steps)
    {
        if (!isMoving && waypoints.Count > 0 && !gameOver)
        {
            playerCanRoll = false;

            // Increment roll count only for the player (index 0)
            if (currentPlayerIndex == 0)
            {
                rollCount++;
            }

            Debug.Log($"🎲 Dice rolled: {steps} steps! Starting movement...");
            StartCoroutine(MoveStepByStep(steps));
        }
    }


    IEnumerator MoveStepByStep(int steps)
    {
        isMoving = true;
        GameObject character = players[currentPlayerIndex];

        int targetIndex = Mathf.Min(playerTilePositions[currentPlayerIndex] + steps, waypoints.Count - 1);
        int stepCount = 0;

        PlayAnimation("Walk");

        while (playerTilePositions[currentPlayerIndex] < targetIndex)
        {
            int nextIndex = playerTilePositions[currentPlayerIndex] + 1;
            if (nextIndex >= waypoints.Count) break;

            Transform nextWaypoint = waypoints[nextIndex];
            Debug.Log($"🚶 Moving step {stepCount + 1}/{steps} ➡ Tile {nextIndex} at {nextWaypoint.position}");

            yield return StartCoroutine(MoveToPosition(character, nextWaypoint.position));

            if (turnTiles.Contains(playerTilePositions[currentPlayerIndex]))
            {
                facingRight = !facingRight;
                characterSprite.flipX = !characterSprite.flipX;
                Debug.Log($"🔄 Turned after leaving tile {playerTilePositions[currentPlayerIndex]}, now facing {(facingRight ? "right" : "left")}");
            }

            playerTilePositions[currentPlayerIndex] = nextIndex;
            stepCount++;
        }

        Debug.Log($"✅ Finished moving. Landed on tile {playerTilePositions[currentPlayerIndex]} after {stepCount} steps.");
        PlayAnimation("Idle");
        isMoving = false;
        SpreadPlayersOnTile(playerTilePositions[currentPlayerIndex]);

        if (playerTilePositions[currentPlayerIndex] == waypoints.Count - 1)
        {
            HandleWin();
        }
        else
        {
            if (diceRollScript != null)
            {
                Debug.Log("🎲 Resetting dice for next roll.");
                diceRollScript.ResetDice();
            }
            NextTurn();
        }
    }

    void HandleWin()
    {
        Debug.Log("🏆 Game Over!");

        gameOver = true; // Stop further actions
        Time.timeScale = 0f; // Pause game time

        float timeElapsed = Time.time - startTime;
        winScreen.SetActive(true); // Show the win screen

        if (currentPlayerIndex == 0)
        {
            winMessageText.text = "🎉 You Won! 🏆";
        }
        else
        {
            winMessageText.text = "💀 You Lost! The AI Won... 😞";
        }

        rollsCountText.text = $"Rolls Taken: {rollCount}";
        timeElapsedText.text = $"Time Elapsed: {timeElapsed:F2} seconds";

        SaveLoadScript saveLoad = FindObjectOfType<SaveLoadScript>();
        //if (saveLoad != null)
        //{
        //    saveLoad.SaveGame();
        //}
    }




    void SpreadPlayersOnTile(int tileIndex)
    {
        List<GameObject> playersOnTile = new List<GameObject>();
        foreach (var player in players)
        {
            if (playerTilePositions[players.IndexOf(player)] == tileIndex)
            {
                playersOnTile.Add(player);
            }
        }

        for (int i = 0; i < playersOnTile.Count; i++)
        {
            Vector3 offset = new Vector3((i % 2 == 0 ? 1 : -1) * (i / 2) * 0.3f, (i / 2) * 0.3f, 0);
            playersOnTile[i].transform.position += offset;
        }
    }

    IEnumerator MoveToPosition(GameObject character, Vector3 target)
    {
        while (Vector3.Distance(character.transform.position, target) > 0.1f)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log($"✔️ Reached waypoint at {target}");
    }

    void PlayAnimation(string action)
    {
        if (characterAnimator != null)
        {
            string animationName = characterName + action;
            characterAnimator.Play(animationName);
            Debug.Log($"🎭 Playing animation: {animationName}");
        }
    }

    void NextTurn()
    {
        if (gameOver) return; // Stop turn progression if the game has ended

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        SetupCurrentPlayer();

        if (currentPlayerIndex != 0)
        {
            StartCoroutine(AIRollDice());
        }
    }

    IEnumerator AIRollDice()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("🎲 AI is rolling the dice...");
        diceRollScript.RollDice();
    }

    public bool CanPlayerRoll()
    {
        return playerCanRoll;
    }
}
