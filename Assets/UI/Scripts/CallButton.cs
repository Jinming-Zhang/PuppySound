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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void onCall()
    {
        dialogue.gameObject.SetActive(true);
        dialogue.callingEvent();
        overlay.Action(cd);
    }
}
