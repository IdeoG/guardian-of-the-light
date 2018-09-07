using Cinemachine;
using UnityEngine;

public class HouseTriggerEvent : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCameraBase _houseCameraSystem;
    [SerializeField] private CinemachineVirtualCameraBase _playerFreeCamera;

    public void OnHouseEnter()
    {
        _houseCameraSystem.Priority = 11;
        _playerFreeCamera.Priority = 10;
    }

    public void OnHouseExit()
    {
        _houseCameraSystem.Priority = 10;
        _playerFreeCamera.Priority = 11;
    }
}