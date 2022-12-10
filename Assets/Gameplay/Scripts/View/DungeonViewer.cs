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
    [SerializeField]
    float mazeDepth = 0;
    [SerializeField]
    float characterDepth = -1;
    [SerializeField]
    Sprite floor1;
    [SerializeField]
    Sprite floor2;
    [SerializeField]
    Sprite floor3;

    [Header("Display Settings")]
    [SerializeField]
    float cellScale = 1f;

    [Header("Child Management")]
    [SerializeField]
    Transform cellRoot;

    // local members
    MazeGraphics[,] mazeCells;
    List<MazeGraphics> mazeEdgeGraphics;

    IMazeModel mazeModel;
    public bool showAll = false;
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
        updateCells();
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
                cell.transform.localScale = Vector3.one * cellScale;
                cell.transform.parent = cellRoot;
                mazeCells[r, c] = cell;
                cell.gameObject.name = $"Maze Cell [{r},{c}]";
                cell.transform.position = MazeLocationToWorldLocation(new GridMazeLocation(r, c));

                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        cell.SetSprite(floor1);
                        break;
                    case 1:
                        cell.SetSprite(floor2);
                        break;
                    case 2:
                        cell.SetSprite(floor3);
                        break;
                    default:
                        cell.SetSprite(floor1);
                        break;
                }

            }
        }
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
                    if (showAll)
                    {
                        mazeCells[r, c].Show();
                    }
                    else
                    {
                        mazeCells[r, c].Hide();
                    }
                }
            }
        }
    }
    public void UpdateMazeCellCollidersBasedOnPlayerPosition(Vector3 worldPos)
    {
        MazeLocation mazeLoc = worldLocationToMazeLocation(worldPos);
        List<MazeLocation> availableDsts = mazeModel.AvailableDirections(mazeLoc).ConvertAll(info => info.Distination);
        for (int r = 0; r < mazeModel.Height; r++)
        {
            for (int c = 0; c < mazeModel.Width; c++)
            {
                if (availableDsts.Contains(new GridMazeLocation(r, c)))
                {
                    mazeCells[r, c].SetCollider(true);
                }
                else
                {
                    mazeCells[r, c].SetCollider(false);
                }
            }
        }
        mazeCells[mazeLoc.Row, mazeLoc.Col].SetCollider(true);
    }

    public Vector3 MazeLocationToWorldLocation(MazeLocation location)
    {
        int row = location.Row;
        int col = location.Col;
        float worldX = col * cellScale;
        float worldY = row * cellScale;
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

    public void ShowDoggoEchoUI(MazeLocation reedLoc, int dirIndex)
    {
        mazeCells[reedLoc.Row, reedLoc.Col].ShowTip(Color.yellow, dirIndex);
    }
    public void ShowMonsterEchoUI(MazeLocation reedLoc, int dirIndex)
    {
        mazeCells[reedLoc.Row, reedLoc.Col].ShowTip(Color.red, dirIndex);
    }
}
