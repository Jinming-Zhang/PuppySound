using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUIOverlay : MonoBehaviour
{
    [SerializeField]
    Image renderImage;
    [SerializeField]
    Image blockRaycastImage;
    Coroutine countingDown;
    private void Start()
    {
        blockRaycastImage.gameObject.SetActive(false);
        renderImage.enabled = false;
    }

    [ContextMenu("test")]
    public void Action(float cd)
    {
        blockRaycastImage.gameObject.SetActive(true);
        renderImage.enabled = true;
        renderImage.fillAmount = 1;
        if (countingDown == null)
        {
            countingDown = StartCoroutine(countDownCR(cd));
        }
    }
    IEnumerator countDownCR(float cd)
    {
        float delta = 1 / cd * Time.deltaTime;
        while (renderImage.fillAmount > 0)
        {
            renderImage.fillAmount -= delta;
            yield return new WaitForEndOfFrame();
        }
        blockRaycastImage.gameObject.SetActive(false);
        renderImage.enabled = false;
        countingDown = null;
    }
}
