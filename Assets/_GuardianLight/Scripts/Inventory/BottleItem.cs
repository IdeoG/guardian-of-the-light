using System;
using UnityEngine.UI;

[Serializable]
public class BottleItem
{
    public int Count;
    public string Description;
    public Image Image;
    public BottleType Type;

    public BottleItem(BottleType type, string description, Image image)
    {
        Type = type;
        Description = description;
        Image = image;
    }
}