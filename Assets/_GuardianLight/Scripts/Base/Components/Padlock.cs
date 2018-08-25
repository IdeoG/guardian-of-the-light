using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public class Padlock : BaseAction
{
//    [SerializeField] private KeyType _keyType;
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

    protected override void OnKeyActionPressedDown()
    {
//        var isContainsKey = _InventorySystem.Instance.IsContainsKey(_keyType);
//        if (isContainsKey)
//            PlayOnDestroyAnimation();
//        else
//            GameManagerSystem.Instance.ShowHint(HintType.PadlockNoKeyFound);
    }
}