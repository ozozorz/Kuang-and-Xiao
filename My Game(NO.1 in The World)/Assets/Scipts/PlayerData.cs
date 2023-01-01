using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class PlayerData : MonoBehaviour
    {
        public int questNumber;
        public float DialogueNumber;

        [Header("PlayerStats")] public float MaxHealth;
        public float curHealth;
        public float MaxMana;
        public float curMana;
        public float AttackPowerStats;
        public float MagicPowerStats;
        public float ArmourStats;
        public float SpeedStats;
        public float LuckStats;

        public ObjectData[] Hotbar;
        public ObjectData[] Equipmentbar;
        public ObjectData[] Inventory;
        public ObjectData[] Chestbar;
        public Image[] inventorySlots;
        public Image[] hotbarSlots;
        public Image[] EquipmentbarSlots;
        public Image[] BackgroundSlots;
        public Image[] ChestbarSlots;
        public ChestScript ChestScript;

        public int curEquipped;

        public Color equippedColor;
        public Color normalColor;

        private List<Vector3> chestPositionList;

        [Header("StatsText")]
        public TextMeshProUGUI AttackPowerText;
        public TextMeshProUGUI MagicPowerText;
        public TextMeshProUGUI ArmourText;
        public TextMeshProUGUI SpeedText;
        public TextMeshProUGUI LuckText;

        [Header("UIComponents")] public Slider healthSlider;
        public TextMeshProUGUI healthText;
        public Slider manaSlider;
        public TextMeshProUGUI manaText;

        [HideInInspector] public bool isDragged;
        public int moveToSlot;
        public ObjectData data;
        public int parentItem;
        public bool isInInventory;
        public bool isInEquipment;
        public bool isInChest;

        public GameObject DraggingSprite;

        public GameObject Canvas;

        private GameObject TempDragObj;

        public Slider eatSlider;

        private UIManager manager;

        public GameObject cameraObj;

        public Transform dropArea;

        public TextMeshProUGUI triggertext;

        private GameObject CurrentEquipped;

        public GameObject PlayerChest;

        private float manaTimer = 0;

        private SaveSystem saveSystem;
        public items Allitems;

        public void Start()
        {
            manager = FindObjectOfType<UIManager>();
            saveSystem = FindObjectOfType<SaveSystem>();
            
            curHealth = MaxHealth;
            curMana = MaxMana;
            
            loadData();
            
            healthSlider.maxValue = MaxHealth;
            manaSlider.maxValue = MaxMana;
            healthSlider.value = curHealth;
            manaSlider.value = curMana;
            healthText.text = curHealth.ToString("F0") + "/" + MaxHealth.ToString("F0");
            manaText.text = curMana.ToString("F0") + "/" + MaxMana.ToString("F0");
            ReloadHotbar();
            EquipHotbar();
            ReloadInventory();
            ReloadEquipmentbar();
        }

        public void OnApplicationQuit()
        {
            saveSystem.SaveVariable("position", variableType.position, null, transform);
            saveSystem.SaveVariable("health", variableType.floats, curHealth.ToString(), null);
            saveSystem.SaveVariable("maxHealth", variableType.floats, MaxHealth.ToString(), null);
            saveSystem.SaveVariable("mana", variableType.floats, curMana.ToString(), null);
            saveSystem.SaveVariable("maxMana", variableType.floats, MaxMana.ToString(), null);
            
            saveSystem.saveItems(Hotbar, Equipmentbar, Inventory);
        }

        public void loadData()
        {
            if (PlayerPrefs.HasKey("positionx"))
            {
                saveSystem.LoadTransform("position");
                curHealth = saveSystem.LoadVariableFloat("health");
                MaxHealth = saveSystem.LoadVariableFloat("maxHealth");
                curMana = saveSystem.LoadVariableFloat("mana");
                MaxMana = saveSystem.LoadVariableFloat("maxMana");
            }

            for (int i = 0; i < Hotbar.Length; i++)
            {
                if (PlayerPrefs.HasKey("hotbar" + i.ToString()))
                {
                    var ObjectsPrefs = PlayerPrefs.GetInt("hotbar" + i.ToString());
                    Hotbar[i] = Allitems.itemOBJ[ObjectsPrefs].data;
                    hotbarSlots[i].gameObject.GetComponent<Slot>().Objdata = Allitems.itemOBJ[ObjectsPrefs].data;
                }
            }

            for (int i = 0; i < Equipmentbar.Length; i++)
            {
                if (PlayerPrefs.HasKey("equipmentbar" + i.ToString()))
                {
                    var ObjectsPrefs = PlayerPrefs.GetInt("equipmentbar" + i.ToString());
                    Equipmentbar[i] = Allitems.itemOBJ[ObjectsPrefs].data;
                    EquipmentbarSlots[i].gameObject.GetComponent<Slot>().Objdata = Allitems.itemOBJ[ObjectsPrefs].data;
                }
            }
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (PlayerPrefs.HasKey("inventory" + i.ToString()))
                {
                    var ObjectsPrefs = PlayerPrefs.GetInt("inventory" + i.ToString());
                    Inventory[i] = Allitems.itemOBJ[ObjectsPrefs].data;
                    inventorySlots[i].gameObject.GetComponent<Slot>().Objdata = Allitems.itemOBJ[ObjectsPrefs].data;
                }
            }

            chestPositionList = saveSystem.loadChest();
            if (chestPositionList != null)
            {
                foreach (var positionVar in chestPositionList)
                {
                    Instantiate(PlayerChest, positionVar, transform.rotation);
                }
            }
        }

        public void UpdateStats()
        {
            AttackPowerText.text = "AttackPower: " + AttackPowerStats.ToString("F0");
            MagicPowerText.text = "MagicPower: " + MagicPowerStats.ToString("F0");
            ArmourText.text = "Armour: " + ArmourStats.ToString("F0");
            SpeedText.text = "Speed: " + SpeedStats.ToString("F0");
            LuckText.text = "Luck: " + LuckStats.ToString("F0");
        }


        public void EquipHotbar()
        {
            for (int i = 0; i < BackgroundSlots.Length; i++)
            {
                if (i == curEquipped)
                {
                    BackgroundSlots[i].color = equippedColor;

                    if (Hotbar[i] != null)
                    {
                        Destroy(CurrentEquipped);
                        CurrentEquipped = Instantiate(Hotbar[i].weaponPrefab, transform.position, transform.rotation);
                        CurrentEquipped.transform.SetParent(cameraObj.transform);
                        CurrentEquipped.transform.localPosition = Hotbar[i].weaponPrefab.transform.position;
                        CurrentEquipped.transform.localRotation = Hotbar[i].weaponPrefab.transform.rotation;
                    }
                    else
                    {
                        Destroy(CurrentEquipped);
                    }
                }
                else
                {
                    BackgroundSlots[i].color = normalColor;
                }
            }
        }

        private void Update()
        {
            if (manaTimer < 8)
            {
                manaTimer += Time.deltaTime;
            }
            else
            {
                curMana -= Random.Range(1, 2);
                manaSlider.value = curMana;
                manaText.text = curMana.ToString("F0") + "/" + MaxMana.ToString("F0");
                manaTimer = 0;
            }

            if (!manager.naviPanel.activeSelf)
            {
                if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
                {
                    if (curEquipped - 1 < 0)
                    {
                        curEquipped = Hotbar.Length - 1;
                    }
                    else
                    {
                        curEquipped--;
                    }

                    EquipHotbar();
                }
                else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
                {
                    if (curEquipped + 1 > Hotbar.Length - 1)
                    {
                        curEquipped = 0;
                    }
                    else
                    {
                        curEquipped++;
                    }

                    EquipHotbar();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (Hotbar[curEquipped] != null)
                {
                    Instantiate(Hotbar[curEquipped].groundItem, dropArea.position, dropArea.rotation);
                    Hotbar[curEquipped] = null;
                    ReloadHotbar();
                    EquipHotbar();
                }
            }

            if (TempDragObj != null)
            {
                TempDragObj.transform.position =
                    Vector2.Lerp(TempDragObj.transform.position, Input.mousePosition, 0.3f);
            }
        }

        public void lateUpdateHotbar(float time)
        {
            Invoke("ReloadHotbar", time);
        }

        public void TakeDamage(float Damage)
        {
            var newDamage = Damage;
            newDamage /= (1 + (ArmourStats / 500));
            
            curHealth -= newDamage;
            healthSlider.value = curHealth;
            if (curHealth <= 0)
            {
                manager.DeathScreen();
            }

            healthText.text = curHealth.ToString("F0") + "/" + MaxHealth.ToString("F0");
        }

        public void StartAgain()
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SceneManager.LoadScene("StartingVillage");
        }

        public void ReloadHotbar()
        {
            for (int i = 0; i < Hotbar.Length; i++)
            {
                if (Hotbar[i] != null)
                {
                    hotbarSlots[i].sprite = Hotbar[i].sprite;
                    hotbarSlots[i].color = new Color(255, 255, 255, 255);
                }
                else
                {
                    hotbarSlots[i].sprite = null;
                    hotbarSlots[i].color = new Color(0, 0, 0, 0);
                }

            }
        }        
        public void ReloadEquipmentbar()
        {
            for (int i = 0; i < Equipmentbar.Length; i++)
            {
                if (Equipmentbar[i] != null)
                {
                    EquipmentbarSlots[i].sprite = Equipmentbar[i].sprite;
                    EquipmentbarSlots[i].color = new Color(255, 255, 255, 255);
                }
                else
                {
                    EquipmentbarSlots[i].sprite = null;
                    EquipmentbarSlots[i].color = new Color(0, 0, 0, 0);
                }

            }
        }

        public void ReloadInventory()
        {
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] != null)
                {
                    inventorySlots[i].sprite = Inventory[i].sprite;
                    inventorySlots[i].color = new Color(255, 255, 255, 255);
                }
                else
                {
                    inventorySlots[i].sprite = null;
                    inventorySlots[i].color = new Color(0, 0, 0, 0);
                }

            }
        }
        public void ReloadChestbar()
        {
            for (int i = 0; i < Chestbar.Length; i++)
            {
                if (Chestbar[i] != null)
                {
                    ChestbarSlots[i].sprite = Chestbar[i].sprite;
                    ChestbarSlots[i].color = new Color(255, 255, 255, 255);
                }
                else
                {
                    ChestbarSlots[i].sprite = null;
                    ChestbarSlots[i].color = new Color(0, 0, 0, 0);
                }

            }
        }

        public void updateToChest()
        {
            ChestScript.data = Chestbar;
            ChestScript.chestImages = ChestbarSlots;
        }

        public void Heal(float Health)
        {
            curHealth += Health;

            if (curHealth > MaxHealth)
                curHealth = MaxHealth;

            healthSlider.value = curHealth;
            healthText.text = curHealth.ToString("F0") + "/" + MaxHealth.ToString("F0");
        }

        public void DragItem(int indexNum, ObjectData passingObj, bool isInventory, bool isInEquipment, bool isInChest)
        {
            if (!isDragged)
            {
                if (passingObj != null)
                {
                    if (!isInventory && !isInEquipment && !isInChest)
                    {
                        isDragged = true;
                        moveToSlot = indexNum;
                        Hotbar[indexNum] = null;
                        data = passingObj;
                        ReloadHotbar();
                        hotbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = null;

                        TempDragObj = Instantiate(DraggingSprite, DraggingSprite.transform.position,
                            DraggingSprite.transform.rotation);
                        TempDragObj.transform.SetParent(Canvas.transform);
                        TempDragObj.GetComponent<Image>().sprite = data.sprite;
                        isInventory = false;
                    }
                    else if(!isInventory && isInEquipment && !isInChest)
                    {
                        isDragged = true;
                        moveToSlot = indexNum;
                        Equipmentbar[indexNum] = null;
                        data = passingObj;
                        ReloadEquipmentbar();
                        ArmourStats -= data.ArmourValue;
                        UpdateStats();
                        EquipmentbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = null;

                        TempDragObj = Instantiate(DraggingSprite, DraggingSprite.transform.position,
                            DraggingSprite.transform.rotation);
                        TempDragObj.transform.SetParent(Canvas.transform);
                        TempDragObj.GetComponent<Image>().sprite = data.sprite;
                        isInEquipment = true;
                    }
                    else if (!isInventory && !isInEquipment && isInChest)
                    {
                        isDragged = true;
                        moveToSlot = indexNum;
                        Chestbar[indexNum] = null;
                        data = passingObj;
                        ReloadChestbar();
                        ArmourStats -= data.ArmourValue;
                        UpdateStats();
                        ChestbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = null;

                        TempDragObj = Instantiate(DraggingSprite, DraggingSprite.transform.position,
                            DraggingSprite.transform.rotation);
                        TempDragObj.transform.SetParent(Canvas.transform);
                        TempDragObj.GetComponent<Image>().sprite = data.sprite;
                        isInChest = true;
                    }
                    else
                    {
                        isDragged = true;
                        moveToSlot = indexNum;
                        Inventory[indexNum] = null;
                        data = passingObj;
                        ReloadInventory();
                        inventorySlots[indexNum].gameObject.GetComponent<Slot>().Objdata = null;

                        TempDragObj = Instantiate(DraggingSprite, DraggingSprite.transform.position,
                            DraggingSprite.transform.rotation);
                        TempDragObj.transform.SetParent(Canvas.transform);
                        TempDragObj.GetComponent<Image>().sprite = data.sprite;
                        isInventory = true;
                    }
                }
            }
            else
            {
                if (!isInventory && !isInEquipment && !isInChest)
                {
                    if (hotbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata == null)
                    {
                        isDragged = false;
                        moveToSlot = -1;
                        Hotbar[indexNum] = data;
                        hotbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = data;
                        data = null;
                        ReloadHotbar();
                        Destroy(TempDragObj);
                        TempDragObj = null;
                    }
                }
                else if (!isInventory && isInEquipment && !isInChest)
                {
                    if (data.itemType == "Chestplate" && indexNum == 0)
                    {
                        isDragged = false;
                        moveToSlot = -1;
                        Equipmentbar[indexNum] = data;
                        EquipmentbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = data;
                        ArmourStats += data.ArmourValue;
                        data = null;
                        ReloadEquipmentbar();
                        UpdateStats();
                        Destroy(TempDragObj);
                        TempDragObj = null;
                    }
                    else if (data.itemType == "Helmet" && indexNum == 1)
                    {
                        isDragged = false;
                        moveToSlot = -1;
                        Equipmentbar[indexNum] = data;
                        EquipmentbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = data;
                        ArmourStats += data.ArmourValue;
                        data = null;
                        ReloadEquipmentbar();
                        UpdateStats();
                        Destroy(TempDragObj);
                        TempDragObj = null;
                    }
                    else if (data.itemType == "Legs" && indexNum == 2)
                    {
                        isDragged = false;
                        moveToSlot = -1;
                        Equipmentbar[indexNum] = data;
                        EquipmentbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = data;
                        ArmourStats += data.ArmourValue;
                        data = null;
                        ReloadEquipmentbar();
                        UpdateStats();
                        Destroy(TempDragObj);
                        TempDragObj = null;
                    }
                    else if (data.itemType == "Boots" && indexNum == 4)
                    {
                        isDragged = false;
                        moveToSlot = -1;
                        Equipmentbar[indexNum] = data;
                        EquipmentbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = data;
                        ArmourStats += data.ArmourValue;
                        data = null;
                        ReloadEquipmentbar();
                        UpdateStats();
                        Destroy(TempDragObj);
                        TempDragObj = null;
                    }
                }
                else if (!isInventory && !isInEquipment && isInChest)
                {
                    isDragged = false;
                    moveToSlot = -1;
                    Chestbar[indexNum] = data;
                    ChestbarSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = data;
                    updateToChest();
                    data = null;
                    ReloadChestbar();
                    Destroy(TempDragObj);
                    TempDragObj = null;
                }
                else
                {
                    if (inventorySlots[indexNum].gameObject.GetComponent<Slot>().Objdata == null)
                    {
                        isDragged = false;
                        moveToSlot = -1;
                        Inventory[indexNum] = data;
                        inventorySlots[indexNum].gameObject.GetComponent<Slot>().Objdata = data;
                        data = null;
                        ReloadInventory();
                        Destroy(TempDragObj);
                        TempDragObj = null;
                    }
                }
            }
        }


    }
}