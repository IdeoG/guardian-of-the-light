using System.Collections.Generic;
using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    [Header("Начальная позиция персонажа")] [SerializeField]
    private Transform _playerPosition;

    [Space(10)] [Header("Доступные порталы локации")] [SerializeField]
    private List<Transform> _portals;

    private void Awake()
    {
        var portal = _portals[SceneBundle.StartPointNumber];

        _playerPosition.position = portal.position;
    }
}