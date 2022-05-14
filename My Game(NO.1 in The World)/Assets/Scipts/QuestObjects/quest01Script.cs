using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{

   public class quest01Script : MonoBehaviour
   {
      public GameObject groundItem;
      public Transform dropPosition;
      public GameObject triggerText;
      public QuestObj obj;
      public GameObject Tafei;

      public Transform[] spawnPoints;

      public GameObject skeletonObj;

      private int spawnAmount;

      private void OnTriggerEnter(Collider other)
      {
         if (other.tag == "Player")
         {
            triggerText.SetActive(true);
            triggerText.GetComponent<TextMeshProUGUI>().text = "Press E to open chest";
         }
      }

      private void OnTriggerStay(Collider other)
      {
         if (other.tag == "Player")
         {
            if (Input.GetKey(KeyCode.E))
            {
               SpawnSword();
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

      void SpawnSword()
      {
         Instantiate(groundItem, dropPosition.position, dropPosition.rotation);
         triggerText.SetActive(false);
         obj.target = Tafei.transform;

         spawnAmount = Random.Range(2, 4);
         for (int i = 0; i < spawnAmount; i++)
         {
            var ranNum = Random.Range(0, spawnPoints.Length);

            Instantiate(skeletonObj, spawnPoints[ranNum].position, spawnPoints[ranNum].rotation);
         }
         
         
         Destroy(gameObject);
      }
   }
}