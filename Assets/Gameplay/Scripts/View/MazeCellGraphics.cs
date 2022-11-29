using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellGraphics : MazeGraphics
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
