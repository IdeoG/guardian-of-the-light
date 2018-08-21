using System.Collections.Generic;
using UnityEngine;

public class MinePortalSystem : MonoBehaviour
{
    [Header("Начальная позиция персонажа")] [SerializeField]
    private Transform _playerPosition;

    [Space(10)] [Header("Доступные порталы локации")] [SerializeField]
    private List<Transform> _portals;

    private void Awake()
    {
        var portal = _portals[SceneBundle.StartPointNumber];

        _playerPosition.position = portal.position;
        Debug.Log(string.Format("Awake: StartPointNumber = {0}, position = {1}", SceneBundle.StartPointNumber, _playerPosition.position));
    }
}