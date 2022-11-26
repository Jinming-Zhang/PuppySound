using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonViewer : MonoBehaviour
{
    [Header("Renderer Settings")]
    [SerializeField]
    GameObject mazeCellGraphic;
    [SerializeField]
    GameObject mazeEdgeGraphic;
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
                //cell.GetComponentInChildren<MeshRenderer>().material = new Material(cellMaterial);
            }
        }
    }

    void updateEdges()
    {
        edges = new List<LineRenderer>();
        List<MazeEdge> mazeEdges = mazeModel.GetAllEdges();
        foreach (MazeEdge e in mazeEdges)
        {
            //edges.Add(AddEdgeLineRendererGraphic(e).GetComponent<LineRenderer>());
            AddEdgePlaneGraphic(e);
        }
    }
    private GameObject AddEdgePlaneGraphic(MazeEdge e)
    {
        MazeLocation l1 = e.GetStartLocation();
        MazeLocation l2 = e.GetEndLocation();

        Vector3 startPos = GetMazeLocation(l1);
        Vector3 endPos = GetMazeLocation(l2);

        GameObject edgeGo = Instantiate(mazeEdgeGraphic);
        edgeGo.transform.parent = edgeRoot;

        edgeGo.transform.position = startPos;
        float dist = (endPos - startPos).magnitude;
        // vertical edge, scale and move on y
        if (l1.Col == l2.Col)
        {
            edgeGo.transform.localScale = new Vector3(.5f, dist, 1);
            int sign = endPos.y - startPos.y < 0 ? -1 : 1;
            edgeGo.transform.position = new Vector3(startPos.x, startPos.y + sign * (dist / 2.0f), startPos.z + 0.01f);
        }
        // horizontal edge, scale and move on x
        else
        {
            edgeGo.transform.localScale = new Vector3(dist, .5f, 1);
            int sign = endPos.x - startPos.x < 0 ? -1 : 1;
            edgeGo.transform.position = new Vector3(startPos.x + sign * dist / 2.0f, startPos.y, startPos.z + 0.01f);
        }

        //edge.material = new Material(edgeMaterial);
        return edgeGo;
    }

    private GameObject AddEdgeLineRendererGraphic(MazeEdge e)
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

        edge.SetPosition(0, startPos);
        edge.SetPosition(1, endPos);

        //edge.material = new Material(edgeMaterial);
        return edgeGo;
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
