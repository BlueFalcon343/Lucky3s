using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // movement systems
    private Rigidbody rb;
    public float MOVESPEED = 1f;
    private float movementX;
    private float movementY;
    private Vector2 movementVector;

    // rotation systems
    public float ROTATIONSPEED = 3f;
    public float ROTATIONLERP = 0.5f;
    public float MINTURN = 40;
    public float MAXTURN = 340;
    private float rotationX;
    private float rotationY;
    public Quaternion nextRotation;
    private Vector2 rotationVector;

    // referenced gameobjects
    public GameObject cameraFollow;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        cameraFollow.transform.rotation *= Quaternion.AngleAxis(rotationY * ROTATIONSPEED, Vector3.up);
        cameraFollow.transform.rotation *= Quaternion.AngleAxis(rotationX * ROTATIONSPEED, Vector3.right);

        var angles = cameraFollow.transform.localEulerAngles;
        angles.z = 0;
        var angle = cameraFollow.transform.localEulerAngles.x;

        if (angle > 180 && angle < MAXTURN)
        {
            angles.x = MAXTURN;
        }
        else if(angle < 180 && angle > MINTURN)
        {
            angles.x = MINTURN;
        }

        cameraFollow.transform.localEulerAngles = angles;
        nextRotation = Quaternion.Lerp(cameraFollow.transform.rotation, nextRotation, Time.deltaTime * ROTATIONLERP);
        transform.rotation = Quaternion.Euler(0, cameraFollow.transform.rotation.eulerAngles.y, 0);
        cameraFollow.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * MOVESPEED);
        movement = Vector3.zero;
        LookAt();
    }

    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnLookX(InputValue rotationValue)
    {
        rotationVector = rotationValue.Get<Vector2>();
        rotationX = rotationVector.y;
        rotationY = rotationVector.x;
    }

    void OnLookKey(InputValue rotationValue)
    {
        rotationVector = rotationValue.Get<Vector2>();
        rotationX = (rotationVector.y / rotationVector.sqrMagnitude);
        rotationY = (rotationVector.x  / rotationVector.sqrMagnitude);
    }

    void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;
        
        if ((movementVector.sqrMagnitude > 0.1f) && (direction.sqrMagnitude > 0.1f))
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
}
