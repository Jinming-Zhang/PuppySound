using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    GameIntroController gameIntroController;
    [Header("Menus")]
    [SerializeField]
    CanvasGroup menuButtonsCG;
    [Header("Input Panel")]
    [SerializeField]
    CanvasGroup inputsCg;
    [SerializeField]
    TMPro.TMP_InputField playerNameInputField;
    [SerializeField]
    TMPro.TMP_InputField puppyNameInputField;
    [SerializeField]
    float fadeOutInputDuration = 2f;
    bool fadedInputPanel;
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        fadedInputPanel = false;
        inputsCg.alpha = 1;
        menuButtonsCG.gameObject.SetActive(false);
        inputsCg.gameObject.SetActive(true);
    }
    public void OnConfirmPressed()
    {
        if (!fadedInputPanel)
        {
            GameStaticData.PLAYER_NAME = playerNameInputField.text;
            GameStaticData.DOGGO_NAME = puppyNameInputField.text;
            fadedInputPanel = true;
            StartCoroutine(FadeoutInputPanel());
        }
    }
    public void ShowMenu()
    {
        StartCoroutine(FadeInMenuButtons());
    }

    public void OnStartPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitPressed()
    {
        Debug.Log("Dont quit! Stay determined!");
    }

    IEnumerator FadeoutInputPanel()
    {
        while (inputsCg.alpha > 0)
        {
            float delta = 1f / fadeOutInputDuration * Time.deltaTime;
            inputsCg.alpha = Mathf.Max(0, inputsCg.alpha - delta);
            yield return new WaitForEndOfFrame();
        }
        gameIntroController.OnPlayerFinishedInput();
    }
    IEnumerator FadeInMenuButtons()
    {
        menuButtonsCG.gameObject.SetActive(true);
        menuButtonsCG.alpha = 0;
        while (menuButtonsCG.alpha < 1)
        {
            float delta = 1f / fadeOutInputDuration * Time.deltaTime;
            menuButtonsCG.alpha = Mathf.Min(1, menuButtonsCG.alpha + delta);
            yield return new WaitForEndOfFrame();
        }
    }
}
