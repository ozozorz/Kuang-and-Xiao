using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Objects")]
public class items : ScriptableObject
{
    public itemOBJ[] itemOBJ;
}
[System.Serializable]
public class itemOBJ
{
    public string name;
    public string tooltip;
    public ObjectData data;
}
