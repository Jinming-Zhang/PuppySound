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

    public int SoundStrength { get => soundStrength; }

    [SerializeField]
    private int soundStrength = 3;

    private MazeLocation location;
    public MazeLocation GetLocation { get => location; }
    public void SetLocation (MazeLocation l) 
    { 
        location = l; 
    }

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

    private bool isMonsterNear(MazeLocation monsterLocation)
    {
        // TODO: find a way to figure out if monster is near.
        if (GameController.Instance.getDistance(monsterLocation, this.location) <= 1) {
            return true;
        }
        return false;
    }

    // This function is triggered when player calls for puppy.
    public bool called()
    {
        // comforts the puppy.
        panicLevel -= comfortEffect;

        // TODO
        // if monster nearby and not panic level 2, bark back;
        if (!isMonsterNear(GameController.Instance.Monster.Location) && panicLevel < panicThreshHold2)
        {
            return true;
        }
        return false;
    }

}
