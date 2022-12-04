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
        }
    }
    void ShowStartMenu()
    {
        audioSource.clip = startMenuClip;
        audioSource.loop = true;
        audioSource.Play();
        //() => SceneManager.LoadScene(1)
    }
}
