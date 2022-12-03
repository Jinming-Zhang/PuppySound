using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeEdgeGraphics : MazeGraphics
{
    [SerializeField]
    float transitionSpeed = .5f;

    public override float TransitionSpeed => transitionSpeed;
}
