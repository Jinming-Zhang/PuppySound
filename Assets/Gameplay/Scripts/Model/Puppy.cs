using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A puppy barks when the player calls.
 */

public class Puppy : MonoBehaviour
{

    private int panicLevel = 0;

    [Header("Panic Level")]
    [SerializeField]
    private int maxPanicLevel = 10;
    [SerializeField]
    private int panicThreshHold1 = 5;
    [SerializeField]
    private int panicThreshHold2 = 8;
    [SerializeField]
    private int comfortEffect = 2;

    private float timeBeforePanicIncreace = 30;
    // Start is called before the first frame update.
    void Start()
    {
        
    }

    // Update is called once per frame.
    void Update()
    {
        timeBeforePanicIncreace -= Time.deltaTime;
        if (timeBeforePanicIncreace <= 0)
        {
            if (panicLevel < maxPanicLevel)
            {
                panicLevel++;
            }
            timeBeforePanicIncreace = 30f;
        }
        Debug.Log("PanicLevel" + panicLevel);
    }

    private bool isMonsterNear()
    {
        // TODO: find a way to figure out if monster is near.
        return false;
    }

    // This function is triggered when player calls for puppy.
    void called()
    {
        // comforts the puppy.
        panicLevel -= comfortEffect;

        // if monster nearby and not panic level 2, bark back;
        if (!isMonsterNear() && panicLevel < panicThreshHold2)
        {
            bark();
        }
    }

    // The puppy barks back after player calls.
    void bark()
    {
        // TODO: notify player and monster the level of sound based on distance.
        // move monster(s) accordingly.
    }
}
