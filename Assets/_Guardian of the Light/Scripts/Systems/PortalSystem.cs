using System.Collections.Generic;
using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    [Header("Available location portals.")] [SerializeField]
    private List<Transform> _portals;

    private void Start()
    {
        var portal = _portals[SceneBundleSystem.SpawnPointNumber];
            
        GameManagerSystem.Instance.Player.transform.position = portal.position;
        GameManagerSystem.Instance.Player.transform.rotation = portal.rotation;
    }
}