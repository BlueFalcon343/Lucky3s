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


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        //set health
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement = (movementY * FindObjectOfType<CameraController>()._orientation.forward) + (movementX * FindObjectOfType<CameraController>()._orientation.right);
        rb.AddForce(movement.normalized * MOVESPEED, ForceMode.Force);
        movement = Vector3.zero;
    }

    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
