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
    public Camera mainCamera;
    public float zoomInSize = 5f;
    public float zoomOutSize = 10f;
    private Vector3 cameraOriginalPosition;
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

    void Start()
    {
        FindWaypoints();
        FindPlayers();
        playerTilePositions = new int[players.Count];
        cameraOriginalPosition = mainCamera.transform.position;
        mainCamera.orthographicSize = zoomOutSize;
        SetupCurrentPlayer();
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
        if (!isMoving && waypoints.Count > 0)
        {
            playerCanRoll = false;
            Debug.Log($"🎲 Dice rolled: {steps} steps! Starting movement...");
            StartCoroutine(MoveStepByStep(steps));
        }
    }

    IEnumerator MoveStepByStep(int steps)
    {
        isMoving = true;
        GameObject character = players[currentPlayerIndex];
        mainCamera.orthographicSize = zoomInSize;

        int targetIndex = Mathf.Min(playerTilePositions[currentPlayerIndex] + steps, waypoints.Count - 1);
        int stepCount = 0;

        PlayAnimation("Walk");

        while (playerTilePositions[currentPlayerIndex] < targetIndex)
        {
            int nextIndex = playerTilePositions[currentPlayerIndex] + 1;
            if (nextIndex >= waypoints.Count) break;

            Transform nextWaypoint = waypoints[nextIndex];
            Debug.Log($"🚶 Moving step {stepCount + 1}/{steps} ➡ Tile {nextIndex} at {nextWaypoint.position}");

            Vector3 randomOffset = (Vector3)Random.insideUnitCircle * 0.5f;
            randomOffset.z = 0;
            yield return StartCoroutine(MoveToPosition(character, nextWaypoint.position + randomOffset + new Vector3(0, 0.1f, 0)));

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

        mainCamera.orthographicSize = zoomOutSize;
        mainCamera.transform.position = new Vector3(cameraOriginalPosition.x, cameraOriginalPosition.y + 3, cameraOriginalPosition.z);

        if (diceRollScript != null)
        {
            Debug.Log("🎲 Resetting dice for next roll.");
            diceRollScript.ResetDice();
        }

        if (playerTilePositions[currentPlayerIndex] == waypoints.Count - 1)
        {
            Debug.Log("🏆 You Win!");
        }
        else
        {
            NextTurn();
        }
    }

    IEnumerator MoveToPosition(GameObject character, Vector3 target)
    {
        while (Vector3.Distance(character.transform.position, target) > 0.1f)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, target, moveSpeed * Time.deltaTime);
            mainCamera.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 3, mainCamera.transform.position.z);
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
