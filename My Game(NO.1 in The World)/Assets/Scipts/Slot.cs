using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class Slot : MonoBehaviour, IPointerClickHandler
    {
        public int slotNumber;

        public ObjectData Objdata;

        private PlayerData data;

        public bool isInventory = true;

        public bool isInEquipment = false;
        public bool isInChest = false;

        private void Awake()
        {
            data = FindObjectOfType<PlayerData>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            data.DragItem(slotNumber, Objdata, isInventory, isInEquipment, isInChest);
        }

    }
}
