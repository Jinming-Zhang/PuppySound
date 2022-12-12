using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player class.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private float playerSpeed = 2f;

    [SerializeField]
    private Puppy puppy;

    [SerializeField]
    private Monster monster;

    [SerializeField]
    private int soundStrength = 5;
    public int GetSoundStrength { get => soundStrength; }

    [SerializeField]
    private int maxPanicLevel = 16;

    private int panicDistanceThreshold1 = 10;
    private int panicDistanceThreshold2 = 3;
    private float speedDecrement1 = 0.2f;
    private float speedDecrement2 = 0.4f;

    private int currentPanicLevel = 0;

    public PanicBar panicBar;

    private float stressTimer = 5f;

    public bool controllable = true;

    void Start()
    {
        this.panicBar.SetMaxHealth(this.maxPanicLevel, 4, 7);
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = 0f;
        int currentDistanceToMonster = GameController.Instance.PlayerToMonster();

        if (currentDistanceToMonster <= this.panicDistanceThreshold1 && currentDistanceToMonster > this.panicDistanceThreshold2)
        {
            if (stressTimer > 0)
            {
                stressTimer -= Time.deltaTime;
            }
            else if (stressTimer <= 0)
            {
                this.currentPanicLevel += 1;
                stressTimer = 5f;
            }
            currentSpeed = playerSpeed - speedDecrement1;
        }
        else if (currentDistanceToMonster <= panicDistanceThreshold2)
        {
            if (stressTimer > 0)
            {
                stressTimer -= Time.deltaTime * 1.5f;
            }
            else if (stressTimer <= 0)
            {
                this.currentPanicLevel += 1;
                stressTimer = 5f;
            }
            currentSpeed = playerSpeed - speedDecrement2;
        }
        else if (currentDistanceToMonster > this.panicDistanceThreshold1)
        {
            if (stressTimer >= 5f)
            {
                if (currentPanicLevel > 0)
                {
                    currentPanicLevel -= 1;
                }
                stressTimer = 0f;
            }
            else
            {
                stressTimer += Time.deltaTime;
            }
            currentSpeed = playerSpeed;
        }

        this.panicBar.SetPanicLevel(this.currentPanicLevel);
        if (currentPanicLevel >= maxPanicLevel)
        {
            GameController.Instance.OnPlayerDead();
        }
        playerMovement.ReadInput(currentSpeed);
    }
    private void FixedUpdate()
    {
        if (controllable)
        {
            playerMovement.Move();
        }
    }
}
