using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFrustum : MonoBehaviour
{
    public float frustumCalculateDistance;  //the distance from camera where frustum height/width is to be calculaed
    private float frustumHeight;  //frustum-width of the camera at given distance from the camera
    private float frustumWidth;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        frustumHeight = 2 * frustumCalculateDistance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * camera.aspect;
    }
}
