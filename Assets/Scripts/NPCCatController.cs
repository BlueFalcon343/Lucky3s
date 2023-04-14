using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCCatController : MonoBehaviour
{
    public RawImage dialogueBox1;
    public RawImage dialogueBox2;
    public RawImage dialogueBox3;
    public RawImage dialogueBox4;
    public RawImage dialogueBox5;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox1.enabled = false;
        dialogueBox2.enabled = false;
        dialogueBox3.enabled = false;
        dialogueBox4.enabled = false;
        dialogueBox5.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDialogue(int dialogueInt)
    {
        switch(dialogueInt)
        {
            case 1:
            {
                dialogueBox1.enabled = true;
                break;
            }
            case 2:
            {
                dialogueBox2.enabled = true;
                break;
            }
            case 3:
            {
                dialogueBox3.enabled = true;
                break;
            }
            case 4:
            {
                dialogueBox4.enabled = true;
                break;
            }
            case 5:
            {
                dialogueBox5.enabled = true;
                break;
            }
        }
    }

    public void RemoveDialogue()
    {
        dialogueBox1.enabled = false;
        dialogueBox2.enabled = false;
        dialogueBox3.enabled = false;
        dialogueBox4.enabled = false;
        dialogueBox5.enabled = false;
    }
}
