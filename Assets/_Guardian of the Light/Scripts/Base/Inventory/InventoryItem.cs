using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public string Name;
    public string Description;
    public GameObject Prefab2D;
    public GameObject Prefab3D;
    public bool IsTook;
}