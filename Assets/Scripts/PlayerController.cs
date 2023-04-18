using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using System.Diagnostics;

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

    [Header("Player TP, Tag Change")]
    public GameObject Player;
    private Transform player;
    private static PlayerController playerInstance;


    [Header("UI and Micean Throwable")]
    public Slider HealthBarUI;
    public Slider JumpBoostUI;
    public Image EnergyCoreUI;

    public Image CatnipUI;
    public int CatnipItem;

    public Image YarnballUI;
    public int YarnballItem;
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
    public AudioSource rocketSource;

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
    public bool tutorial = false;

    //Dialogue
    public RawImage dialogueBox;
    public TextMeshProUGUI dialogueText;


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
        YarnballUI.enabled = false;
        CatnipUI.enabled = false;
        animator = GetComponent<Animator>();
        dialogueBox.enabled = false;

        /*/Position in New Scene
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //player.transform.position = new Vector3(0, 0, 0);
        if (SceneManager.GetActiveScene().name == "MarsLevel")
        {
            player.transform.position = new Vector3(435, 39, 205);
        }//*/
    }


    void Awake()
    {
        //Data Preservation 
        DontDestroyOnLoad(this.gameObject);

        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //Spawn
        if (SceneManager.GetActiveScene().name == "HubLevel")
        {
            playerInstance.transform.position = new Vector3(1f, 1f, -30f);
        }
        if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            playerInstance.transform.position = new Vector3(256f, 6f, 440f);
        }
        if (SceneManager.GetActiveScene().name == "MarsLevel")
        {
            playerInstance.transform.position = new Vector3(436f, 38f, 204f);
        }
        if (SceneManager.GetActiveScene().name == "JupiterLevel")
        {
            playerInstance.transform.position = new Vector3(342f, 25f, 433f);
        }
        if (SceneManager.GetActiveScene().name == "CaturnLevel")
        {
            playerInstance.transform.position = new Vector3(119F, 50F, 1443F);
        }
        if (SceneManager.GetActiveScene().name == "CaturnCaveLevel")
        {
            playerInstance.transform.position = new Vector3(61f, -22f, -13f);
        }
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
            animator.SetBool("isFalling", false);
        }
        
        while (grounded == true && jumpBoost <= 4)
        {
               jumpBoost = jumpBoost + 1;
               JumpBoostUI.value = jumpBoost;
        }

        if(grounded == false)
        {
            animator.SetBool("isFalling", true);
        }
    }

    void Update()
    {       
        //Ground Check Raycast 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, WhatIsGround);

        //Spawn Set Transform
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();


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

        if (YarnballItem == 1)
        {
            YarnballUI.enabled = true;
        }
        else
        {
            YarnballUI.enabled = false;
        }

        if (grounded == true || jumpBoost >= 5)
        {
            MOVESPEED = 50f;
        }
        else
        {
            MOVESPEED = 30f;
        }

        ToggleDialogue();
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
        //animator.SetBool("isJumping", true);
        if (grounded == true)
        {
            rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
        }

        if(jumpBoost >= 1 && grounded == false)
        {
            rocketSource.Play();
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
        talk1 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC1);
        talk2 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC2);
        talk3 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC3);
        talk4 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC4);
        talk5 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPC5);
        
        if ((gunheat <= 0 && !PauseMenu.GameIsPaused) && !(talk1||talk2||talk3||talk4||talk5))
        {
            PlayerProjectile.tag = "Fire";
            waterSource.Play();
            Instantiate(PlayerProjectile, firePoint.position, firePoint.rotation);
            Instantiate(WaterGun, firePoint.position, firePoint.rotation);
            gunheat = fireRate;
        }
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
        if (Micean >= 1 && !PauseMenu.GameIsPaused)
        {
            gameObject.name = "Player1";
            Player.tag = "Player1";
            Instantiate(MiceanThrowable, firePoint.position, firePoint.rotation);
            Micean = Micean - 1;
            yield return new WaitForSeconds(10f);
            gameObject.name = "Player";
            Player.tag = "Player";

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

        //Items

        if (other.gameObject.CompareTag("EnergyCore"))
        {
            coreAmmo = coreAmmo = 1;
        }

        if (other.gameObject.CompareTag("Micean"))
        {
            Micean = Micean + 1;
        }

        if (other.gameObject.CompareTag("Catnip"))
        {
            CatnipItem = CatnipItem = 1;
        }

        if (other.gameObject.CompareTag("Yarn"))
        {
            YarnballItem = YarnballItem = 1;
        }
        
        //Portals

        if (other.gameObject.CompareTag("PortalToHub"))
        {
            SceneManager.LoadScene("HubLevel");
            sceneTracker = 0;
            tutorial = true;
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
            DisplayDialogue(1);
        }
        else if (talk2)
        {
            DisplayDialogue(2);
        }
        else if (talk3)
        {
            DisplayDialogue(3);
        }
        else if (talk4)
        {
            DisplayDialogue(4);
        }
        else if (talk5)
        {
            DisplayDialogue(5);
        }
        else
        {
            DisplayDialogue(0);
        }
    }

    void DisplayDialogue(int dialogueInt)
    {
        switch(dialogueInt)
        {
            case 1:
            {
                dialogueBox.enabled = true;
                dialogueText.text = "Welcome fellow human being. I have created a closed simulation for you to practice navigating new environments.";
                break;
            }
            case 2:
            {
                dialogueBox.enabled = true;
                dialogueText.text = "Your human boots are equipped with rocket boosters. You can press (…) to activate them as long as you have fuel. Don’t worry, your fuel recharges on solid ground.";
                break;
            }
            case 3:
            {
                dialogueBox.enabled = true;
                dialogueText.text = "You have a water gun that you could use to spray any hostile cats. You can’t use it kill us *cough* *cough* I mean them, but they’ll be stunned for a few seconds. Press (…) to fire it.";
                break;
            }
            case 4:
            {
                dialogueBox.enabled = true;
                dialogueText.text = "There are energy cores scattered across the map that you can collect. If you have one, press (…) to fire a blast of freezing energy that can stop those darn non-human cats in their tracks.";
                break;
            }
            case 5:
            {
                dialogueBox.enabled = true;
                dialogueText.text = "I think you’re ready for the real thing. Head through the portal in front of you. Catkind is counting on you … and humankind too. Good luck!";
                break;
            }
            default:
            {
                dialogueBox.enabled = false;
                dialogueText.text = " ";
                break;
            }
        }
    }
}
