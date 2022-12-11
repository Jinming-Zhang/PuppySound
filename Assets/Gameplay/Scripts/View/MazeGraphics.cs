using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeGraphics : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer graphic;
    [SerializeField]
    Collider clder;

    [Header("Echos")]
    [SerializeField]
    List<Transform> tipLocations;
    [SerializeField]
    GameObject echosTemplate;
    [SerializeField]
    List<Transform> wallAnchors;
    [SerializeField]
    GameObject wallTemplate;

    List<SpriteRenderer> wallSprites = new List<SpriteRenderer>();

    float targetAlpha = 1;
    public abstract float TransitionSpeed { get; }
    private void Update()
    {
        float delta = 1 / TransitionSpeed * Time.deltaTime;
        if (graphic.color.a != targetAlpha)
        {
            Color c = graphic.color;
            c.a = targetAlpha - graphic.color.a > 0 ? c.a + delta : c.a - delta;
            c.a = Mathf.Clamp01(c.a);
            graphic.color = c;

            wallSprites.ForEach(s =>
            {
                Color sc = s.color;
                sc.a = targetAlpha - s.color.a > 0 ? sc.a + delta : sc.a - delta;
                sc.a = Mathf.Clamp01(sc.a);
                s.color = sc;
            });
        }
    }

    public void SetSprite(Sprite sprite)
    {
        graphic.sprite = sprite;
    }

    public void Show()
    {
        targetAlpha = 1;
    }

    public void Hide()
    {
        targetAlpha = 0;
    }

    public void SetCollider(bool activated)
    {
        clder.enabled = activated;
    }

    public void ShowTip(Color color, int dirIndex, bool outward = false)
    {
        GameObject destroyLater = Instantiate(echosTemplate, tipLocations[dirIndex].transform);
        destroyLater.GetComponentInChildren<SpriteRenderer>().color = color;
        switch (dirIndex)
        {
            case 0:
                destroyLater.transform.rotation = outward ? Quaternion.Euler(0, 0, 180) : Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                destroyLater.transform.rotation = outward ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, -90);
                break;
            case 2:
                destroyLater.transform.rotation = outward ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, -180);
                break;
            case 3:
                destroyLater.transform.rotation = outward ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, -270);
                break;
            default:
                break;
        }
        StartCoroutine(DestroyAfterSecCR(destroyLater.gameObject, 2));
    }

    public void DrawWall(List<GridMazeModel.DirectionInfo> availableDirs)
    {
        List<GridMazeModel.MazeDirection> dirs = availableDirs.ConvertAll(info => info.Direction);
        List<GridMazeModel.MazeDirection> allDirs = new List<GridMazeModel.MazeDirection>() { GridMazeModel.MazeDirection.Top, GridMazeModel.MazeDirection.Right, GridMazeModel.MazeDirection.Bottom, GridMazeModel.MazeDirection.Left };
        foreach (var dir in dirs)
        {
            allDirs.Remove(dir);
        }

        foreach (int dirIndex in allDirs)
        {
            GameObject wallGo = Instantiate(wallTemplate, wallAnchors[dirIndex].transform);
            wallSprites.Add(wallGo.GetComponentInChildren<SpriteRenderer>());
            switch (dirIndex)
            {
                case 0:
                    wallGo.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 1:
                    wallGo.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case 2:
                    wallGo.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case 3:
                    wallGo.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator DestroyAfterSecCR(GameObject target, float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(target);
    }
}
