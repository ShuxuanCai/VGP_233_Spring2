using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(22.0f, 2.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
