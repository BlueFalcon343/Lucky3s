using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 8f;

    void Start()
    {

    }

    void Update()
    {
        //Make Bullet Move
        transform.position += transform.forward * speed * Time.deltaTime;

        if (transform.position.magnitude > 200.0f)
        {
            Destroy(gameObject);
        }
    }
}
