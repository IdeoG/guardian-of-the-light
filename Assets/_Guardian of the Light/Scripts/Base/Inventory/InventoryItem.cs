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

namespace _Guardian_of_the_Light.Scripts.Inventory
{
    public class InventoryEntity : MonoBehaviour
    {
        public int id;
    }
}