using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class MazeCellGraphics : MazeGraphics
{
    [SerializeField]
    float transitionSpeed = .5f;
    public override float TransitionSpeed => transitionSpeed;
}
