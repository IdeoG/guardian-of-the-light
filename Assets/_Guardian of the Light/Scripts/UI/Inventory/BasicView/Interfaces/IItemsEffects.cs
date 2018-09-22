using UnityEngine;

public interface IItemsEffects
{
    void SetLightingPosition(Vector3 position);
    void SetName(string text);
    void SetArrowsVisibility(bool isLeftArrowVisible, bool isRightArrowVisible);
}