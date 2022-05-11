using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public int questNumber;
    public float DialogueNumber;

    [Header("PlayerStats")] 
    public float MaxHealth;
    public float curHealth;

    public ObjectData[] Hotbar;
    public Image[] hotabrSlots;
    public Image[] BackgroundSlots;

    public int curEquipped;

    public Color equippedColor;
    public Color normalColor;

    [Header("UIComponents")] 
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    [HideInInspector]
    public bool isDragged;
    public int moveToSlot;
    public ObjectData data;
    public int parentItem;

    public GameObject DraggingSprite;

    public GameObject Canvas;

    private GameObject TempDragObj;

    private UIManager manager;
    
    public void Start()
    {
        healthSlider.maxValue = MaxHealth;
        curHealth = MaxHealth;
        healthSlider.value = curHealth;
        healthText.text = curHealth.ToString("F0") + "/" + MaxHealth.ToString("F0");
        manager = FindObjectOfType<UIManager>();
        EquipHotbar();
    }

    public void EquipHotbar()
    {
        for (int i = 0; i < BackgroundSlots.Length; i++)
        {
            if (i == curEquipped)
            {
                BackgroundSlots[i].color = equippedColor;
            }
            else
            {
                BackgroundSlots[i].color = normalColor;
            }
        }
    }

    private void Update()
    {
        if (!manager.InventorySystem.activeSelf)
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

        if (TempDragObj != null)
        {
            TempDragObj.transform.position = Vector2.Lerp(TempDragObj.transform.position, Input.mousePosition, 0.3f);
        }
    }

    public void TakeDamage(float Damage)
    {
        curHealth -= Damage;
        healthSlider.value = curHealth;
        if (curHealth <= 0)
        {
            Application.LoadLevel("StartingVillage");
        }
        healthText.text = curHealth.ToString("F0") + "/" + MaxHealth.ToString("F0");
    }

    public void ReloadHotbar()
    {
        for (int i = 0; i < Hotbar.Length; i++)
        {
            if (Hotbar[i] != null)
            {
                hotabrSlots[i].sprite = Hotbar[i].sprite;
                hotabrSlots[i].color = new Color(255, 255, 255, 255);
            }
            else
            {
                hotabrSlots[i].sprite = null;
                hotabrSlots[i].color = new Color(0, 0, 0, 0);
            }
            
        }
    }

    public void Heal(float Health)
    {
        curHealth += Health;
        
        if (curHealth > MaxHealth)
            curHealth = MaxHealth;
        
        healthSlider.value = curHealth;
        healthText.text = curHealth.ToString("F0") + "/" + MaxHealth.ToString("F0");
    }

    public void DragItem(int indexNum, ObjectData passingObj)
    {
        if (!isDragged)
        {
            isDragged = true;
            moveToSlot = indexNum;
            Hotbar[indexNum] = null;
            data = passingObj;
            ReloadHotbar();
            hotabrSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = null;

            TempDragObj = Instantiate(DraggingSprite, DraggingSprite.transform.position,
                DraggingSprite.transform.rotation);
            TempDragObj.transform.SetParent(Canvas.transform);
            TempDragObj.GetComponent<Image>().sprite = data.sprite;
        }
        else
        {
            isDragged = false;
            moveToSlot = -1;
            Hotbar[indexNum] = data;
            hotabrSlots[indexNum].gameObject.GetComponent<Slot>().Objdata = data;
            data = null;
            ReloadHotbar();
            Destroy(TempDragObj);
            TempDragObj = null;
        }
    }

   
}
