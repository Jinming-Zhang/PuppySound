using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement player;

    [SerializeField]
    private Puppy puppy;

    [SerializeField]
    private AudioClip encounterMusic;

    private bool hasEncountered = false;

    //[SerializeField]
    //private MazeLocation location;

    public MazeLocation Location
    {
        get
        {
            MazeLocation l = GameController.Instance.Viewer.worldLocationToMazeLocation(transform.position);
            return l;
        }
        //set => location = value;
    }

    Vector3 targetLocation;
    Rigidbody2D rb;
    [SerializeField]
    float speed = 2;
    [SerializeField]
    SpriteRenderer spriteRenderer;
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
            rb.MovePosition(currentPos + direction * speed * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasEncountered)
        {
            AudioSource.PlayClipAtPoint(this.encounterMusic, this.transform.position);
            hasEncountered = true;
        }
        
    }

    public void OnPlayerCalling(int playerSoundStrength, int puppySoundStrength, MazeLocation playerLocation, MazeLocation puppyLocation)
    {
        //this.location = GameController.Instance.Viewer.worldLocationToMazeLocation(transform.position);
        if (playerSoundStrength > puppySoundStrength)
        {
            // move towards player.
            targetLocation = GameController.Instance.GetNextLocation(this.Location, playerLocation);
        }
        else
        {
            // move towards puppy.
            targetLocation = GameController.Instance.GetNextLocation(this.Location, puppyLocation);
        }
    }
    public void Hide()
    {
        spriteRenderer.enabled = false;
    }
    public void Show()
    {
        spriteRenderer.enabled = true;

    }

}
