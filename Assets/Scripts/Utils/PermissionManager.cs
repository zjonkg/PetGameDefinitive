#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.Android;

public class PermissionManager : MonoBehaviour
{
    private readonly string[] requiredPermissions = {
        Permission.Camera,
        Permission.Microphone,
        Permission.FineLocation
    };

    private void Start()
    {
        RequestAllPermissions();
    }

    private void RequestAllPermissions()
    {
        foreach (string permission in requiredPermissions)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
    }

    public bool AreAllPermissionsGranted()
    {
        foreach (string permission in requiredPermissions)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                return false;
            }
        }
        return true;
    }
}
#endif
