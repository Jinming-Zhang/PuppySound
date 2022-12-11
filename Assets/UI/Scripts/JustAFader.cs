using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustAFader : MonoBehaviour
{
    [SerializeField]
    CanvasGroup cg;
    Action cb;
    private void Start()
    {
        FadeoutFromBlack(2);
    }
    public void FadeToBlack(float duration, Action cb = null)
    {
        this.cb = cb;
        gameObject.SetActive(true);
        cg.alpha = 0;
        StartCoroutine(FadeCR(1, duration));
    }
    public void FadeoutFromBlack(float duration, Action cb = null)
    {
        this.cb = cb;
        gameObject.SetActive(true);
        cg.alpha = 1;
        StartCoroutine(FadeCR(-1, duration));
    }
    IEnumerator FadeCR(float amt, float duration)
    {
        float counter = 0;
        while (counter < Mathf.Abs(amt))
        {
            float delta = amt / duration * Time.deltaTime;
            counter += Mathf.Abs(delta);
            cg.alpha += delta;
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
        cb?.Invoke();
    }
}
