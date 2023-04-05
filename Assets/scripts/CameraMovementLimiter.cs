using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovementLimiter : MonoBehaviour
{//limits movement of 2D camera to within bounds
    private Camera camera;
    private float frustumHeightby2;  //half of frustum-width of the camera at given distance from the camera
    private float frustumWidthby2;

    private Vector2 minCameraPos;   //calculated position of camera based on frustum-height/width and minBounds
    private Vector2 maxCameraPos;

    public Vector2 minBounds;
    public Vector2 maxBounds;

    void Start()
    {
        camera = GetComponent<Camera>();
        float distance = - gameObject.transform.position.z;   
        frustumHeightby2 = distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidthby2 = frustumHeightby2 * camera.aspect;

        minCameraPos = new Vector2( minBounds.x + frustumWidthby2, minBounds.y + frustumHeightby2);
        maxCameraPos = new Vector2(maxBounds.x - frustumWidthby2, maxBounds.y - frustumHeightby2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 getNewCameraPosition(Vector2 deltaPos)
    {
        Vector2 newpos = new Vector2();
        newpos.x = gameObject.transform.position.x + deltaPos.x;  //unfiltered value
        newpos.y = gameObject.transform.position.y + deltaPos.y;  //unfiltered value

        //filtering by applying limits
        if (deltaPos.x < 0){
            //movement is happening towards minimum value
            newpos.x = Mathf.Max(minCameraPos.x, newpos.x);
        }
        else {
            //movement is happening towards minimum value
                newpos.x = Mathf.Min(maxCameraPos.x, newpos.x);
        }


        if (deltaPos.y < 0)
        {
            //movement is happening towards minimum value
            newpos.y = Mathf.Max(minCameraPos.y, newpos.y);
        }
        else
        {
            //movement is happening towards minimum value
            newpos.y = Mathf.Min(maxCameraPos.y, newpos.y);
        }

        return newpos;
    }
}
