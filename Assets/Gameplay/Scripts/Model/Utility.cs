using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{

    // calculate the sound strength from one player to another based on the sound strength.
    public static int calculateSoundStrength(IMazeModel Maze, MazeLocation start, MazeLocation end, int strength)
    {
        AStarSearch aStar = new AStarSearch();
        var path = aStar.ComputePath(Maze, start, end);
        int soundStrength = strength - path.Count;
        if (soundStrength <= 0)
        {
            soundStrength = 1;
        }
        return soundStrength;
    }
}
