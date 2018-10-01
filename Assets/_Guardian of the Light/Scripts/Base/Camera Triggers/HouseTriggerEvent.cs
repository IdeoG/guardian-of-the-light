using Cinemachine;
using UnityEngine;

public class HouseTriggerEvent : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCameraBase _enterCamera;
    [SerializeField] private CinemachineVirtualCameraBase _exitCamera;

    public void OnHouseEnter()
    {
        _enterCamera.Priority = 11;
        _exitCamera.Priority = 10;

        _enterCamera.enabled = true;
        _exitCamera.enabled = false;
    }

    public void OnHouseExit()
    {
        _enterCamera.Priority = 10;
        _exitCamera.Priority = 11;
        
        _enterCamera.enabled = false;
        _exitCamera.enabled = true;
    }
}