using GameCore.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using WolfAudioSystem;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;
    [Header("Services")]
    [SerializeField]
    List<GameService> services;

    [Header("Audio")]
    public GameAudioData audioData;

    [Header("Maze Configuration")]
    [SerializeField]
    int mazeHeight;
    [SerializeField]
    int mazeWidth;
    [SerializeField]
    bool wrapped;
    [SerializeField]
    int interconnectivity;

    [Header("Maze Components")]
    private Player player;
    private Puppy puppy;
    private Monster monster;
    MazeLocation exit = null;
    public Monster Monster { get => monster; }

    [SerializeField]
    private DungeonViewer viewer;
    public DungeonViewer Viewer => viewer;

    [SerializeField]
    private Dungeon dungeon;

    public PanicBar puppyPanicBar;
    public PanicBar playerPanicBar;

    [Header("Game Play Settings")]
    [SerializeField]
    float minEchoStrength = .5f;

    [Header("UI Initialization")]
    [SerializeField]
    TMPro.TextMeshProUGUI playerBarText;
    [SerializeField]
    TMPro.TextMeshProUGUI puppyBarText;

    [Header("Lazy Lazy")]
    [SerializeField]
    JustAFader fader;
    [SerializeField]
    AudioClip doggoAudio;
    [SerializeField]
    AudioClip monsterAudio;
    [SerializeField]
    NotificationText notificationTemplate;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    bool debugging;
    // game state tracking
    bool foundPuppy;
    bool playerDead;
    bool playerFoundExit;
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        InitializeService();
        InitializeDungeon();
        if (AudioSystem.Instance)
        {
            AudioSystem.Instance.TransitionBGMQuick(audioData.dungeonBGMClip);
        }
        playerBarText.text = GameStaticData.PLAYER_NAME;
        puppyBarText.text = GameStaticData.DOGGO_NAME;
    }
    private void Update()
    {
        viewer.showAll = debugging;
        viewer.UpdateGraphicsByPlayerPosition(player.GetComponent<WalkableChecker>().SteppingOn);
        MazeLocation poppyLocation = viewer.worldLocationToMazeLocation(puppy.transform.position);
        MazeLocation reedLocation = viewer.worldLocationToMazeLocation(player.transform.position);
        MazeLocation monsterLocation = viewer.worldLocationToMazeLocation(monster.transform.position);
        if (poppyLocation != reedLocation && !debugging && !foundPuppy)
        {
            puppy.Hide();
        }
        else
        {
            puppy.Show();
        }
        if (monsterLocation != reedLocation && !debugging)
        {
            monster.Hide();
        }
        else
        {
            monster.Show();
        }
    }
    private void FixedUpdate()
    {
        viewer.UpdateMazeCellCollidersBasedOnPlayerPosition(player.transform.position);
    }

    void InitializeService()
    {
        if (services != null)
        {
            foreach (GameService service in services)
            {
                ServiceLocator.RegisterService(service);
            }
        }

    }

    private void InitializeDungeon()
    {
        dungeon.InitializeMaze(mazeHeight, mazeWidth, wrapped, interconnectivity);
        viewer.setMaze(dungeon.Maze);
        MazeLocation playerL = dungeon.InitializePlayerMazeLocation();
        MazeLocation puppyL = dungeon.InitializePuppyLocation();
        MazeLocation monsterL = dungeon.InitializeMonsterLocation();

        player = viewer.InstantiatePlayer(playerL).GetComponent<Player>();
        player.panicBar = this.playerPanicBar;

        puppy = viewer.InstantiatePuppy(puppyL).GetComponent<Puppy>();
        puppy.panicBar = this.puppyPanicBar;
        monster = viewer.InstantiateMonster(monsterL).GetComponent<Monster>();
    }

    // triggered when Call button clicks
    public void Calling(string text = null)
    {
        if (!foundPuppy)
        {
            CallingDoggo();
        }
        else
        {
            AskExit();
        }
        void CallingDoggo()
        {
            StartCoroutine(CallingCR());
            IEnumerator CallingCR()
            {
                if (text != null)
                {
                    PlayerSays(text);
                }
                yield return new WaitForSeconds(2f);

                MazeLocation playerLocation = viewer.worldLocationToMazeLocation(player.transform.position);
                MazeLocation monsterLocation = viewer.worldLocationToMazeLocation(monster.transform.position);
                int playerToMonster = Utility.calculateSoundStrength(dungeon.Maze, playerLocation, monsterLocation, player.GetSoundStrength);

                // if puppy barks, lead monster to the closest one.
                bool puppyBark = puppy.called();
                if (puppyBark)
                {
                    int puppyToMonster = Utility.calculateSoundStrength(dungeon.Maze, puppy.Location, monsterLocation, player.GetSoundStrength);
                    ShowDoggoEcho();
                    yield return new WaitForSeconds(3f);
                    playerLocation = viewer.worldLocationToMazeLocation(player.transform.position);
                    monster.OnPlayerCalling(playerToMonster, puppyToMonster, playerLocation, puppy.Location);
                    ShowMonsterEcho();
                }
                else
                {
                    yield return new WaitForSeconds(3f);
                    // otherwise leads the monster to player
                    playerLocation = viewer.worldLocationToMazeLocation(player.transform.position);
                    monster.OnPlayerCalling(playerToMonster, -1, playerLocation, puppy.Location);
                    ShowMonsterEcho();
                }
            }
        }
        void AskExit()
        {
            StartCoroutine(AskExitCR());
            IEnumerator AskExitCR()
            {
                PlayerSays($"Can you feel the exit {GameStaticData.DOGGO_NAME}?");
                yield return new WaitForSeconds(2f);

                MazeLocation reed = viewer.worldLocationToMazeLocation(player.transform.position);
                AStarSearch wow = new AStarSearch();
                var pathToHeaven = wow.ComputePath(dungeon.Maze, reed, exit);
                if (pathToHeaven.Count > 1)
                {
                    MazeLocation tipCell = pathToHeaven[1];
                    int dirIndex = dungeon.Maze.GetDirection(reed, tipCell);
                    viewer.ShowPathToExitUI(reed, dirIndex);
                    AudioSystem.Instance.PlaySFXAtWorldPoint(doggoAudio, puppy.transform.position, 1);

                    monster.OnPlayerCalling(1, -1, reed, reed);
                    ShowMonsterEcho();
                }
            }
        }
    }

    public int GetDistance(MazeLocation start, MazeLocation mazeLocation)
    {
        var path = new AStarSearch().ComputePath(dungeon.Maze, start, mazeLocation);
        return path.Count - 1;
    }

    public Vector3 GetNextLocation(MazeLocation start, MazeLocation end)
    {
        AStarSearch aStar = new AStarSearch();
        var path = aStar.ComputePath(dungeon.Maze, start, end);
        if (path.Count > 1)
        {
            return viewer.MazeLocationToWorldLocation(path[1]);
        }
        else
        {
            return viewer.MazeLocationToWorldLocation(start);
        }
    }

    public int PlayerToMonster()
    {
        MazeLocation playerLocation = viewer.worldLocationToMazeLocation(player.transform.position);
        MazeLocation monsterLocation = viewer.worldLocationToMazeLocation(monster.transform.position);
        return this.GetDistance(playerLocation, monsterLocation);
    }

    public void ShowDoggoEcho(float soundStrength = 1)
    {
        MazeLocation doggo = viewer.worldLocationToMazeLocation(puppy.transform.position);
        MazeLocation reed = viewer.worldLocationToMazeLocation(player.transform.position);
        var pathToPlayer = new AStarSearch().ComputePath(dungeon.Maze, doggo, reed);
        if (pathToPlayer.Count > 1)
        {
            int dir = dungeon.Maze.GetDirection(reed, pathToPlayer[pathToPlayer.Count - 2]);
            viewer.ShowDoggoEchoUI(reed, dir);
        }
        AudioSystem.Instance.PlaySFXAtWorldPoint(doggoAudio, puppy.transform.position, soundStrength);
    }
    public void ShowMonsterEcho(float soundStrength = 1)
    {
        MazeLocation m = viewer.worldLocationToMazeLocation(monster.transform.position);
        MazeLocation reed = viewer.worldLocationToMazeLocation(player.transform.position);
        var pathToPlayer = new AStarSearch().ComputePath(dungeon.Maze, m, reed);
        if (pathToPlayer.Count > 1)
        {
            int dir = dungeon.Maze.GetDirection(reed, pathToPlayer[pathToPlayer.Count - 2]);
            viewer.ShowMonsterEchoUI(reed, dir);
        }
        AudioSystem.Instance.PlaySFXAtWorldPoint(monsterAudio, monster.transform.position, soundStrength);
    }
    public void PlayerSays(string text)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        NotificationText no = Instantiate(notificationTemplate, canvas.transform);
        no.GetComponent<RectTransform>().position = screenPos;
        no.gameObject.SetActive(true);
        no.SetOffset(Vector3.up * 100f);
        no.Action(text, player.gameObject);
    }
    public void OnPlayerFoundDog()
    {
        foundPuppy = true;
        int minDstToExit = 7;
        MazeLocation reed = viewer.worldLocationToMazeLocation(player.transform.position);
        AStarSearch a = new AStarSearch();
        var allLocations = dungeon.Maze.GetAllLocations();
        var exitCandidates = allLocations.FindAll(l => a.ComputePath(dungeon.Maze, reed, l).Count > minDstToExit);
        if (exitCandidates.Count > 0)
        {
            exit = exitCandidates[UnityEngine.Random.Range(0, exitCandidates.Count)];
        }
        else
        {
            exit = allLocations[UnityEngine.Random.Range(0, allLocations.Count)];
        }
        viewer.SetExitLocation(exit);
    }
    public void OnPlayerDead()
    {
        if (!playerDead)
        {
            playerDead = true;
            StartCoroutine(PlayerDeadCR());
        }
        IEnumerator PlayerDeadCR()
        {
            PlayerSays($"OMG THE MONSTER!!!");
            yield return new WaitForSeconds(4f);
            PlayerSays($"I'm scared to DEATH!!");
            yield return new WaitForSeconds(4f);
            fader.FadeToBlack(2, () => SceneManager.LoadScene("SadSadSad"));
        }
    }
    public void OnPlayerFoundExit()
    {
        if (!playerFoundExit)
        {
            StartCoroutine(GameEndCR());
        }
        IEnumerator GameEndCR()
        {
            playerFoundExit = true;
            player.controllable = false;
            PlayerSays($"OMG {GameStaticData.DOGGO_NAME} you found the exit!!");
            yield return new WaitForSeconds(4f);
            PlayerSays($"Let's get out of here!!");
            yield return new WaitForSeconds(4f);
            PlayerSays($"Wryyyyyyyyyyyyyyyyyyyyyyyyy");
            yield return new WaitForSeconds(4f);
            fader.FadeToBlack(2, () => SceneManager.LoadScene("HappyEnding"));

        }
    }

    public void OnPuppyDeath()
    {
        StartCoroutine(PuppyDeadCR());
        IEnumerator PuppyDeadCR()
        {
            PlayerSays($"No! My Puppy!");
            yield return new WaitForSeconds(4f);
            fader.FadeToBlack(2, () => SceneManager.LoadScene("SadSadSad"));
        }
    }
}
