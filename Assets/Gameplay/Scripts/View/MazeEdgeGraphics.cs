using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeEdgeGraphics : MazeGraphics
{
    [SerializeField]
    SpriteRenderer graphic;
    [SerializeField]
    float transitionSpeed = .5f;

    float targetAlpha = 1;
    public override void Hide()
    {
        targetAlpha = 0;
    }

    private void Update()
    {
        float delta = 1 / transitionSpeed * Time.deltaTime;
        if (graphic.color.a != targetAlpha)
        {
            Color c = graphic.color;
            c.a = targetAlpha - graphic.color.a > 0 ? c.a + delta : c.a - delta;
            c.a = Mathf.Clamp01(c.a);
            graphic.color = c;
        }
    }
    public override void Show()
    {
        targetAlpha = 1;
    }
}
