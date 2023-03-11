using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _orientation;
    public Transform _player;
    public Transform _playerBody;
    public float ROTATESPEED = 1f;
    private Rigidbody rb;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 lookDirection = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
        _orientation.forward = lookDirection.normalized;
        Vector3 moveDirection = _orientation.forward * FindObjectOfType<PlayerController>().movementY + _orientation.right * FindObjectOfType<PlayerController>().movementX;

        if(moveDirection != Vector3.zero)
        {
            _playerBody.forward = Vector3.Slerp(_playerBody.forward, moveDirection.normalized, Time.deltaTime * ROTATESPEED);
        }
    }
}
