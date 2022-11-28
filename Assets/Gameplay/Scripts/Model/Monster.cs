using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Puppy puppy;

    [SerializeField]
    private MazeLocation location;
    public MazeLocation Location { get => location; set => location = value; }

    Vector3 targetLocation;
    Rigidbody2D rb;
    [SerializeField]
    float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetLocation = gameObject.transform.position;
    }

   
   
    private void FixedUpdate()
    {
        if (Vector3.Distance(targetLocation, transform.position) > .1f)
        {
            Vector3 currentPos = transform.position;
            Vector3 direction = targetLocation - currentPos;
            direction = direction.normalized;
            rb.MovePosition(currentPos+direction*speed*Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collided with {collision.gameObject.name}");
    }

    public void OnPlayerCalling(int playerSoundStrength, int puppySoundStrength, MazeLocation playerLocation, MazeLocation puppyLocation)
    {
        if (playerSoundStrength > puppySoundStrength)
        {
            // move towards player.
            targetLocation = GameController.Instance.getNextLocation(this.location, playerLocation);
        } else
        {
            // move towards puppy.
            targetLocation = GameController.Instance.getNextLocation(this.location, puppyLocation);
        }
    }

}
