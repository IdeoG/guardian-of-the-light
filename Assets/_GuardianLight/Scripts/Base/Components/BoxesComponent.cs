public class BoxesComponent : BaseAction
{
    protected override void OnKeyActionPressedDown()
    {
        /** TODO: OnKeyPressedAction
         * Сделать красивее!
         */
        DestroyBoxes();
    }

    private void DestroyBoxes()
    {
        Destroy(gameObject, 1);
    }
}