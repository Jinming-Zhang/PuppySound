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
    List<string> playerTexts = new List<string>() { "Where are you?", "[dogName]!", "Can you hear me, [dogName]?", "Come here, [dogName]" };

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
        StartCoroutine(PushNotificationsCR());
    }
    IEnumerator PushNotificationsCR()
    {
        nPanel.PushNotification(playerTexts[Random.Range(0, playerTexts.Count)]);
        yield return new WaitForSeconds(1f);
        nPanel.PushNotification(dogTexts[Random.Range(0, dogTexts.Count)]);
        yield return new WaitForSeconds(1f);
        nPanel.PushNotification(monsterText);
    }
}
