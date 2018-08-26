public class Key : BaseAction
{
    protected override void OnKeyActionPressedDown()
    {
        CollectKey();
        PlayKeyAnimation();
    }

    private void PlayKeyAnimation()
    {
        // TODO: PlayKeyAnimation -> Add attractive animation
        Destroy(gameObject, 1);
    }

    private void CollectKey()
    {
        // TODO: CollectKey -> Implement key collection functionality
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