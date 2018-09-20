using UnityEngine;

public interface IBasicViewItemsEffects
{
    void SetLightingPosition(Vector3 position);
    void SetName(string text);
    void SetArrowsVisibility(bool isLeftArrowVisible, bool isRightArrowVisible);
}