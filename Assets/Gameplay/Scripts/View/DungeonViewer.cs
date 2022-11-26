using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonViewer : MonoBehaviour
{
    [Header("Renderer Settings")]
    [SerializeField]
    GameObject mazeCellGraphic;
    [SerializeField]
    Material cellMaterial;
    [SerializeField]
    Material edgeMaterial;
    [SerializeField]
    float renderDepth = 0;

    [Header("Display Settings")]
    [SerializeField]
    float cellGap = 2f;
    [SerializeField]
    float edgeWidth = .5f;

    [Header("Child Management")]
    [SerializeField]
    Transform cellRoot;
    [SerializeField]
    Transform edgeRoot;

    // local members
    GameObject[,] mazeCells;
    List<LineRenderer> edges;

    IMazeModel mazeModel;

    public void setMaze(IMazeModel newMaze)
    {
        mazeModel = newMaze;
        updateGraphics();
    }

    void updateGraphics()
    {
        foreach (Transform c in cellRoot)
        {
            Destroy(c.gameObject);
        }
        foreach (Transform e in edgeRoot)
        {
            Destroy(e.gameObject);
        }
        updateCells();
        updateEdges();
    }

    private void updateCells()
    {
        mazeCells = new GameObject[mazeModel.Height, mazeModel.Width];
        for (int r = 0; r < mazeModel.Height; ++r)
        {
            for (int c = 0; c < mazeModel.Width; ++c)
            {
                // cells
                GameObject cell = Instantiate(mazeCellGraphic);
                cell.transform.parent = cellRoot;
                mazeCells[r, c] = cell;
                cell.transform.position = GetMazeLocation(new GridMazeLocation(r, c));
                cell.GetComponentInChildren<MeshRenderer>().material = new Material(cellMaterial);
            }
        }
    }

    void updateEdges()
    {
        edges = new List<LineRenderer>();
        List<MazeEdge> mazeEdges = mazeModel.GetAllEdges();
        foreach (MazeEdge e in mazeEdges)
        {
            edges.Add(AddEdgeGraphic(e).GetComponent<LineRenderer>());
        }
    }

    private GameObject AddEdgeGraphic(MazeEdge e)
    {
        GameObject edgeGo = new GameObject();
        edgeGo.transform.parent = edgeRoot;
        edgeGo.transform.position = Vector3.zero;

        LineRenderer edge = edgeGo.AddComponent<LineRenderer>();
        edge.startWidth = edgeWidth;
        edge.endWidth = edgeWidth;

        MazeLocation start = e.GetStartLocation();
        MazeLocation dst = e.GetEndLocation();
        Vector3 startPos = GetMazeLocation(start);
        Vector3 endPos = GetMazeLocation(dst);

        ////edge.SetPosition(0, startCell.transform.position);
        //edge.SetPosition(1, dstCell.transform.position);
        edge.SetPosition(0, startPos);
        edge.SetPosition(1, endPos);

        edge.material = new Material(edgeMaterial);
        return edgeGo;
    }
    (float, float) LocationToCoord(MazeLocation l)
    {
        return (l.Row * cellGap, l.Col * cellGap);
    }

    private Vector3 GetMazeLocation(MazeLocation location)
    {
        int row = location.Row;
        int col = location.Col;
        float worldX = col * cellGap;
        float worldY = row * cellGap;
        return new Vector3(worldX, worldY, renderDepth);
    }

}
