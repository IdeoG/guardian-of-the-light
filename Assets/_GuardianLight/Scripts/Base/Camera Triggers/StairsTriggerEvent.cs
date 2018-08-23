using UnityEngine;

public class StairsTriggerEvent : MonoBehaviour
{
    [SerializeField] private GameObject _stairsTrackedCamera;

    public void OnStairsEnter()
    {
        _stairsTrackedCamera.SetActive(true);
    }

    public void OnStairsExit()
    {
        _stairsTrackedCamera.SetActive(false);
    }
}