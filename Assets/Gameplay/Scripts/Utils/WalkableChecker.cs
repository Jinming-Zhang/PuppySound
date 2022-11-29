using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableChecker : MonoBehaviour
{
    [SerializeField]
    List<Transform> bounds;
    [SerializeField]
    LayerMask rayCheckLayer;
    Vector3 offset;

    List<MazeGraphics> steppingOn = new List<MazeGraphics>();
    public List<MazeGraphics> SteppingOn => steppingOn;
    public bool IsOnBoard(Vector3 offset)
    {
        bool isOnBoard = true;
        this.offset = offset;
        steppingOn.Clear();
        foreach (Transform bound in bounds)
        {
            RaycastHit[] hits = Physics.RaycastAll(bound.position + offset, Vector3.forward * 2, rayCheckLayer);
            bool hitSurface = false;
            // top
            RaycastHit hit = hits[0];
            if (hit.transform.CompareTag("MazeSurface"))
            {
                hitSurface = true;
            }
            MazeGraphics s = hit.transform.GetComponent<MazeGraphics>();
            if (s && !steppingOn.Contains(s))
            {
                steppingOn.Add(s);
            }
            //foreach (RaycastHit hit in hits)
            //{
            //    if (hit.transform.CompareTag("MazeSurface"))
            //    {
            //        hitSurface = true;
            //    }
            //    MazeGraphics s = hit.transform.GetComponent<MazeGraphics>();
            //    if (s && !steppingOn.Contains(s))
            //    {
            //        steppingOn.Add(s);
            //    }
            //}
            isOnBoard = isOnBoard && hitSurface;
        }
        return isOnBoard;
    }
    private void OnDrawGizmos()
    {
        foreach (Transform bound in bounds)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(bound.position + offset, bound.position + offset + Vector3.forward * 2);
        }
    }
}
