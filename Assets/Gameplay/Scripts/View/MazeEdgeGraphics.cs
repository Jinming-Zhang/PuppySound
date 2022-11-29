using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeEdgeGraphics : MazeGraphics
{
    [SerializeField]
    SpriteRenderer graphic;
    public override void Hide()
    {
        graphic.enabled = false;
    }

    public override void Show()
    {
        graphic.enabled = true;
    }
}
