using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class ChestScript : MonoBehaviour
    {
        public GameObject Equipmentbar;
        public GameObject Chestbar;
        public GameObject button;
        public GameObject naviPanel;
        
        private SaveSystem saveSystem;
        public items Allitems;

        public int ChestNumber = 0;

        public ObjectData[] data;
        public Image[] chestImages;

        private bool isChestOpen = false;
        private bool isCloseChest = false;

        public TextMeshProUGUI triggerText;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                triggerText.text = "Press E to open the chest";
                triggerText.gameObject.SetActive(true);
                isCloseChest = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                triggerText.gameObject.SetActive(false);
                isCloseChest = false;
            }
        }

        private void Start()
        {
            saveSystem = FindObjectOfType<SaveSystem>();
            
            ChestNumber = Random.Range(0, 999999999);

            if (PlayerPrefs.HasKey((transform.parent.position.x + transform.parent.position.y + transform.parent.position.z).ToString()))
            {
                ChestNumber = PlayerPrefs.GetInt((
                    transform.parent.position.x + transform.parent.position.y + transform.parent.position.z).ToString());
            }
            
        }

        private void Update()
        {
            if (isCloseChest)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!isChestOpen)
                    {
                        naviPanel.SetActive(true);
                        Chestbar.SetActive(true);
                        Equipmentbar.SetActive(false);
                        button.SetActive(false);
                        triggerText.gameObject.SetActive(false);
                        FindObjectOfType<PlayerData>().ChestScript = this;
                        FindObjectOfType<SaveSystem>().ChestScript = this;
                        saveSystem.checkChest(transform.parent);
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (PlayerPrefs.HasKey(ChestNumber + "chestbar" + i.ToString()))
                            {
                                var ObjectsPrefs = PlayerPrefs.GetInt(ChestNumber + "chestbar" + i.ToString());
                                data[i] = Allitems.itemOBJ[ObjectsPrefs].data;
                                chestImages[i].gameObject.GetComponent<Slot>().Objdata = Allitems.itemOBJ[ObjectsPrefs].data;
                            }
                        }
                        FindObjectOfType<PlayerData>().Chestbar = data;
                        FindObjectOfType<PlayerData>().ChestbarSlots = chestImages;
                        FindObjectOfType<PlayerData>().ReloadChestbar();
                        Time.timeScale = 0;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        isChestOpen = true;
                    }
                    else
                    {
                        naviPanel.SetActive(false);
                        Chestbar.SetActive(false);
                        Equipmentbar.SetActive(true);
                        button.SetActive(true);
                        triggerText.gameObject.SetActive(true);
                        saveSystem.saveChest(data);
                        FindObjectOfType<PlayerData>().ChestScript = this;
                        FindObjectOfType<SaveSystem>().ChestScript = this;
                        FindObjectOfType<PlayerData>().Chestbar = data;
                        FindObjectOfType<PlayerData>().ChestbarSlots = chestImages;
                        FindObjectOfType<PlayerData>().ReloadChestbar();
                        Time.timeScale = 1;
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        isChestOpen = false;
                    }
                }
            }
        }

        
    }
}
