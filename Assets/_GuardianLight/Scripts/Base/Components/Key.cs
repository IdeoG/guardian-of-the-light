using System;
using UnityEngine;

public class Key : BaseAction
{
//    [SerializeField] private KeyType _keyType;

    protected override void OnKeyActionPressedDown()
    {
        CollectKey();
        PlayKeyAnimation();
    }

    private void PlayKeyAnimation()
    {
        /** PlayKeyAnimation
         * TODO: 1. Проиграть анимацию поднятия ключа
         * 2. Удалить объект как только закончится анимация
         */


        Destroy(gameObject, 1);
    }

    private void CollectKey()
    {
//        var inventory = _InventorySystem.Instance;
//
//        switch (_keyType)
//        {
//            case KeyType.Alchemist:
//                inventory.IsContainsAlchemistKey = true;
//                break;
//            case KeyType.Mine:
//                break;
//            default:
//                throw new ArgumentOutOfRangeException();
//        }
    }
}