using UnityEngine;

/// <summary>
///     Rotates object to camera
/// </summary>
public class CameraBillboardEx : MonoBehaviour
{
    public Vector3 Axis = new Vector3(1, 1, 1);
    public bool IsLocalRotation;
    public string CameraTag = "MainCamera";

    private Camera _camera;

    private void OnEnable()
    {
        var cameraGO = GameObject.FindGameObjectWithTag(CameraTag);
        if(cameraGO != null)
        {
            _camera = cameraGO.GetComponent<Camera>();
        }
    }

    private void LateUpdate()
    {
        if (_camera != null)
        {
            var dir = transform.position - _camera.transform.position;
            dir.x *= Axis.x;
            dir.y *= Axis.y;
            dir.z *= Axis.z;

            dir.Normalize();
            if (!IsLocalRotation)
                transform.rotation = Quaternion.LookRotation(-dir, transform.root.up);
            else
                transform.localRotation = Quaternion.LookRotation(-dir, transform.root.up);
        }
        else
        {
            Debug.LogError("Failed to find camera with tag: " + CameraTag);
        }
    }
}