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
    private Puppy puppy;

    [SerializeField]
    private Monster monster;

    [SerializeField]
    private int soundStrength = 5;
    public int GetSoundStrength { get => soundStrength; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.ReadInput();
    }
    private void FixedUpdate()
    {
        playerMovement.Move();
    }
}
