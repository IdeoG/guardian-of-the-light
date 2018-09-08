using System.Collections.Generic;
using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    [Header("Player initial point.")] [SerializeField]
    private Transform _playerPosition;

    [Space(10)] [Header("Available location portals.")] [SerializeField]
    private List<Transform> _portals;

    private void Awake()
    {
        var portal = _portals[SceneBundleSystem.StartPointNumber];

        _playerPosition.position = portal.position;
    }
}