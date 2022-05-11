using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class Npc1 : MonoBehaviour
    {
        public GameObject triggerText;
        public GameObject DialogueObject;
        public RigidbodyFirstPersonController rigid;
        public bool hasTalked = false;
        public bool isInDialogue = false;
        public string npcName;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player" && !isInDialogue)
            {


                triggerText.SetActive(true);
                triggerText.GetComponent<TextMeshProUGUI>().text = "Press E to talk to " + npcName;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    triggerText.SetActive(true);
                    if (!hasTalked)
                    {
                        other.gameObject.GetComponent<PlayerData>().DialogueNumber = 1;
                        DialogueObject.SetActive(true);
                        rigid.enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        triggerText.SetActive(false);
                    }
                    else
                    {
                        other.gameObject.GetComponent<PlayerData>().DialogueNumber = 1.5f;
                        DialogueObject.SetActive(true);
                        rigid.enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        triggerText.SetActive(false);
                        
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            triggerText.SetActive(false);
        }
    }
}
