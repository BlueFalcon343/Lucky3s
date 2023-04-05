using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // movement systems
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



    [Header("UI")]
    public Slider HealthBarUI;
    public Slider JumpBoostUI;
    public Image EnergyCoreUI;
    public Image CatnipUI;
    public int CatnipItem;


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
    [Header("Gun Point, Ammo, Audio")]
    float gunheat;
    const float fireRate = 0.9f;
    public int coreAmmo = 0;
    public AudioSource waterSource;
    public AudioSource freezeSource;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject PlayerProjectile;

    //Particles
    public ParticleSystem WaterGun;
    public ParticleSystem FreezeGun;
    public ParticleSystem BootFlame;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        //set health
        currentHealth = maxHealth;

        //Setting UI
        JumpBoostUI.value = jumpBoost;
        HealthBarUI.value = currentHealth;
        EnergyCoreUI.enabled = false;
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
        }
        while (grounded == true && jumpBoost <= 4)
        {
               jumpBoost = jumpBoost + 1;
               JumpBoostUI.value = jumpBoost;
        }
    }

    void Update()
    {       
        //Ground Check Raycast 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 1.5f, WhatIsGround);

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
            SceneManager.LoadScene("DeathScreen");
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

        if (other.gameObject.CompareTag("Portal"))
        {
            SceneManager.LoadScene("MarsLevel");
        }
    }
    void handleAnimation()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if((movement.x > 0f || movement.y > 0f) && !isWalking)
        {
            Debug.Log("Walking");
            animator.SetBool("isWalking", true);
        }

        else if((movement.x == 0f || movement.y == 0f) && isWalking)
        {
            Debug.Log("NotWalking");
            animator.SetBool("isWalking", false);
        }
    }
}
