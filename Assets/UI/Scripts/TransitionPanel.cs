using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionPanel : MonoBehaviour
{
    [SerializeField]
    CanvasGroup cg;
    [SerializeField]
    Image background;
    [SerializeField]
    TMPro.TextMeshProUGUI tmpText;

    [SerializeField]
    float transInDuration = 1f;
    [SerializeField]
    float stayDuration = 2f;
    [SerializeField]
    float transOutDuration = 1f;
    [SerializeField]
    float durationBetweenTransition = 1f;

    List<TransitionData> transitionQueue = new List<TransitionData>();

    Action onTransitionFinished;
    bool shutup;
    public void AddTransitionTask(TransitionData task)
    {
        transitionQueue.Add(task);
    }
    public void Action(Action finishedCB = null)
    {
        shutup = false;
        onTransitionFinished = finishedCB;
        StopAllCoroutines();
        StartCoroutine(TransitionCR());
    }
    public void idontwantyourstorystopstopstopstopstop()
    {
        transitionQueue.Clear();
        shutup = true;
    }

    IEnumerator TransitionCR()
    {
        while (transitionQueue.Count > 0)
        {
            // initialize
            cg.alpha = 0;
            TransitionData t = transitionQueue[0];
            transitionQueue.RemoveAt(0);
            if (string.IsNullOrEmpty(t.text))
            {
                tmpText.text = string.Empty;
            }
            else
            {
                tmpText.text = t.text.Replace("player_name", GameStaticData.PLAYER_NAME).Replace("puppy_name", GameStaticData.DOGGO_NAME); ;
            }
            if (t.sprite)
            {
                background.sprite = t.sprite;
            }
            // transition in
            while (cg.alpha < 1)
            {
                float delta = 1f / transInDuration * Time.deltaTime;
                cg.alpha = Mathf.Min(1, cg.alpha + delta);
                yield return new WaitForEndOfFrame();
            }
            if (shutup)
            {
                break;
            }
            // stay
            yield return new WaitForSeconds(stayDuration);
            if (!t.stay)
            {
                // transition out
                while (cg.alpha > 0)
                {
                    float delta = 1f / transOutDuration * Time.deltaTime;
                    cg.alpha = Mathf.Max(0, cg.alpha - delta);
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForSeconds(durationBetweenTransition);
        }
        onTransitionFinished?.Invoke();
    }

    [Serializable]
    public class TransitionData
    {
        public string text;
        public Sprite sprite;
        public bool stay = false;
        public TransitionData(string text = "", Sprite sprite = null, bool stay = false)
        {
            this.text = text;
            this.sprite = sprite;
            this.stay = stay;
        }
    }
}
