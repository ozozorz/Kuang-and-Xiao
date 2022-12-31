using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
   public class UIManager : MonoBehaviour
   {
      public GameObject DialgoueObj;
      public GameObject HotbarObj;
      public GameObject InventorySystem;
      public GameObject DeathScreenObj;
      public GameObject EscapeScreenObj;
      public GameObject StatsPanel;
      public GameObject naviPanel;
      
      private PlayerData data;
      public GameObject player;
      private RigidbodyFirstPersonController rigid;

      private void Start()
      {
         rigid = FindObjectOfType<RigidbodyFirstPersonController>();
         data = FindObjectOfType<PlayerData>();
      }

      private void Update()
      {
         if (DialgoueObj.activeSelf)
         {
            HotbarObj.SetActive(false);
         }
         else
         {
            HotbarObj.SetActive(true);
         }

         if (Input.GetKeyDown(KeyCode.Tab) && !EscapeScreenObj.activeSelf && !DeathScreenObj.activeSelf)
         {
            naviPanel.SetActive(!naviPanel.activeSelf);
            data.UpdateStats();
            if (naviPanel.activeSelf)
            {
               Time.timeScale = 0;
               Cursor.visible = true;
               Cursor.lockState = CursorLockMode.None;
            }
            else
            {
               Time.timeScale = 1;
               Cursor.visible = false;
               Cursor.lockState = CursorLockMode.Locked;
               if (data.isDragged)
               {
                  data.DragItem(data.moveToSlot, data.data, data.isInInventory, data.isInEquipment, data.isInChest);
               }
            }
            data.EquipHotbar();
         }

         if (Input.GetKeyDown(KeyCode.Escape))
         {
            EscapeScreen();
         }
      }

      public void ClickOnStatsButton()
      {
         StatsPanel.SetActive(true);
         InventorySystem.SetActive(false);
      }public void ClickOnInventoryButton()
      {
         StatsPanel.SetActive(false);
         InventorySystem.SetActive(true);
      }

      public void EscapeScreen()
      {
         if (!DeathScreenObj.activeSelf)
         {

            EscapeScreenObj.SetActive(!EscapeScreenObj.activeSelf);

            if (EscapeScreenObj.activeSelf)
            {
               Time.timeScale = 0;
               Cursor.visible = true;
               Cursor.lockState = CursorLockMode.None;
            }
            else
            {
               Time.timeScale = 1;
               Cursor.visible = false;
               Cursor.lockState = CursorLockMode.Locked;
            }
         }
      }

      public void DeathScreen()
      {
         DeathScreenObj.SetActive(true);
         rigid.enabled = false;
         Time.timeScale = 0;
         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;
      }
   }

      
}