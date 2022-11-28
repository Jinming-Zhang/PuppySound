using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;
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
        InitializeDungeon();
    }

    private void InitializeDungeon()
    {
        dungeon.InitializeMaze(mazeHeight, mazeWidth, wrapped, interconnectivity);
        viewer.setMaze(dungeon.Maze);
        MazeLocation playerL = dungeon.InitializePlayerMazeLocation();
        MazeLocation puppyL = dungeon.InitializePuppyLocation();
        MazeLocation monsterL = dungeon.InitializeMonsterLocation();

        player = viewer.InstantiatePlayer(playerL).GetComponent<Player>();

        puppy = viewer.InstantiatePuppy(puppyL).GetComponent<Puppy>();
        puppy.Location = puppyL;
        monster = viewer.InstantiateMonster(monsterL).GetComponent<Monster>();
        monster.Location = monsterL;
    }


    public void Calling()
    {
        MazeLocation playerLocation = viewer.worldLocationToMazeLocation(player.transform.position);
        MazeLocation monsterLocation = viewer.worldLocationToMazeLocation(monster.transform.position);
        MazeLocation puppyLocation = puppy.Location;
        int playerToMonster = Utility.calculateSoundStrength(playerLocation, monsterLocation, player.GetSoundStrength);

        bool puppyBark = puppy.called();
        if (puppyBark)
        {
            int puppyToMonster = Utility.calculateSoundStrength(puppyLocation, monsterLocation, player.GetSoundStrength);
            monster.OnPlayerCalling(playerToMonster, puppyToMonster, playerLocation, puppyLocation);
        }

        monster.OnPlayerCalling(playerToMonster, -1, playerLocation, puppyLocation);
    }

    public int getDistance(MazeLocation start, MazeLocation mazeLocation)
    {
        var path = new AStarSearch().ComputePath(dungeon.Maze, start, mazeLocation);
        return path.Count - 1;
    }

    public Vector3 getNextLocation(MazeLocation start, MazeLocation end)
    {
        AStarSearch aStar = new AStarSearch();
        var path =  aStar.ComputePath(dungeon.Maze, start, end);
        if (path.Count > 1)
        {
            return viewer.MazeLocationToWorldLocation(path[1]);
        }
        else
        {
            return viewer.MazeLocationToWorldLocation(start);
        }
    }
}
