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
        private GameObject Equipmentbar;
        private GameObject Chestbar;
        private GameObject button;
        private GameObject naviPanel;
        
        private SaveSystem saveSystem;
        private UIManager uIManager;
        private PlayerData playerData;
        public items Allitems;

        public int ChestNumber = 0;

        public ObjectData[] data;
        public Image[] chestImages;

        private bool isChestOpen = false;
        private bool isClose = false;

        public TextMeshProUGUI triggerText;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                triggerText.text = "Press E to open the chest";
                triggerText.gameObject.SetActive(true);
                isClose = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                triggerText.gameObject.SetActive(false);
                isClose = false;
            }
        }

        public void Start()
        {
            saveSystem = FindObjectOfType<SaveSystem>();
            uIManager = FindObjectOfType<UIManager>();
            playerData = FindObjectOfType<PlayerData>();
            Equipmentbar = uIManager.Equipmentbar;
            Chestbar = uIManager.Chestbar;
            button = uIManager.uiButton;
            naviPanel = uIManager.naviPanel;
            triggerText = playerData.triggertext;
            chestImages = uIManager.chestImages;

            ChestNumber = Random.Range(0, 999999999);

            if (PlayerPrefs.HasKey((transform.parent.position.x + transform.parent.position.y + transform.parent.position.z).ToString()))
            {
                ChestNumber = PlayerPrefs.GetInt((
                    transform.parent.position.x + transform.parent.position.y + transform.parent.position.z).ToString());
            }
            
        }

        private void Update()
        {
            if (isClose)
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
                        playerData.ChestScript = this;
                        saveSystem.ChestScript = this;
                        saveSystem.checkChest(transform.parent.position);
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (PlayerPrefs.HasKey(ChestNumber + "chestbar" + i.ToString()))
                            {
                                var ObjectsPrefs = PlayerPrefs.GetInt(ChestNumber + "chestbar" + i.ToString());
                                data[i] = Allitems.itemOBJ[ObjectsPrefs].data;
                                chestImages[i].gameObject.GetComponent<Slot>().Objdata = Allitems.itemOBJ[ObjectsPrefs].data;
                            }
                        }
                        playerData.Chestbar = data;
                        playerData.ChestbarSlots = chestImages;
                        playerData.ReloadChestbar();
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
                        playerData.ChestScript = this;
                        saveSystem.ChestScript = this;
                        playerData.Chestbar = data;
                        playerData.ChestbarSlots = chestImages;
                        playerData.ReloadChestbar();
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
