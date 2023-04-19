using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform _orientation;
    public Transform _player;
    public Transform _playerBody;
    private Rigidbody rb;
    public Vector3 nextPosition;
    public Quaternion nextRotation;
    public float ROTATESPEED = 2.0f;

    public float rotationPower = 1.5f;
    public float rotationLerp = 0.5f;
    public Vector3 lookDirection;

    private static CameraController cameraInstance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (cameraInstance == null)
        {
            cameraInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        lookDirection = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
        _orientation.forward = lookDirection.normalized;
        Vector3 moveDirection = _orientation.forward * FindObjectOfType<PlayerController>().movementY + _orientation.right * FindObjectOfType<PlayerController>().movementX;

        if(moveDirection != Vector3.zero)
        {
            _playerBody.forward = Vector3.Slerp(_playerBody.forward, moveDirection.normalized, Time.deltaTime * ROTATESPEED);
        }
    }

    public void ToggleCursor()
    {
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
