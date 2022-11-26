using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    IMazeModel mazeModel;
    [SerializeField]
    DungeonViewer viewer;

    [SerializeField]
    int initialMazeHeight = 3;
    [SerializeField]
    int initialMazeWidth = 4;
    [SerializeField]
    int interconnectivity = 0;

    IMazeGenerationAlgorithm k;

    void Start()
    {
        ChangeNewMaze(initialMazeHeight, initialMazeWidth);
    }

    public void ChangeNewMaze(int height, int width)
    {
        mazeModel = new GridMazeModel(height, width);
        k = new KruskalAlgorithm();

        List<MazeEdge> leftoverEdges = k.Generate(mazeModel);
        for (int i = 0; i < interconnectivity; i++)
        {
            mazeModel.AddEdge(leftoverEdges[i]);
        }
        viewer.setMaze(mazeModel);
    }
}
