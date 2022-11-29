using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonViewer : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject puppy;
    [SerializeField]
    GameObject monster;

    [Header("Renderer Settings")]
    [SerializeField]
    MazeGraphics mazeCellGraphic;
    [SerializeField]
    MazeGraphics mazeEdgeGraphic;
    Material cellMaterial;
    Material edgeMaterial;
    [SerializeField]
    float mazeDepth = 0;
    [SerializeField]
    float characterDepth = -1;

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
    MazeGraphics[,] mazeCells;
    List<MazeGraphics> mazeEdgeGraphics;

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
        mazeCells = new MazeGraphics[mazeModel.Height, mazeModel.Width];
        for (int r = 0; r < mazeModel.Height; ++r)
        {
            for (int c = 0; c < mazeModel.Width; ++c)
            {
                // cells
                MazeGraphics cell = Instantiate(mazeCellGraphic);
                cell.transform.parent = cellRoot;
                mazeCells[r, c] = cell;
                cell.transform.position = MazeLocationToWorldLocation(new GridMazeLocation(r, c));
            }
        }
    }

    void updateEdges()
    {
        List<MazeEdge> mazeEdges = mazeModel.GetAllEdges();
        mazeEdgeGraphics = new List<MazeGraphics>();
        foreach (MazeEdge e in mazeEdges)
        {
            mazeEdgeGraphics.Add(AddEdgePlaneGraphic(e));
        }
    }

    private MazeGraphics AddEdgePlaneGraphic(MazeEdge e)
    {
        MazeLocation l1 = e.GetStartLocation();
        MazeLocation l2 = e.GetEndLocation();

        Vector3 startPos = MazeLocationToWorldLocation(l1);
        Vector3 endPos = MazeLocationToWorldLocation(l2);

        MazeGraphics edgeGo = Instantiate(mazeEdgeGraphic);
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

    public GameObject InstantiatePlayer(MazeLocation l)
    {
        GameObject p = Instantiate(player);
        Vector3 worldPos = MazeLocationToWorldLocation(l);
        worldPos.z = characterDepth;
        p.transform.position = worldPos;

        return p;
    }
    public GameObject InstantiatePuppy(MazeLocation l)
    {
        GameObject p = Instantiate(puppy);
        Vector3 worldPos = MazeLocationToWorldLocation(l);
        worldPos.z = characterDepth;
        p.transform.position = worldPos;
        return p;

    }
    public GameObject InstantiateMonster(MazeLocation l)
    {
        GameObject m = Instantiate(monster);
        Vector3 worldPos = MazeLocationToWorldLocation(l);
        worldPos.z = characterDepth;
        m.transform.position = worldPos;
        return m;
    }
    public void UpdateGraphicsByPlayerPosition(List<MazeGraphics> playerOn)
    {
        // loop through cell graphics
        for (int r = 0; r < mazeModel.Height; r++)
        {
            for (int c = 0; c < mazeModel.Width; c++)
            {
                if (playerOn.Contains(mazeCells[r, c]))
                {
                    mazeCells[r, c].Show();
                }
                else
                {
                    mazeCells[r, c].Hide();
                }
            }
        }
        // loop through edge graphics
        mazeEdgeGraphics.ForEach(g =>
        {
            if (playerOn.Contains(g))
            {
                g.Show();
            }
            else
            {
                g.Hide();
            }
        });
    }

    public Vector3 MazeLocationToWorldLocation(MazeLocation location)
    {
        int row = location.Row;
        int col = location.Col;
        float worldX = col * cellGap;
        float worldY = row * cellGap;
        return new Vector3(worldX, worldY, mazeDepth);
    }

    public MazeLocation worldLocationToMazeLocation(Vector3 location)
    {
        int row = 0;
        int col = 0;
        float distance = float.MaxValue;
        for (int r = 0; r < mazeModel.Height; ++r)
        {
            for (int c = 0; c < mazeModel.Width; ++c)
            {
                MazeGraphics cell = mazeCells[r, c];
                float d = Vector3.Distance(location, cell.transform.position);
                if (d < distance)
                {
                    row = r;
                    col = c;
                    distance = d;
                }
            }
        }
        return new GridMazeLocation(row, col);
    }
}
