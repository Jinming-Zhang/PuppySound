using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallButton : MonoBehaviour
{
    [SerializeField]
    DialoguePanel dialogue;
    [SerializeField]
    float cd = 2f;
    [SerializeField]
    CountDownUIOverlay overlay;

    [SerializeField]
    NotificationPanel nPanel;
    [SerializeField]
    List<string> playerTexts = new List<string>() { "Where are you?", "puppy_name!", "Can you hear me, puppy_name?", "Come here, puppy_name" };

    [SerializeField]
    List<string> dogTexts = new List<string>() { "Woof woof!", "Bow-wow!", "Ruff arff!", "Bark!" };

    [SerializeField]
    string monsterText = "(Deep roaring from afar.)";

    public void onCall()
    {
        //dialogue.gameObject.SetActive(true);
        //dialogue.callingEvent();
        StopAllCoroutines();
        overlay.Action(cd);
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
        //nPanel.PushNotification(new NotificationInfo(monsterText));
        GameController.Instance.ShowMonsterEcho();
    }
}
