using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;
    [SerializeField]
    private Player player;

    [SerializeField]
    private Puppy puppy;

    [SerializeField]
    private Monster monster;
    public Monster Monster { get => monster; }

    [SerializeField]
    private DungeonViewer viewer;

    [SerializeField]
    private DungeonController dungeonController;

    private void Awake()
    {
        if(instance && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Calling()
    {
        MazeLocation playerLocation = viewer.worldLocationToMazeLocation(player.transform.position);
        MazeLocation monsterLocation = viewer.worldLocationToMazeLocation(monster.transform.position);
        MazeLocation puppyLocation = puppy.GetLocation;
        int playerToMonster = Utility.calculateSoundStrength(playerLocation, monsterLocation, player.GetSoundStrength);
        int puppyToMonster = Utility.calculateSoundStrength(puppyLocation, monsterLocation, player.GetSoundStrength);

        bool puppyBark = puppy.called();
        if (puppyBark)
        {
            // monster is notified puppy bark.
        }

        // monster notified player sound.
    }

    public int getDistance(MazeLocation start, MazeLocation mazeLocation)
    {
        var path = new AStarSearch().ComputePath(dungeonController.MazeModel, start, mazeLocation);
        return path.Count - 1;
    }

}
