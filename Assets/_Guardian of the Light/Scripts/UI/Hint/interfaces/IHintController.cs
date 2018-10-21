using _Guardian_of_the_Light.Scripts.Base.Inventory;

namespace _Guardian_of_the_Light.Scripts.UI.Hint.interfaces
{
    public interface IHintController
    {
        void OnShowHintButtonPressed(HintType type, string text, InventoryEntity entity);
    }
}