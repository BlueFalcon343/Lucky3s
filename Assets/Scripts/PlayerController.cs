using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // movement systems
    private Rigidbody rb;
    public float MOVESPEED = 1f;
    public float movementX;
    public float movementY;
    private Vector2 movementVector;
    private Vector3 movement;

    // health variables
    public int maxHealth = 10;
    public int health { get { return currentHealth; } }
    int currentHealth;

    //Jump and Boost Mechanics
    [Header("Jump and Boost")]
    public int jumpBoost = 5;
    public float jumpforce = 12f;

    //Jump Recharge and Ground Check
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    bool grounded;

    //Gun firing location and usage
    [Header("Gun Location")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject PlayerProjectile;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        //set health
        currentHealth = maxHealth;
    }

    
    void FixedUpdate()
    {
        movement = (movementY * FindObjectOfType<CameraController>()._orientation.forward) + (movementX * FindObjectOfType<CameraController>()._orientation.right);
        rb.AddForce(movement.normalized * MOVESPEED, ForceMode.Force);
        movement = Vector3.zero;

        //Set Force and Refuel Boost Jumps
        if(grounded == true)
        {
            jumpforce = 12f;
        }
        while (grounded == true && jumpBoost <= 4)
        {
               jumpBoost = jumpBoost + 1;
        }
    }

    void Update()
    {
        //Ground Check Raycast 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);
    }

    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
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
            rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    // Ray and Freeze Ray
    public void OnFire()
    {
        PlayerProjectile.tag = "Fire";
        Instantiate(PlayerProjectile, firePoint.position, firePoint.rotation);
    }

    public void OnAltFire()
    {
        PlayerProjectile.tag = "AltFire";
        Instantiate(PlayerProjectile, firePoint.position, firePoint.rotation);
    }
}
