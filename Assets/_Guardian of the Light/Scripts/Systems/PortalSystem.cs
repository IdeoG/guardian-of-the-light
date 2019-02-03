using System.Collections.Generic;
using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    [Header("Available location portals.")] [SerializeField]
    private List<Transform> _portals;
    /**
     * 0 - self position
     * 1..10 - static positions
     */

    private void Start()
    {
        var portal = _portals[SceneBundleSystem.SpawnPointNumber];

        GGameManager.Instance.Player.transform.position = portal.position;
        GGameManager.Instance.Player.transform.rotation = portal.rotation;
    }
}