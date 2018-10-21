using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public int Id;
    public string Name;
    [Multiline] public string Description;
    public GameObject Prefab2D;
    public GameObject Prefab3D;
    public bool IsTook;
}
