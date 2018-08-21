public class BoxesComponent : BaseActionBehaviour
{
    protected override void OnKeyPressedAction()
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