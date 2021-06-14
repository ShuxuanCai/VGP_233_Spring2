using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPController : MonoBehaviour
{
    public GameObject cam;
    public GameObject stevePrefeb;
    public Slider healthbar;
    public Text ammoReserves;
    public Text ammoClipAmount;
    public Transform shotDirection;
    public Animator anim;
    public AudioSource shot;
    public GameObject bloodPrefeb;

    public GameObject uiBloodPrefab;
    public GameObject gameOverPrefeb;
    public GameObject winTextPrefeb;
    public GameObject canvas;

    float cWidth;
    float cHeight;

    float speed = 0.1f;
    float Xsensitivity = 2;
    float Ysensitivity = 2;
    float minX = -90;
    float maxX = 90;

    //float x;
    //float z;

    //Inventory
    int ammo = 50;
    int maxAmmo = 50;
    float health = 100.0f;
    float maxHealth = 100.0f;
    int ammoClip = 10;
    int ammoClipMax = 10;

    Rigidbody rb;
    CapsuleCollider capsule;

    Quaternion cameraRot;
    Quaternion characterRot;

    bool cursorIsLocked = true;
    bool lockCursor = true;

    public void TakeHit(float amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        healthbar.value = health;

        GameObject bloodSplatter = Instantiate(uiBloodPrefab);
        bloodSplatter.transform.SetParent(canvas.transform);
        bloodSplatter.transform.position = new Vector3(Random.Range(0, cWidth), Random.Range(0, cHeight), 0);

        Destroy(bloodSplatter, 2.2f);
        
        Debug.Log("Health: " + health);
        if(health <= 0)
        {
            Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            GameObject steve = Instantiate(stevePrefeb, pos, this.transform.rotation);
            steve.GetComponent<Animator>().SetTrigger("Death");
            GameStats.gameOver = true;
            Destroy(this.gameObject);

            GameObject gameOverText = Instantiate(gameOverPrefeb);
            gameOverText.transform.SetParent(canvas.transform);
            gameOverText.transform.localPosition = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Home")
        {
            Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            GameObject steve = Instantiate(stevePrefeb, pos, this.transform.rotation);
            steve.GetComponent<Animator>().SetTrigger("Dance");
            GameStats.gameOver = true;
            Destroy(this.gameObject);
            GameObject winTextText = Instantiate(winTextPrefeb);
            winTextText.transform.SetParent(canvas.transform);
            winTextText.transform.localPosition = Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        capsule = this.GetComponent<CapsuleCollider>();

        cameraRot = cam.transform.localRotation;
        characterRot = this.transform.localRotation;

        health = maxHealth;
        healthbar.value = health;

        ammoReserves.text = ammo + "";
        ammoClipAmount.text = ammoClip + "";

        cWidth = canvas.GetComponent<RectTransform>().rect.width;
        cHeight = canvas.GetComponent<RectTransform>().rect.height;
    }

    void ProcessZombieHit()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(shotDirection.position, shotDirection.forward, out hitInfo, 200))
        {
            GameObject hitZombie = hitInfo.collider.gameObject;
            if (hitZombie.tag == "Zombie")
            {
                GameObject blood = Instantiate(bloodPrefeb, hitInfo.point, Quaternion.identity);
                blood.transform.LookAt(this.transform.position);
                Destroy(blood, 1.5f);

                hitZombie.GetComponent<ZombieController>().shotsTaken++;
                if (hitZombie.GetComponent<ZombieController>().shotsTaken ==
                hitZombie.GetComponent<ZombieController>().shotsRequired)
                {
                    if (Random.Range(0, 10) < 5)
                    {
                        GameObject rdPrefab = hitZombie.GetComponent<ZombieController>().ragdoll;
                        GameObject newRD = Instantiate(rdPrefab, hitZombie.transform.position, hitZombie.transform.rotation);
                        newRD.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(shotDirection.forward * 10000);
                        Destroy(hitZombie);
                    }

                    else
                    {
                        hitZombie.GetComponent<ZombieController>().KillZombie();
                    }
                }
            }
        }
    }

    public ParticleSystem muzzleFlash;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            if (ammoClip > 0)
            {
                anim.SetTrigger("fire");
                shot.Play();
                muzzleFlash.Play();
                ProcessZombieHit();
                ammoClip--;
                ammoClipAmount.text = ammoClip + "";
            }
            Debug.Log("Ammo Left in clip: " + ammoClip);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("reload");
            int amountNeed = ammoClipMax - ammoClip;
            int ammoAvailable = amountNeed < ammo ? amountNeed : ammo;
            ammo -= ammoAvailable;
            ammoClip += ammoAvailable;
            ammoReserves.text = ammo + "";
            ammoClipAmount.text = ammoClip + "";
            Debug.Log("Ammo Left: " + ammo);
            Debug.Log("Ammo in clip: " + ammoClip);
        }
    }

    void FixedUpdate()
    {
        float yRot = Input.GetAxis("Mouse X") * Ysensitivity;
        float xRot = Input.GetAxis("Mouse Y") * Xsensitivity;

        cameraRot *= Quaternion.Euler(-xRot, 0, 0);
        characterRot *= Quaternion.Euler(0, yRot, 0);

        cameraRot = ClampRotationAroundXAxis(cameraRot);

        this.transform.localRotation = characterRot;
        cam.transform.localRotation = cameraRot;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0, 300, 0);
        }

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        transform.position += cam.transform.forward * z + cam.transform.right * x;

        UpdateCursorLock();
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, minX, maxX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    bool IsGrounded()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, capsule.radius, Vector3.down, out hitInfo, (capsule.height / 2f - capsule.radius + 0.1f)))
        {
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Ammo" && ammo < maxAmmo)
        {
            ammo = Mathf.Clamp(ammo + 10, 0, maxAmmo);
            ammoReserves.text = ammo + "";
            Debug.Log("Ammo: " + ammo);
            Destroy(col.gameObject);
        }

        else if (col.gameObject.tag == "MedKit" && health < maxHealth)
        {
            health = Mathf.Clamp(health + 25, 0, maxHealth);
            healthbar.value = health;
            Debug.Log("MedKit: " + health);
            Destroy(col.gameObject);
        }
    }


    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        if (lockCursor)
        {
            InternalLockUpdate();
        }
    }

    public void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cursorIsLocked = false;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            cursorIsLocked = true;
        }

        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        else if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
