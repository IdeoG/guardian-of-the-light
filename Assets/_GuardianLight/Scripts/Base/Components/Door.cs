public class Door : BaseAction
{
    private void ChangeDoorState()
    {
        var doorState = Animator.GetBool("isOpenDoor");
        Animator.SetBool("isOpenDoor", !doorState);
    }

    protected override void OnKeyActionPressedDown()
    {
        ChangeDoorState();
    }
}