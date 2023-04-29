using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserBoost : MonoBehaviour
{
    public float boostHeight;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * boostHeight);
            FindObjectOfType<PlayerController>().count = -5;
        }
    }
}
