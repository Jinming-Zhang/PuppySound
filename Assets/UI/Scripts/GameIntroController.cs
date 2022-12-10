using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIntroController : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip introClip;
    [SerializeField]
    AudioClip startMenuClip;

    [Header("UI")]
    [SerializeField]
    TransitionPanel transitionPanel;
    [SerializeField]
    List<TransitionPanel.TransitionData> introData;
    [SerializeField]
    StartMenu startPanel;

    // Start is called before the first frame update
    void Start()
    {
        ShowStartMenu();

    }
    void ShowStartMenu()
    {
        transitionPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
    }

    public void OnPlayerFinishedInput()
    {
        transitionPanel.gameObject.SetActive(true);
        startPanel.gameObject.SetActive(false);
        if (introData != null)
        {
            audioSource.clip = introClip;
            audioSource.loop = true;
            audioSource.Play();
            introData.ForEach(d => transitionPanel.AddTransitionTask(d));
            transitionPanel.Action(() =>
            {
                audioSource.clip = startMenuClip;
                audioSource.loop = true;
                audioSource.Play();
                transitionPanel.gameObject.SetActive(false);
                startPanel.gameObject.SetActive(true);
                startPanel.ShowMenu();
            });
            startPanel.gameObject.SetActive(false);
        }
    }
}
