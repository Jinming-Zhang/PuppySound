using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallButton : MonoBehaviour
{
    [SerializeField]
    DialoguePanel dialogue;
    [SerializeField]
    float coolDown = 5f;
    [SerializeField]
    CountDownUIOverlay overlay;

    [SerializeField]
    NotificationPanel nPanel;
    [SerializeField]
    List<string> playerTexts = new List<string>() { "Where are you?", "puppy_name!", "Can you hear me, puppy_name?", "Come here, puppy_name" };

    [SerializeField]
    List<string> dogTexts = new List<string>() { "Woof woof!", "Bow-wow!", "Ruff arff!", "Bark!" };

    private float cdCounter = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && cdCounter <= 0)
        {
            onCall();
            cdCounter = coolDown;
        }
        cdCounter -= Time.deltaTime;
    }

    public void onCall()
    {
        //dialogue.gameObject.SetActive(true);
        //dialogue.callingEvent();
        StopAllCoroutines();
        overlay.Action(coolDown);
        string saySomething = playerTexts[Random.Range(0, playerTexts.Count)].Replace("puppy_name", GameStaticData.DOGGO_NAME);
        GameController.Instance.Calling(saySomething);
    }
    IEnumerator PushNotificationsCR()
    {
        //nPanel.PushNotification(new NotificationInfo());
        string saySomething = playerTexts[Random.Range(0, playerTexts.Count)].Replace("puppy_name", GameStaticData.DOGGO_NAME);
        GameController.Instance.PlayerSays(saySomething);
        yield return new WaitForSeconds(2f);
        //nPanel.PushNotification(new NotificationInfo(dogTexts[Random.Range(0, dogTexts.Count)]));
        GameController.Instance.ShowDoggoEcho();
        yield return new WaitForSeconds(3f);
        GameController.Instance.ShowMonsterEcho();
    }
}
