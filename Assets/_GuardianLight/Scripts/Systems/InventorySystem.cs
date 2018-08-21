using System;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public bool IsContainsKey(KeyType keyType)
    {
        bool result;
        switch (keyType)
        {
            case KeyType.Alchemist:
                result = IsContainsAlchemistKey;
                break;
            case KeyType.Mine:
                result = IsContainsMineKey;
                break;
            default:
                throw new ArgumentOutOfRangeException("keyType", keyType, null);
        }

        return result;
    }

    public bool IsContainsAlchemistKey;
    public bool IsContainsMineKey;


    public static InventorySystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}


public enum KeyType
{
    Alchemist = 1,
    Mine = 2
}