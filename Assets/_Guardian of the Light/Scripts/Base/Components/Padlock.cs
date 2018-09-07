using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public class Padlock : BaseAction
{
    [SerializeField] private GameObject _lockedDoor;

    private void PlayOnDestroyAnimation()
    {
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
        // TODO: OnKeyActionPressedDown -> Implement Padlock action
//        var isContainsKey = _InventorySystem.Instance.IsContainsKey(_keyType);
//        if (isContainsKey)
//            PlayOnDestroyAnimation();
//        else
//            GameManagerSystem.Instance.ShowHint(HintType.PadlockNoKeyFound);
    }
}