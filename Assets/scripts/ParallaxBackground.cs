using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform mainCamera;

    [SerializeField] private float parallaxEffect;
    private Vector2 lastCameraPosition;

    void Start()
    {
        mainCamera = Camera.main.transform;
	lastCameraPosition = new Vector2(mainCamera.position.x, mainCamera.position.y);
    }

    
    void LateUpdate()
    {
        Vector2 newCameraPosition =  new Vector2(mainCamera.position.x, mainCamera.position.y);
        Vector2 deltaPos = (newCameraPosition - lastCameraPosition) * parallaxEffect;
	this.transform.position += new Vector3(deltaPos.x, deltaPos.y, 0);
        lastCameraPosition = newCameraPosition;
    }
}
