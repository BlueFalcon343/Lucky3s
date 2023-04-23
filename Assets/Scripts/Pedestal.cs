using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    public GameObject Catnip;
    public GameObject Yarnball;
    public GameObject Portal;
    bool isCatnip = false;
    bool isYarnball = false;
    public bool isPortal = false;

    // Start is called before the first frame update
    void Start()
    {
        Catnip.SetActive(false);
        Yarnball.SetActive(false);
        Portal.SetActive(false);
        isPortal = false;
    }

    void Update()
    {
        if (isCatnip && isYarnball)
        {
            Portal.SetActive(true);
            isPortal = true;
        }
    }

    public void ShowCatnip()
    {
        Catnip.SetActive(true);
        isCatnip = true;
    }

    // Update is called once per frame
    public void ShowYarnball()
    {
        Yarnball.SetActive(true);
        isYarnball = true;
    }
}
