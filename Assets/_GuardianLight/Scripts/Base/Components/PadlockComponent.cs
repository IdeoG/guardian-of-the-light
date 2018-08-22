﻿using System;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public class PadlockComponent : BaseActionBehaviour
{
    [SerializeField] private KeyType _keyType;
    [SerializeField] private GameObject _lockedDoor;
    
    private void PlayOnDestroyAnimation()
    {
        GameManagerSystem.Instance.HideHint();
        
        /** PlayOnDestroyAnimation
         * 1. Анимация поднятия зацепки в замке
         * 2. Добавляем rigidbody замку
         * 3. От падения на землю считаем 3 секунды и плавно в это время меняем альфу у материала
         * 4. Разрушаем объект
         */

        Animator.enabled = true;

        _lockedDoor.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        
        Destroy(gameObject, 3f);
    }

    private void AddRigidBody()
    {
        gameObject.AddComponent<Rigidbody>();
    }

    private void Awake()
    {
        _lockedDoor.GetComponent<BoxCollider>().enabled = false;
        Animator = GetComponent<Animator>();
        Animator.enabled = false;
    }

    protected override void OnKeyPressedAction()
    {
        var isContainsKey = _InventorySystem.Instance.IsContainsKey(_keyType);
        if (isContainsKey)
        {
            PlayOnDestroyAnimation();
        }
        else
        {
            /** TODO: OnKeyPressedAction
             * 1. Блокируем все кнопки, кроме кнопки действия
             * 2. Анимация плавного показа подсказки
             * 3. Ждем пока нажмем кнопку действия
             * 4. По нажатию на кнопку действия проигрываем анимацию плавного скрытия подсказки
             * 5. Разблокировка всех кнопок
             */
            GameManagerSystem.Instance.ShowHint(HintType.PadlockNoKeyFound);
        }
    }
}

