using UnityEngine;

/// <summary>
///     Rotates object to camera
/// </summary>
public class CameraBillboardEx : MonoBehaviour
{
    public Vector3 Axis = new Vector3(1, 1, 1);
    public bool IsLocalRotation;

    private void LateUpdate()
    {
        var camera = Camera.main;
        if (camera != null)
        {
            var dir = transform.position - camera.transform.position;
            dir.x *= Axis.x;
            dir.y *= Axis.y;
            dir.z *= Axis.z;

            dir.Normalize();
            if (!IsLocalRotation)
                transform.rotation = Quaternion.LookRotation(-dir, transform.root.up);
            else
                transform.localRotation = Quaternion.LookRotation(-dir, transform.root.up);
        }
    }
}