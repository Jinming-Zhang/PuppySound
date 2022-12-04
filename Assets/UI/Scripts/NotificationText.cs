using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationText : MonoBehaviour
{
    [SerializeField]
    CanvasGroup cg;
    [SerializeField]
    TMPro.TextMeshProUGUI tmp;
    [SerializeField]
    float stayTimer = 3f;
    [SerializeField]
    float fadeTimer = 2f;
    bool started = false;

    public void Action(string text)
    {
        if (!started)
        {
            started = true;
            tmp.text = text;
            StartCoroutine(LifeCR());
        }
    }

    IEnumerator LifeCR()
    {
        cg.alpha = 1;
        yield return new WaitForSeconds(stayTimer);
        float delta = 1f / fadeTimer;
        while (cg.alpha > 0)
        {
            cg.alpha = Mathf.Max(0, cg.alpha - delta * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
