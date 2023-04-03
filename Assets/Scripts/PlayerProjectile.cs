using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 0f;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        //Make Bullet Move
        transform.position += transform.forward * speed * Time.deltaTime;

        if ((transform.position - startPosition).magnitude > 40.0f)
        {
            Destroy(gameObject);
        }
    }
}
