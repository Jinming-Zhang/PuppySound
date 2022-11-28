using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    IMazeModel mazeModel;
    public IMazeModel Maze => mazeModel;

    MazeLocation playerLocation;
    MazeLocation puppyLocation;

    [SerializeField]
    private int playerToPuppyMinLength = 7;
    [SerializeField]
    private int playerToPuppyMaxLength = 10;

    [SerializeField]
    private int monsterToPlayerLengthMax = 5;
    [SerializeField]
    private int monsterToPlayerLengthMin = 3;
    [SerializeField]
    private int monsterToPuppyLength = 3;

    public void InitializeMaze(int height, int width, bool wrapped, int interconnectivity)
    {
        mazeModel = new GridMazeModel(height, width);
        var k = new KruskalAlgorithm();

        List<MazeEdge> leftoverEdges = k.Generate(mazeModel);
        for (int i = 0; i < interconnectivity; i++)
        {
            mazeModel.AddEdge(leftoverEdges[i]);
        }
    }

    public MazeLocation InitializePlayerMazeLocation()
    {
        int row = Random.Range(0, Maze.Height);
        int col = Random.Range(0, Maze.Width);
        playerLocation = new GridMazeLocation(row, col);

        return playerLocation;
    }

    public MazeLocation InitializePuppyLocation()
    {
        AStarSearch s = new AStarSearch();
        List<MazeLocation> allLocations = mazeModel.GetAllLocations();

        var candidates = allLocations.FindAll(l => s.ComputePath(Maze, l, playerLocation).Count >= playerToPuppyMinLength
        && s.ComputePath(Maze, l, playerLocation).Count <= playerToPuppyMaxLength);
        if (candidates.Count > 0)
        {
            puppyLocation = candidates[Random.Range(0, candidates.Count)];
        }
        else
        {
            puppyLocation = allLocations[Random.Range(0, allLocations.Count)];
        }
        return puppyLocation;

    }

    public MazeLocation InitializeMonsterLocation()
    {
        AStarSearch s = new AStarSearch();
        List<MazeLocation> allLocations = mazeModel.GetAllLocations();

        var candidates = allLocations.FindAll(l => s.ComputePath(Maze, l, playerLocation).Count >= monsterToPlayerLengthMin
        && s.ComputePath(Maze, l, playerLocation).Count <= monsterToPlayerLengthMax
        && s.ComputePath(Maze, l, puppyLocation).Count > monsterToPuppyLength);
        if (candidates.Count > 0)
        {
            return candidates[Random.Range(0, candidates.Count)];
        }
        else
        {
            return allLocations[Random.Range(0, allLocations.Count)];
        }

    }

}
