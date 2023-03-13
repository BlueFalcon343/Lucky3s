using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    public void OnCollisionEnter(Collision other)
    {
        PlayerController player = other.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-2);
        }

        Destroy(gameObject);
    }
}
