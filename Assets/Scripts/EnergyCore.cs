using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCore : MonoBehaviour
{
    public AudioClip energyCore;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(energyCore, transform.position);
            Destroy(gameObject);
        }
    }
}
