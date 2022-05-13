using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GroundItem : MonoBehaviour
{
   public TextMeshProUGUI triggerText;
   public new string Name;
   public ObjectData pickupItem;

   private PlayerData data;

   private bool isPickedUp;

   public GameObject Parent;
   
   private void Start()
   {
      data = FindObjectOfType<PlayerData>();
      triggerText = data.triggertext;
   }

   private void OnTriggerStay(Collider other)
   {
      if (other.tag == "Player")
      {

         if (!isPickedUp)
         {
            triggerText.gameObject.SetActive(true);
         }

         triggerText.text = "Press E to pick up " + Name;
         if (Input.GetKeyDown(KeyCode.E))
         {
            for (int i = 0; i < data.Hotbar.Length; i++)
            {
               if (data.Hotbar[i] == null)
               {
                  data.Hotbar[i] = pickupItem;
                  data.hotabrSlots[i].gameObject.GetComponent<Slot>().Objdata = pickupItem;
                  triggerText.gameObject.SetActive(false);
                  i = data.Hotbar.Length + 1;
                  isPickedUp = true;
                  data.EquipHotbar();
                  Destroy(Parent);
               }
            }

            data.ReloadHotbar();
         }
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.tag == "Player")
      {
         triggerText.gameObject.SetActive(false);
      }
   }
}
