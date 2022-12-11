using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeExit : MonoBehaviour
{
    bool foundExit = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && !foundExit)
        {
            foundExit = true;
            GameController.Instance.OnPlayerFoundExit();
        }
    }
}
