using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Portals : MonoBehaviour
{
    public GameObject Portal;
    // Start is called before the first frame update
    void Start()
    {
        Portal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PlayerController>().YarnballItem == 1)
        {
            Portal.SetActive(true);
        }
    }
}
