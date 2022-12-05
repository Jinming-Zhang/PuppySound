using GameCore.Services;
using System;
using System.Collections;
using System.Collections.Generic;
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
        viewer.UpdateGraphicsByPlayerPosition(player.GetComponent<WalkableChecker>().SteppingOn);
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
        puppy.Location = puppyL;
        puppy.panicBar = this.puppyPanicBar;
        monster = viewer.InstantiateMonster(monsterL).GetComponent<Monster>();
        monster.Location = monsterL;
    }

    // triggered when Call button clicks
    public void Calling()
    {
        MazeLocation playerLocation = viewer.worldLocationToMazeLocation(player.transform.position);
        MazeLocation monsterLocation = viewer.worldLocationToMazeLocation(monster.transform.position);
        MazeLocation puppyLocation = puppy.Location;
        int playerToMonster = Utility.calculateSoundStrength(dungeon.Maze, playerLocation, monsterLocation, player.GetSoundStrength);

        // if puppy barks, lead monster to the closest one.
        bool puppyBark = puppy.called();
        if (puppyBark)
        {
            int puppyToMonster = Utility.calculateSoundStrength(dungeon.Maze, puppyLocation, monsterLocation, player.GetSoundStrength);
            monster.OnPlayerCalling(playerToMonster, puppyToMonster, playerLocation, puppyLocation);
        }
        else
        {
            // otherwise leads the monster to player
            monster.OnPlayerCalling(playerToMonster, -1, playerLocation, puppyLocation);
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
}
