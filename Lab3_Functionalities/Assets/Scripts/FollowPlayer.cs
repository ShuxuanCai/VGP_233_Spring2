using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //game object to follow : player
    public GameObject player;
    //the position of the camera
    private Vector3 offset = new Vector3(22.0f, 2.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        //the camera will follow the player
        transform.position = player.transform.position + offset;
    }
}
