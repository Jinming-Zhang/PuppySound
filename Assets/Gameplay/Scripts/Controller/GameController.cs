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
        monster = viewer.InstantiateMonster(monsterL).GetComponent<Monster>();
    }


    public void Calling()
    {
        MazeLocation playerLocation = viewer.worldLocationToMazeLocation(player.transform.position);
        MazeLocation monsterLocation = viewer.worldLocationToMazeLocation(monster.transform.position);
        MazeLocation puppyLocation = puppy.GetLocation;
        int playerToMonster = Utility.calculateSoundStrength(playerLocation, monsterLocation, player.GetSoundStrength);
        int puppyToMonster = Utility.calculateSoundStrength(puppyLocation, monsterLocation, player.GetSoundStrength);

        bool puppyBark = puppy.called();
        if (puppyBark)
        {
            // monster is notified puppy bark.
        }

        // monster notified player sound.
    }

    public int getDistance(MazeLocation start, MazeLocation mazeLocation)
    {
        var path = new AStarSearch().ComputePath(dungeon.Maze, start, mazeLocation);
        return path.Count - 1;
    }

}
