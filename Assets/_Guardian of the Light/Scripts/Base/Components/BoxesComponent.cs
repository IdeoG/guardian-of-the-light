public class BoxesComponent : BaseAction
{
    protected override void OnKeyActionPressedDown()
    {
        // TODO: Add attractive animation =)
        DestroyBoxes();
    }

    private void DestroyBoxes()
    {
        Destroy(gameObject, 1);
    }
}