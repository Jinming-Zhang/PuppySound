using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeGraphics : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer graphic;
    [SerializeField]
    Collider clder;
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
        }
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
}
