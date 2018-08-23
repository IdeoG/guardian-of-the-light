using System;
using UnityEngine;

public class _InventorySystem : MonoBehaviour
{
    public bool IsContainsAlchemistKey;
    public bool IsContainsMineKey;


    public static _InventorySystem Instance { get; private set; }

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