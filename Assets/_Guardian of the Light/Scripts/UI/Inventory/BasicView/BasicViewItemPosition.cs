using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BasicViewItemPosition : MonoBehaviour
{
    public int CurrentPosition = -1;
    public int SelfPosition;

    [HideInInspector] public RectTransform RectTransform;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }
}