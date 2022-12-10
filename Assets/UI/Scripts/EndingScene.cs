using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip endingClip;

    [Header("UI")]
    [SerializeField]
    TransitionPanel transitionPanel;
    [SerializeField]
    CanvasGroup btnGroup;
    [SerializeField]
    float showBtnDuration = 3f;
    [SerializeField]
    List<TransitionPanel.TransitionData> endData;
    float countdown;

    // Start is called before the first frame update
    void Start()
    {
        btnGroup.alpha = 0;
        audioSource.clip = endingClip;
        audioSource.loop = true;
        audioSource.Play();
        countdown = audioSource.clip.length;
        endData.ForEach(d => transitionPanel.AddTransitionTask(d));
        transitionPanel.Action(() =>
        {
            StartCoroutine(ShowBtnGroupCR());
        });
    }
    IEnumerator ShowBtnGroupCR()
    {
        while (btnGroup.alpha < 1)
        {
            btnGroup.alpha += (1.0f / showBtnDuration * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    public void ToStartScene()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown < 0)
        {
            SceneManager.LoadScene(0);
        }
    }

}
