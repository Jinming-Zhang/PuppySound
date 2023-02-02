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

    private float timeBeforePanicIncreace = 5;

    public int SoundStrength { get => soundStrength; }

    [SerializeField]
    private int soundStrength = 8;

    //private MazeLocation location;
    public MazeLocation Location
    {
        get
        {
            MazeLocation l = GameController.Instance.Viewer.worldLocationToMazeLocation(transform.position);
            return l;
        }
    }

    public PanicBar panicBar;
    [SerializeField]
    SpriteRenderer sprite;


    [SerializeField]
    float followingDistance = .2f;
    float followingSpeed = 1f;
    PlayerMovement reed;
    public bool controllable = true;

    // Start is called before the first frame update.
    void Start()
    {
        panicBar.SetMaxHealth(this.maxPanicLevel, panicThreshHold1, panicThreshHold2);
    }

    // Update is called once per frame.
    void Update()
    {
        if (!controllable)
        {
            return;
        }
        if (!reed)
        {
            if (panicLevel >= maxPanicLevel)
            {
                GameController.Instance.OnPuppyDeath();
                return;
            }
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
        }
        // Debug.Log("PanicLevel" + panicLevel);
        if (reed)
        {
            if (Vector3.Distance(transform.position, reed.transform.position) > followingDistance)
            {
                Vector3 dir = (reed.transform.position - transform.position).normalized;
                transform.position = transform.position + dir * followingSpeed * Time.deltaTime;
                transform.right = dir.x > 0 ? Vector3.left : Vector3.right;
            }
        }
    }

    private bool isMonsterNear(MazeLocation monsterLocation)
    {
        if (GameController.Instance.GetDistance(monsterLocation, this.Location) <= 1)
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
    public void Hide()
    {
        sprite.enabled = false;
    }
    public void Show()
    {
        sprite.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (reed)
        {
            return;
        }

        PlayerMovement p = collision.gameObject.GetComponent<PlayerMovement>();
        if (p)
        {
            reed = p;
            followingSpeed = p.Speed;
            GameController.Instance.OnPlayerFoundDog();
            panicLevel = 0;
            panicBar.SetPanicLevel(panicLevel);
            Debug.Log("Reed found me!");
        }
    }
}
