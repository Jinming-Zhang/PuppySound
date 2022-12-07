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

    private int panicThreshold1 = 5;
    private int panicThreshold2 = 2;

    private int currentPanicLevel = 0;

    public PanicBar panicBar;

    private float stressTimer = 5f;

    void Start()
    {
        this.panicBar.SetMaxHealth(this.maxPanicLevel, 4, 7);
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = 0f;
        int currentDistanceToMonster = GameController.Instance.PlayerToMonster();

        if (currentDistanceToMonster <= this.panicThreshold1 && currentDistanceToMonster > this.panicThreshold2)
        {
            if (stressTimer > 0)
            {
                stressTimer -= Time.deltaTime;
            } else if (stressTimer <= 0)
            {
                this.currentPanicLevel += 1;
                stressTimer = 5f;
            }
            currentSpeed = playerSpeed - 0.5f;
        } else if (currentDistanceToMonster <= panicThreshold2)
        {
            if (stressTimer > 0)
            {
                stressTimer -= Time.deltaTime * 2;
            }
            else if (stressTimer <= 0)
            {
                this.currentPanicLevel += 1;
                stressTimer = 5f;
            }
            currentSpeed = playerSpeed - 1f;
        } else if (currentDistanceToMonster > this.panicThreshold1)
        {
            if (stressTimer >= 5f)
            {
                if (currentPanicLevel > 0)
                {
                    currentPanicLevel -= 1;
                }
                stressTimer = 0f;
            } else
            {
                stressTimer += Time.deltaTime;
            }
            currentSpeed = playerSpeed;
        }

        this.panicBar.SetPanicLevel(this.currentPanicLevel);
        playerMovement.ReadInput(currentSpeed);
    }
    private void FixedUpdate()
    {
        playerMovement.Move();
    }
}
