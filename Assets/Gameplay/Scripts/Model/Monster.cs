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
    public MazeLocation Location { get => location; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collided with {collision.gameObject.name}");
    }


}
