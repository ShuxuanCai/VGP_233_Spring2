using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // The Rigidbody of player
    private Rigidbody playerRb;
    private bool isOnGround = true;

    // Add some audios on the player
    public AudioSource shotSound;
    public AudioSource stepSound;

    // Add some attributes for the player
    public float speed;
    public float currentHealth;
    private float maxHealth;
    public float jumpForce;
    public float gravityModifier;
    public int currentAmmo;
    private int maxAmmo;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
        shotSound = GetComponent<AudioSource>();
        stepSound = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            stepSound.Play();
        }

        if (collision.gameObject.CompareTag("AmmoBox"))
        {
            // Ammo will increase
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("HealthBox"))
        {
            // Health will increase
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("Zombie"))
        {
            if (currentHealth > 0)
            {
                // If hit from zombies, the health will decrease
            }
            else
            {
                // The player will die
                // gameOver in the GameStats will equal true
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // The player can rotate and move using mouse and W,A,S,D keys
        // And when press SPACE key, the gun will be shoot
    }
}
