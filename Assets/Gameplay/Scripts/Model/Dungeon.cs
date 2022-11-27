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

        var candidates = allLocations.FindAll(l => s.ComputePath(Maze, l, playerLocation).Count > 5);
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

        var candidates = allLocations.FindAll(l => s.ComputePath(Maze, l, playerLocation).Count > 5 && s.ComputePath(Maze, l, puppyLocation).Count > 5);
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
