using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class quest01gateOpener : MonoBehaviour
{
    public Animator anim;
    public GameObject triggerText;

    public PlayerData data;

    private void Start()
    {
        data = FindObjectOfType<PlayerData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && data.questNumber == 1)
        {
            triggerText.SetActive(true);
            triggerText.GetComponent<TextMeshProUGUI>().text = "Press E to open gate";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && data.questNumber == 1)
        {
            if (Input.GetKey(KeyCode.E))
            {
                openGate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            triggerText.SetActive(false);
        }
    }

    void openGate()
    {
        anim.enabled = true;
        triggerText.SetActive(false);
        Destroy(gameObject);
    }
}
