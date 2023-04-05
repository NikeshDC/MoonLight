using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    private Camera camera;
    private float frustumHeight;  //half of frustum-width of the camera at given distance from the camera
    private float frustumWidth;

    public CloudSpawner owner;

    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        float distance = -camera.transform.position.z;
        frustumHeight = 2 * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * camera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
    }
}
