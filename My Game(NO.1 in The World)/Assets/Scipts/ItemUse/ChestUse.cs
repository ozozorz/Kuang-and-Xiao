using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class ChestUse : MonoBehaviour
    {
        private PlayerData data;
        private SaveSystem saveSystem;
        
        public GameObject PlayerChest;
        
        private Vector3 transformVar;

        private void Start()
        {
            data = FindObjectOfType<PlayerData>();
            saveSystem = FindObjectOfType<SaveSystem>();
        }

        private void setLocation()
        {
            var x = data.dropArea.position.x;
            var z = data.dropArea.position.z;
            transformVar = new Vector3(x, 0f, z);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                setLocation();
                Instantiate(PlayerChest, transformVar, data.dropArea.rotation);
                PlayerChest.GetComponentInChildren<ChestScript>().Start();
                saveSystem.ChestScript = PlayerChest.GetComponentInChildren<ChestScript>();
                saveSystem.checkChest(transformVar);
                data.Hotbar[data.curEquipped] = null;
                data.hotbarSlots[data.curEquipped].sprite = null;
                data.hotbarSlots[data.curEquipped].color = new Color(0, 0, 0, 0);
                Destroy(gameObject);
            }
        }
    }
}
