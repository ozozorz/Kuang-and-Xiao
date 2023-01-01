using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public enum variableType
{
    integer,
    strings,
    floats,
    inventory,
    position
}

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class SaveSystem : MonoBehaviour
    {
        private Transform transformVar;
        public items AllItems;
        public ChestScript ChestScript;

        StringBuilder ChestList = new StringBuilder(" ");
        private string[] ChestListString;
        private List<Vector3> ChestPositionList = new List<Vector3>();

        private void Start()
        {
            transformVar = FindObjectOfType<PlayerData>().transform;
        }

        public void SaveVariable(string name, variableType type, string value, Transform transform)
        {
            switch (type)
            {
                case variableType.integer:
                    var tempIntValue = int.Parse(value);

                    PlayerPrefs.SetInt(name, tempIntValue);
                    break;
                case variableType.strings:
                    PlayerPrefs.SetString(name, value);
                    break;
                case variableType.floats:
                    var tempFloatValue = float.Parse(value);

                    PlayerPrefs.SetFloat(name, tempFloatValue);
                    break;
                case variableType.inventory:
                    break;
                case variableType.position:
                    var x = transform.position.x;
                    var y = transform.position.y;
                    var z = transform.position.z;
                    var rx = transform.rotation.x;
                    var ry = transform.rotation.y;
                    var rz = transform.rotation.z;

                    PlayerPrefs.SetFloat(name + "x", x);
                    PlayerPrefs.SetFloat(name + "y", y);
                    PlayerPrefs.SetFloat(name + "z", z);
                    PlayerPrefs.SetFloat(name + "rx", rx);
                    PlayerPrefs.SetFloat(name + "ry", ry);
                    PlayerPrefs.SetFloat(name + "rz", rz);

                    break;
            }
        }

        public string LoadVariableString(string name)
        {
            var stringPrefs = PlayerPrefs.GetString(name);
            return stringPrefs;
        }

        public float LoadVariableFloat(string name)
        {
            var floatPrefs = PlayerPrefs.GetFloat(name);
            return floatPrefs;
        }

        public int LoadVariableInt(string name)
        {
            var intPrefs = PlayerPrefs.GetInt(name);
            return intPrefs;
        }

        public Transform LoadTransform(string name)
        {
            var x = PlayerPrefs.GetFloat(name + "x");
            var y = PlayerPrefs.GetFloat(name + "y");
            var z = PlayerPrefs.GetFloat(name + "z");
            var rx = PlayerPrefs.GetFloat(name + "rx");
            var ry = PlayerPrefs.GetFloat(name + "ry");
            var rz = PlayerPrefs.GetFloat(name + "rz");

            transformVar.position = new Vector3(x, y, z);
            transformVar.rotation = new Quaternion(rx, ry, rz, 1);

            return transformVar;
        }
        public void saveItems(ObjectData[] hotbar, ObjectData[] equipmentbar, ObjectData[] inventory)
        {
            for (int i = 0; i < hotbar.Length; i++)
            {
                if (hotbar[i] != null)
                {
                    for (int x = 0; x < AllItems.itemOBJ.Length; x++)
                    {
                        if (AllItems.itemOBJ[x].data == hotbar[i])
                        {
                            PlayerPrefs.SetInt("hotbar" + i.ToString(), x);
                        }
                    }
                }
                else
                {
                    PlayerPrefs.DeleteKey("hotbar" + i.ToString());
                }
            }
            for (int i = 0; i < equipmentbar.Length; i++)
            {
                if (equipmentbar[i] != null)
                {
                    for (int x = 0; x < AllItems.itemOBJ.Length; x++)
                    {
                        if (AllItems.itemOBJ[x].data == equipmentbar[i])
                        {
                            PlayerPrefs.SetInt("equipmentbar" + i.ToString(), x);
                        }
                    }
                }
                else
                {
                    PlayerPrefs.DeleteKey("equipmentbar" + i.ToString());
                }
            }
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null)
                {
                    for (int x = 0; x < AllItems.itemOBJ.Length; x++)
                    {
                        if (AllItems.itemOBJ[x].data == inventory[i])
                        {
                            PlayerPrefs.SetInt("inventory" + i.ToString(), x);
                        }
                    }
                }
                else
                {
                    PlayerPrefs.DeleteKey("inventory" + i.ToString());
                }
            }
        }

        public void saveChest(ObjectData[] chestbar)
        {
            for (int i = 0; i < chestbar.Length; i++)
            {
                if (chestbar[i] != null)
                {
                    for (int x = 0; x < AllItems.itemOBJ.Length; x++)
                    {
                        if (AllItems.itemOBJ[x].data == chestbar[i])
                        {
                            PlayerPrefs.SetInt(ChestScript.ChestNumber + "chestbar" + i.ToString(), x);
                        }
                    }
                }
                else
                {
                    PlayerPrefs.DeleteKey(ChestScript.ChestNumber + "chestbar" + i.ToString());
                }
            }
        }

        public List<Vector3> loadChest()
        {
            if (PlayerPrefs.HasKey("savedChest"))
            {
                ChestListString = PlayerPrefs.GetString("savedChest").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var positionVar in ChestListString)
                {
                    if (PlayerPrefs.HasKey(positionVar + "x"))
                    {
                        ChestPositionList.Add(new Vector3(PlayerPrefs.GetFloat(positionVar + "x"), 
                            PlayerPrefs.GetFloat(positionVar + "y"),PlayerPrefs.GetFloat(positionVar + "z")));
                    }
                }
            }
            return ChestPositionList;
        }

        public void checkChest(Vector3 position)
        {
            var Chestx = position.x;
            var Chesty = position.y;
            var Chestz = position.z;
            
            PlayerPrefs.SetInt((Chestx + Chesty + Chestz).ToString(), ChestScript.ChestNumber);
            PlayerPrefs.SetFloat(ChestScript.ChestNumber + "x", Chestx);
            PlayerPrefs.SetFloat(ChestScript.ChestNumber + "y", Chesty);
            PlayerPrefs.SetFloat(ChestScript.ChestNumber + "z", Chestz);
            if (PlayerPrefs.HasKey("savedChest"))
            {
                StringBuilder ChestList = new StringBuilder(PlayerPrefs.GetString("savedChest"));
            }
            ChestList.Append(ChestScript.ChestNumber + " ");
            PlayerPrefs.SetString("savedChest", ChestList.ToString());
        }
    }
}