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
        private PlayerData data;
        public ObjectData woodenSword;
        private bool hasGottenSword = false;
        private QuestObj obj;

        private void Start()
        {
            data = FindObjectOfType<PlayerData>();
            obj = FindObjectOfType<QuestObj>();
        }

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
                        other.gameObject.GetComponent<PlayerData>().questNumber = 1;
                    }
                    else
                    {
                        for (int i = 0; i < data.Hotbar.Length; i++)
                        {
                            if (data.Hotbar[i] == woodenSword)
                            {
                                hasGottenSword = true;
                            }
                        }

                        if (!hasGottenSword)
                        {
                            other.gameObject.GetComponent<PlayerData>().DialogueNumber = 1.5f;
                            DialogueObject.SetActive(true);
                            rigid.enabled = false;
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                            triggerText.SetActive(false);
                        }
                        else
                        {
                            other.gameObject.GetComponent<PlayerData>().questNumber = -1;
                            other.gameObject.GetComponent<PlayerData>().DialogueNumber = 1.6f;
                            obj.target = null;
                            DialogueObject.SetActive(true);
                            rigid.enabled = false;
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                            triggerText.SetActive(false);
                            
                        }
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
