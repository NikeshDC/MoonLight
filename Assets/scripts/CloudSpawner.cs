using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public bool spawnCloud = true; //should new cloud be spawned 

    public float cloudSpawnRate;   //how frequently should the cloud be spawned
    public float cloudSpawnRateVariance;
    private float cloudSpawnTimer;  //inverse of cloud spawn rate; roughly represents how often to spawn new cloud
    public float cloudSpawnTimerVariance;

    public float maxCloudHeight;  //maximum y-position where cloud can be spawned
    public float minCloudHeight;  //randomly spwans cloud between max and min y-positions

    public ObjectPooler objectPooler; //use pooler to spawn new clouds
    public string cloudTag; //tag of cloud object in pooler

    public Transform cloudParent;  //parent of spawned clouds

    Camera mainCamera;
    float frustumWidth;

    void Start()
    {
        mainCamera = Camera.main;
        float distance = -mainCamera.transform.position.z;
        float frustumHeight = 2 * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * mainCamera.aspect;

        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        Vector3 spawnPoint = new Vector3();
        while (true)
        {
            spawnPoint.y = Random.Range(minCloudHeight, maxCloudHeight);
            spawnPoint.x = mainCamera.transform.position.x - 1.1f * frustumWidth;
            GameObject spawnedCloud = objectPooler.spawn(cloudTag, spawnPoint, Quaternion.identity);
            spawnedCloud.transform.SetParent(cloudParent, true);
            cloudSpawnTimer = 1 / cloudSpawnRate;
            cloudSpawnTimerVariance = 1 / cloudSpawnRateVariance;
            cloudSpawnTimer += Random.Range(0, cloudSpawnTimerVariance);
            yield return new WaitForSeconds(cloudSpawnTimer);
        }
    }

    public void destroyCloud(GameObject cloudObject)
    {
        objectPooler.destroy(cloudTag, cloudObject);
    }
}
