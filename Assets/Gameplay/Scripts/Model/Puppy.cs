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

    private float timeBeforePanicIncreace = 10;

    public int SoundStrength { get => soundStrength; }

    [SerializeField]
    private int soundStrength = 8;

    private MazeLocation location;
    public MazeLocation Location { get => location; set => location = value; }
    public void SetLocation(MazeLocation l)
    {
        location = l;
    }

    public PanicBar panicBar;

    // Start is called before the first frame update.
    void Start()
    {
        panicBar.SetMaxHealth(this.maxPanicLevel, panicThreshHold1, panicThreshHold2);
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
                panicBar.SetPanicLevel(this.panicLevel);
            }
            timeBeforePanicIncreace = 20f;
        }
        // Debug.Log("PanicLevel" + panicLevel);
    }

    private bool isMonsterNear(MazeLocation monsterLocation)
    {
        if (GameController.Instance.GetDistance(monsterLocation, this.location) <= 1)
        {
            return true;
        }
        return false;
    }

    // This function is triggered when player calls for puppy.
    public bool called()
    {
        // comforts the puppy.
        panicLevel -= comfortEffect;
        panicBar.SetPanicLevel(this.panicLevel);

        if (!isMonsterNear(GameController.Instance.Monster.Location) && panicLevel < panicThreshHold2)
        {
            return true;
        }
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with {other.gameObject.name}");
    }

}
