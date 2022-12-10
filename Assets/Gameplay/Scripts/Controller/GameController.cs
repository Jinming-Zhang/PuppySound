using GameCore.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    [SerializeField]
    private Player player;
    [SerializeField]
    private Puppy puppy;
    [SerializeField]
    private Monster monster;
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
    [Header("Lazy Lazy")]
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
    }
    private void Update()
    {
        viewer.showAll = debugging;
        viewer.UpdateGraphicsByPlayerPosition(player.GetComponent<WalkableChecker>().SteppingOn);
        MazeLocation poppyLocation = viewer.worldLocationToMazeLocation(puppy.transform.position);
        MazeLocation reedLocation = viewer.worldLocationToMazeLocation(player.transform.position);
        MazeLocation monsterLocation = viewer.worldLocationToMazeLocation(monster.transform.position);
        if (poppyLocation != reedLocation && !debugging)
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
            MazeLocation puppyLocation = puppy.Location;
            int playerToMonster = Utility.calculateSoundStrength(dungeon.Maze, playerLocation, monsterLocation, player.GetSoundStrength);

            // if puppy barks, lead monster to the closest one.
            bool puppyBark = puppy.called();
            if (puppyBark)
            {
                int puppyToMonster = Utility.calculateSoundStrength(dungeon.Maze, puppyLocation, monsterLocation, player.GetSoundStrength);
                ShowDoggoEcho();
                yield return new WaitForSeconds(3f);
                monster.OnPlayerCalling(playerToMonster, puppyToMonster, playerLocation, puppyLocation);
                ShowMonsterEcho();
            }
            else
            {
                yield return new WaitForSeconds(3f);
                // otherwise leads the monster to player
                monster.OnPlayerCalling(playerToMonster, -1, playerLocation, puppyLocation);
                ShowMonsterEcho();
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

    }
    public void OnPlayerDead()
    {

    }
    public void OnPlayerFoundExit()
    {

    }
}
