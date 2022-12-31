using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/type")]
public class ObjectData : ScriptableObject
{
    public Sprite sprite;
    public GameObject weaponPrefab;
    public GameObject groundItem;
    public float AttackSpeed;
    public float AttackDamage;
    public float ArmourValue = 0;

    public string itemType;

}
