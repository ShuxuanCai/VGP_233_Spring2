using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    // Zombies need the target
    // They are walk or run to the player
    public GameObject target;

    // Some attributes for the zombies, like walking speed, running speed...
    // When I do the project, I think there will be other attributes to add.
    public float walkingSpeed;
    public float runningSpeed;
    public float damage;
    public float health;

    // Zombies can have different states, and use enum to include different states
    enum States
    {
        Idle, // Just stay at the position where they are
        Chase, // Chase for player
        Attack, // Attack the player
        Dead // Died after being attacked by the player
    };

    // Make sure initializing the first state is idle
    States state = States.Idle;

    float DistanceBetweenZombieAndPlayer()
    {
        float distance = 0.0f;
        // Here is calculating the distance between zombie and player
        return distance;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            // I don't konw all the actual numbers in this function, just for example
            case States.Idle:
                if (DistanceBetweenZombieAndPlayer() <= 30.0f) 
                    state = States.Chase;
                break;
            case States.Chase:
                if (DistanceBetweenZombieAndPlayer() >= 20.0f && DistanceBetweenZombieAndPlayer() <= 30.0f)
                    walkingSpeed = 5.0f; 
                else if (DistanceBetweenZombieAndPlayer() < 20.0f && DistanceBetweenZombieAndPlayer() >= 5.0f)
                    walkingSpeed = runningSpeed;
                else
                    state = States.Attack;
                    break;
            case States.Attack:
                if (DistanceBetweenZombieAndPlayer() < 5.0f)
                    damage = 5.0f; // Just attack the player
                else if (DistanceBetweenZombieAndPlayer() <= 30.0f)
                    state = States.Chase;
                else
                    state = States.Idle;
                break;
            case States.Dead:
                Destroy(this); // Destory the zombie
                break;
            default:
                break;
        }
    }
}
