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
        if (introData != null)
        {
            audioSource.clip = introClip;
            audioSource.loop = true;
            audioSource.Play();
            introData.ForEach(d => transitionPanel.AddTransitionTask(d));
            transitionPanel.Action(() => ShowStartMenu());
            startPanel.gameObject.SetActive(false);
        }
    }
    void ShowStartMenu()
    {
        audioSource.clip = startMenuClip;
        audioSource.loop = true;
        audioSource.Play();
        transitionPanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        //() => SceneManager.LoadScene(1)
    }
}
