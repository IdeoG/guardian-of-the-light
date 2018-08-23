using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Item
{
    public bool IsTook;
    public string Name;
    public string Description;
    public Sprite Sprite;
    public GameObject Prefab;
}