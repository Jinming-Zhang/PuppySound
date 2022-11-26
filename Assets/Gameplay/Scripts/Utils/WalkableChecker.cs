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

    public bool IsOnBoard()
    {
        bool isOnBoard = true;
        foreach (Transform bound in bounds)
        {
            RaycastHit[] hits = Physics.RaycastAll(bound.position, Vector3.forward * 2, rayCheckLayer);
            bool hitSurface = false;
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("MazeSurface"))
                {
                    hitSurface = true;
                }
            }
            isOnBoard = isOnBoard && hitSurface;
        }
        return isOnBoard;
    }
    private void OnDrawGizmos()
    {
        foreach (Transform bound in bounds)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(bound.position, bound.position + Vector3.forward * 2);
        }
    }

    private void Update()
    {
        Debug.Log(IsOnBoard());
    }
}
