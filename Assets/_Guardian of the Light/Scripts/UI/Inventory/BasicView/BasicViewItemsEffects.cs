using UnityEngine;
using UnityEngine.UI;

public class BasicViewItemsEffects : MonoBehaviour, IBasicViewItemsEffects
{
    [SerializeField] private Text _description;
    [SerializeField] private RectTransform _lighting;
    [SerializeField] private GameObject _leftArrow;
    [SerializeField] private GameObject _rightArrow;
    
    public void SetLightingPosition(Vector3 position)
    {
        _lighting.SetPositionAndRotation(position, Quaternion.identity);
    }

    public void SetName(string text)
    {
        _description.text = text;
    }

    public void SetArrowsVisibility(bool isLeftArrowVisible, bool isRightArrowVisible)
    {
        _leftArrow.SetActive(isLeftArrowVisible);
        _rightArrow.SetActive(isRightArrowVisible);
    }
}