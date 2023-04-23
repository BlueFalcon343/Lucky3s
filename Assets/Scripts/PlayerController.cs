using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;
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
    public float count = -1;

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
    public TextMeshProUGUI scoreMice;
    public int Micean;
    int scoreM;
    public GameObject MiceanThrowable;

    public Image CatCoinUI;
    public TextMeshProUGUI scoreCoin;
    int score = 0;



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
    bool talk6;
    bool pedestal1;
    bool pedestal2;
    public LayerMask NPC1;
    public LayerMask NPC2;
    public LayerMask NPC3;
    public LayerMask NPC4;
    public LayerMask NPC5;
    public LayerMask CatPedestal;
    public LayerMask YarnPedestal;
    public LayerMask NPCGod;

    //Scenes
    int sceneTracker = 0;
    /* 0 is hub, 1 is tutorial, 2 is mars, 3 is jupiter, 4 is caturn(space), 5 is caturn(cave) */
    public bool tutorial = false;
    // cutscene progression
    int a = 1, b = 1, c = 1, d = 1, e = 1;

    //Dialogue
    public RawImage dialogueBox;
    public TextMeshProUGUI dialogueText;
    public bool dialogue;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();

        //set health
        currentHealth = maxHealth;

        //Setting UI
        YarnballUI.enabled = false;
        CatnipUI.enabled = false;
        MiceanUI.enabled = false;
        CatCoinUI.enabled = true;
        dialogueBox.enabled = false;
        EnergyCoreUI.enabled = false;
        JumpBoostUI.value = jumpBoost;
        HealthBarUI.value = currentHealth;

        scoreCoin.text = "X" + score.ToString();
        scoreMice.text = scoreM.ToString();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

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
        if(grounded == true || jumpBoost == 5)
        {
            jumpforce = 12f;
            animator.SetBool("isFalling", false);
        }
        else if(grounded != true && jumpBoost == 5)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", true);
        }
        
        while (grounded == true && jumpBoost <= 4)
        {
            jumpBoost = jumpBoost + 1;
            JumpBoostUI.value = jumpBoost;
        }
        /*
        if(grounded == false)
        {
            animator.SetBool("isFalling", true);
        }
        */
    }

    void Update()
    {       
        //Ground Check Raycast 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, WhatIsGround);

        //Spawn Set Transform
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if(Input.GetKey("j"))
        {
            SceneManager.LoadScene("MarsLevel");
        }
        if (Input.GetKey("k"))
        {
            SceneManager.LoadScene("JupiterLevel");
        }
        if (Input.GetKey("l"))
        {
            SceneManager.LoadScene("CaturnLevel");
        }
        if (Input.GetKey("i"))
        {
            SceneManager.LoadScene("CaturnCaveLevel");
        }

        if(PauseMenu.GameIsPaused)
        {
            Cursor.visible = true;
        }

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
            scoreMice.enabled = true;
        }
        else
        {
            MiceanUI.enabled = false;
            scoreMice.enabled = false;
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
            MOVESPEED = 35f;
        }
        
        ToggleDialogue();

        // fast fall
        if(!grounded)
        {
            count += Time.deltaTime;
            jumpforce = 10f;
            if (count >= 1)
            {
                rb.AddForce(Vector3.down * count * 1.25f, ForceMode.Force);
                jumpforce = 10f * count / 2f;
            }
        }
        else
        {
            count = -2;
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
        //animator.SetBool("isJumping", true);
        if (grounded == true)
        {
            rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
        }

        if(jumpBoost >= 1 && grounded == false)
        {
            rocketSource.Play();
            // jumpforce = 10f;
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
            Cursor.visible = true;
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
        talk6 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, NPCGod);
        pedestal1 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, CatPedestal);
        pedestal2 = Physics.Raycast(transform.position, FindObjectOfType<CameraController>()._orientation.forward, 10f, YarnPedestal);
        
        if ((gunheat <= 0 && !PauseMenu.GameIsPaused) && !(talk1||talk2||talk3||talk4||talk5||pedestal1||pedestal2) && !(dialogue))
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
            scoreM = scoreM - 1;
            scoreMice.text = scoreM.ToString();
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

        /*if ((FindObjectOfType<HubCutScene>().HubScene)||(FindObjectOfType<JupiterCutScene>().JupiterScene)||(FindObjectOfType<MarsCutScene>().MarsScene)||(FindObjectOfType<CaturnCutScene>().CaturnScene)||(FindObjectOfType<WinCutScene>().WinScene))
        {
            ToggleCutScene();
        }*/
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
            scoreM = scoreM + 1;
            scoreMice.text = scoreM.ToString();
        }

        if (other.gameObject.CompareTag("Catnip"))
        {
            CatnipItem = CatnipItem = 1;
        }

        if (other.gameObject.CompareTag("Yarn"))
        {
            YarnballItem = YarnballItem = 1;
        }

        if (other.gameObject.CompareTag("CatCoin"))
        {
            score = score + 1;
            scoreCoin.text = "X" + score.ToString();
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
        if (other.gameObject.CompareTag("ExitPortal"))
        {
            SceneManager.LoadScene("WinScreen");
        }

        if (other.gameObject.CompareTag("DeathZone"))
        {
            Cursor.visible = true;
            SceneManager.LoadScene("DeathScreen");
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
        else if (talk6)
        {
            DisplayDialogue(6);
        }
        else if (pedestal1)
        {
            FindObjectOfType<Pedestal>().ShowCatnip();
            CatnipItem = 0;
        }
        else if (pedestal2)
        {
            FindObjectOfType<Pedestal>().ShowYarnball();
            YarnballItem = 0;
        }
        else
        {
            DisplayDialogue(0);
        }
    }

    void DisplayDialogue(int dialogueInt)
    {
        dialogue = true;
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
                dialogueText.text = "Your human boots are equipped with rocket boosters that recharge on the ground. You can press (SPACE/A) to activate them as long as you have fuel.";
                break;
            }
            case 3:
            {
                dialogueBox.enabled = true;
                dialogueText.text = "You have a water gun that you could use to spray any hostile cats. They’ll be stunned for a few seconds. Press (LMB/B) to fire it.";
                break;
            }
            case 4:
            {
                dialogueBox.enabled = true;
                dialogueText.text = "These energy cores are scattered across the map. If you have one, press (RMB/Y) to fire a freezing blast to stop those darn non-human cats in their tracks.";
                break;
            }
            case 5:
            {
                dialogueBox.enabled = true;
                dialogueText.text = "You can find little Miceans on your journey. If you have one, press (E/X) to throw them and distract the cats. Good Luck!";
                break;
            }
            case 6:
            {
                if(!(FindObjectOfType<Pedestal>().isPortal))
                {
                    dialogueBox.enabled = true;
                    dialogueText.text = "Welcome human. Present your gifts on the pedestals.";
                    break;
                }
                else
                {
                    dialogueBox.enabled = true;
                    dialogueText.text = "I thank you for all that you've done. Farewell.";
                    break;
                }
            }
            default:
            {
                dialogueBox.enabled = false;
                dialogueText.text = " ";
                dialogue = false;
                break;
            }
        }
    }
/*
    void ToggleCutScene()
    {
        if (SceneManager.GetActiveScene().name == "CaturnLevel")
        {
           switch(a)
           {
                case 1:
                {
                    a = 2;
                    FindObjectOfType<CaturnCutScene>().Next();
                    break;
                }
                case 2:
                {
                    a = 3;
                    FindObjectOfType<CaturnCutScene>().Next1();
                    break;
                }
                case 3:
                {
                    a = 0;
                    FindObjectOfType<CaturnCutScene>().Next2();
                    break;
                }
                default:
                {
                    break;
                }
           }
        }
        else if (SceneManager.GetActiveScene().name == "HubLevel")
        {
           switch(b)
           {
                case 1:
                {
                    b = 2;
                    FindObjectOfType<HubCutScene>().Next();
                    break;
                }
                case 2:
                {
                    b = 3;
                    FindObjectOfType<HubCutScene>().Next1();
                    break;
                }
                case 3:
                {
                    b = 4;
                    FindObjectOfType<HubCutScene>().Next2();
                    break;
                }
                case 4:
                {
                    b = 0;
                    FindObjectOfType<HubCutScene>().Next3();
                    break;
                }
                default:
                {
                    break;
                }
           }
        }
        else if (SceneManager.GetActiveScene().name == "JupiterLevel")
        {
           switch(c)
           {
                case 1:
                {
                    c = 2;
                    FindObjectOfType<JupiterCutScene>().Next();
                    break;
                }
                case 2:
                {
                    c = 3;
                    FindObjectOfType<JupiterCutScene>().Next1();
                    break;
                }
                case 3:
                {
                    c = 0;
                    FindObjectOfType<JupiterCutScene>().Next2();
                    break;
                }
                default:
                {
                    break;
                }
           }
        }
        else if (SceneManager.GetActiveScene().name == "MarsLevel")
        {
           switch(d)
           {
                case 1:
                {
                    d = 2;
                    FindObjectOfType<MarsCutScene>().Next();
                    break;
                }
                case 2:
                {
                    d = 3;
                    FindObjectOfType<MarsCutScene>().Next1();
                    break;
                }
                case 3:
                {
                    d = 0;
                    FindObjectOfType<MarsCutScene>().Next2();
                    break;
                }
                default:
                {
                    break;
                }
           }
        }
        else if (SceneManager.GetActiveScene().name == "WinScreen")
        {
           switch(e)
           {
                case 1:
                {
                    e = 2;
                    FindObjectOfType<WinCutScene>().Next();
                    break;
                }
                case 2:
                {
                    e = 3;
                    FindObjectOfType<WinCutScene>().Next1();
                    break;
                }
                case 3:
                {
                    e = 4;
                    FindObjectOfType<WinCutScene>().Next2();
                    break;
                }
                case 4:
                {
                    e = 0;
                    FindObjectOfType<WinCutScene>().Next3();
                    break;
                }
                default:
                {
                    break;
                }
           }
        }
    }*/
}
