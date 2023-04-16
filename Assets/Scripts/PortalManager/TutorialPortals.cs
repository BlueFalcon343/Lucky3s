using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPortals : MonoBehaviour
{
    public GameObject MarsPortal;
    public GameObject TutorialPortal;

    // Start is called before the first frame update
    void Start()
    {
        MarsPortal.SetActive(false);
        TutorialPortal.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<PlayerController>().tutorial)
        {
            MarsPortal.SetActive(true);
            TutorialPortal.SetActive(false);
        }
    }
}
