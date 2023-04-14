using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // movement systems
    [Header("Movement")]
    private Rigidbody rb;
    public float MOVESPEED = 1f;
    public float movementX;
    public float movementY;
    private Vector2 movementVector;
    public Vector2 rotationVector;
    private Vector3 movement;
    public Transform _orientation;
    public GameObject _camera;

    //animator
    Animator animator;



    [Header("UI and Micean Throwable")]
    public Slider HealthBarUI;
    public Slider JumpBoostUI;
    public Image EnergyCoreUI;
    public Image CatnipUI;
    public int CatnipItem;
    public Image MiceanUI;
    public int Micean;
    public GameObject MiceanThrowable;


    // health variables
    [Header("Health")]
    public int maxHealth = 10;
    public int health { get { return currentHealth; } }
    int currentHealth;

    //Jump and Boost Mechanics
    [Header("Jump and Boost")]
    public int jumpBoost = 5;
    public float jumpforce = 12f;
    public Transform jumpPointLeft;
    public Transform jumpPointRight;

    //Jump Recharge and Ground Check
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    bool grounded;

    //Gun firing location and usage
    [Header("Weapons")]
    float gunheat;
    const float fireRate = 0.9f;
    public int coreAmmo = 0;
    public AudioSource waterSource;
    public AudioSource freezeSource;
    public Transform firePoint;
    public GameObject PlayerProjectile;

    //Particles
    [Header("Particles")]
    public ParticleSystem WaterGun;
    public ParticleSystem FreezeGun;
    public ParticleSystem BootFlame;

    //Dialogue
    [Header("Dialogue")]
    bool talk1;
    bool talk2;
    bool talk3;
    bool talk4;
    bool talk5;
    public LayerMask NPC1;
    public LayerMask NPC2;
    public LayerMask NPC3;
    public LayerMask NPC4;
    public LayerMask NPC5;

    //Scenes
    int sceneTracker = 0;
    /* 0 is hub, 1 is tutorial, 2 is mars, 3 is jupiter, 4 is caturn(space), 5 is caturn(cave) */


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();

        //set health
        currentHealth = maxHealth;

        //Setting UI
        JumpBoostUI.value = jumpBoost;
        HealthBarUI.value = currentHealth;
        EnergyCoreUI.enabled = false;
        MiceanUI.enabled = false;
        animator = GetComponent<Animator>();

    }

    
    void FixedUpdate()
    {
        if (!PauseMenu.GameIsPaused)
        {
            movement = (movementY * FindObjectOfType<CameraController>()._orientation.forward) + (movementX * FindObjectOfType<CameraController>()._orientation.right);
            rb.AddForce(movement.normalized * MOVESPEED, ForceMode.Force);
            movement = Vector3.zero;
        }
        //Set Force and Refuel Boost Jumps
        if(grounded == true)
        {
            jumpforce = 12f;
            MOVESPEED = 50f;
        }
        /*
        while (grounded == true)
        {
            MOVESPEED = 50f;
        }
        while (grounded != true)
        {
            MOVESPEED = 20f;
        }
        */
        while (grounded == true && jumpBoost <= 4)
        {
               jumpBoost = jumpBoost + 1;
               JumpBoostUI.value = jumpBoost;
        }

        //ToggleDialogue();
    }

    void Update()
    {       
        //Ground Check Raycast 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, WhatIsGround);

        // Fire Rate
        if (gunheat > 0) gunheat -= Time.deltaTime;

        //Items UI
        if(coreAmmo == 1)
        {
            EnergyCoreUI.enabled = true;
        }
        else
        {
            EnergyCoreUI.enabled = false;
        }

        if (CatnipItem == 1)
        {
            CatnipUI.enabled = true;
        }
        else
        {
            CatnipUI.enabled = false;
        }

        if (Micean >= 1)
        {
            MiceanUI.enabled = true;
        }
        else
        {
            MiceanUI.enabled = false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        if (!PauseMenu.GameIsPaused)
        {
            movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
        }
    }

    void OnLook(InputValue rotationValue)
    {
        // receives mouse/joystick input
        rotationVector = rotationValue.Get<Vector2>();
    }

    // Jump and Jump Boost
    void OnJump()
    {
        if(grounded == true)
        {
            rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
        }

        if(jumpBoost >= 1 && grounded == false)
        {
            jumpforce = 10f;
            jumpBoost = jumpBoost - 1;
            JumpBoostUI.value = jumpBoost;
            rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
            Instantiate(BootFlame, jumpPointLeft.position, jumpPointLeft.rotation);
            Instantiate(BootFlame, jumpPointRight.position, jumpPointRight.rotation);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        HealthBarUI.value = currentHealth;
        Debug.Log(currentHealth + "/" + maxHealth);
        if(currentHealth <= 0)
        {
            SceneManager.LoadScene("DeathScreen");
            FindObjectOfType<CameraController>().ToggleCursor();
        }
    }

    // Ray and Freeze Ray
    public void OnFire()
    {
        if (gunheat <= 0 && !PauseMenu.GameIsPaused)
        {
            PlayerProjectile.tag = "Fire";
            waterSource.Play();
            Instantiate(PlayerProjectile, firePoint.position, firePoint.rotation);
            Instantiate(WaterGun, firePoint.position, firePoint.rotation);
            gunheat = fireRate;
        }
        
        talk1 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC1);
        talk2 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC2);
        talk3 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC3);
        talk4 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC4);
        talk5 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC5);
    }

    public void OnAltFire()
    {
        if (coreAmmo == 1 && !PauseMenu.GameIsPaused)
        {
            PlayerProjectile.tag = "AltFire";
            freezeSource.Play();
            Instantiate(PlayerProjectile, firePoint.position, firePoint.rotation);
            Instantiate(FreezeGun, firePoint.position, firePoint.rotation);
            coreAmmo = coreAmmo - 1;           
        }
    }

    IEnumerator OnThrowable()
    {
        if (Micean == 1 && !PauseMenu.GameIsPaused)
        {
            gameObject.name = "Player1";
            Instantiate(MiceanThrowable, firePoint.position, firePoint.rotation);
            Micean = Micean - 1;
            yield return new WaitForSeconds(10f);
            gameObject.name = "Player";
        }
    }

    public void OnA()
    {
        if (PauseMenu.GameIsPaused)
        {
            FindObjectOfType<PauseMenu>().Resume();
        }
    }
    public void OnB()
    {
        if (PauseMenu.GameIsPaused)
        {
            FindObjectOfType<PauseMenu>().QuitGame();
        }
    }

    // Item Collection and Portals
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnergyCore"))
        {
            coreAmmo = coreAmmo = 1;
        }

        if (other.gameObject.CompareTag("Catnip"))
        {
            CatnipItem = CatnipItem = 1;
        }

        if (other.gameObject.CompareTag("Micean"))
        {
            Micean = Micean + 1;
        }

        if (other.gameObject.CompareTag("PortalToHub"))
        {
            SceneManager.LoadScene("HubLevel");
            sceneTracker = 0;
        }
        if (other.gameObject.CompareTag("PortalToTutorial"))
        {
            SceneManager.LoadScene("TutorialLevel");
            sceneTracker = 1;
        }
        if (other.gameObject.CompareTag("PortalToMars"))
        {
            SceneManager.LoadScene("MarsLevel");
            sceneTracker = 2;
        }
        if (other.gameObject.CompareTag("PortalToJupiter"))
        {
            SceneManager.LoadScene("JupiterLevel");
            sceneTracker = 3;
        }
        if (other.gameObject.CompareTag("PortalToCaturn"))
        {
            SceneManager.LoadScene("CaturnLevel");
            sceneTracker = 4;
        }
        if (other.gameObject.CompareTag("PortalInToCaturn"))
        {
            SceneManager.LoadScene("CaturnCaveLevel");
            sceneTracker = 5;
        }


        if (other.gameObject.CompareTag("DeathZone"))
        {
            SceneManager.LoadScene("DeathScreen");
            FindObjectOfType<CameraController>().ToggleCursor();
        }
    }

    void ToggleDialogue()
    {
        if (talk1)
        {
            FindObjectOfType<NPCCatController>().DisplayDialogue(1);
        }
        else if (talk2)
        {
            FindObjectOfType<NPCCatController>().DisplayDialogue(2);
        }
        else if (talk3)
        {
            FindObjectOfType<NPCCatController>().DisplayDialogue(3);
        }
        else if (talk4)
        {
            FindObjectOfType<NPCCatController>().DisplayDialogue(4);
        }
        else if (talk5)
        {
            FindObjectOfType<NPCCatController>().DisplayDialogue(5);
        }
        else
        {
            FindObjectOfType<NPCCatController>().RemoveDialogue();
        }
    }
}
