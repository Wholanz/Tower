using UnityEngine;
using System.Collections;

public class CameraPoint : MonoBehaviour 
{
    public static CameraPoint Instance = null;

    // ִ������Start����
    void Awake()
    {
        Instance = this;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "CameraPoint.tif");
    }

}
