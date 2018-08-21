public class DoorComponent : BaseActionBehaviour
{
    private void ChangeDoorState()
    {
        var doorState = Animator.GetBool("isOpenDoor");
        Animator.SetBool("isOpenDoor", !doorState);
    }

    protected override void OnKeyPressedAction()
    {
        ChangeDoorState();
    }
}