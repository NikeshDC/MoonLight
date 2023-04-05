using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class PoolType
    {//definition for the type of pools to create
        public string tag; //tag is used to identify type of object to spawn
        public int initialPoolSize;  //number of objects to initially spawn
        public int newObjectsBatchSize;  //if new objects are required while all objects are already in the scene then add batchSize of new objects to the pool (maxPoolSize is applicable)
        public int maxPoolSize;   //maximum allowed number of objects in pool after which oldest object (may be present in scene) will be recycled immediately if to be spawned
        public GameObject objectPrefab;  //the prefab to use for spawing object
    }
    public class Pool {
        public PoolType poolType;
        private Queue<GameObject> pooledObjects;  //objects remaining in the pool
        private List<GameObject> spawnedObjects;  //objects that have been spawned to the scene

        public Pool(PoolType poolType)
        {
            this.poolType = poolType;
            this.pooledObjects = new Queue<GameObject>();
            addNewObjectsToPool(poolType.initialPoolSize);
            this.spawnedObjects = new List<GameObject>();
        }

        private void addNewObjectsToPool(int number)
        {
            if(pooledObjects == null)
            {
                Debug.LogWarning("No pool-queue instance created");
                return;
            }
            for (int i = 0; i < number; i++)
            {
                GameObject poolObject = Instantiate(poolType.objectPrefab);  //instantiate initial objects for pool
                poolObject.SetActive(false);   //make the object inactive so that it doesn't interact in scene
                pooledObjects.Enqueue(poolObject);  //add the object to the pool
            }
        }

        public int getPoolItemsCount() { return pooledObjects.Count;  }
        public int getSceneItemsCount() { return spawnedObjects.Count;  }

        public GameObject spawn(Vector3 position, Quaternion rotation)
        {
            if(pooledObjects.Count == 0)
            {//if remaining objects in pool is empty then add more items (either new or recycled from scene)
                if(spawnedObjects.Count >= poolType.maxPoolSize)
                {//no more new objects are permitted to be created so recycle oldest one from the scene
                    GameObject recylceObject = spawnedObjects[0];
                    spawnedObjects.RemoveAt(0);
                    pooledObjects.Enqueue(recylceObject);
                }
                else
                {//new objects can still be spawned
                    //int newObjectsSize = Math.min(poolType.newObjectsBatchSize, poolType.maxPoolSize - spawnedObjects.Count());
                    int newObjectsSize = poolType.newObjectsBatchSize;
                    addNewObjectsToPool(newObjectsSize);
                }
            }

            GameObject spawnObject = pooledObjects.Dequeue();
            spawnObject.SetActive(true);
            spawnObject.transform.position = position;
            spawnObject.transform.rotation = rotation;

            spawnedObjects.Add(spawnObject);   //add to spawned list so that it can be recycled later
            return spawnObject;
        }

        public bool destroy(GameObject destroyingObject)
        {
            bool destroyed = spawnedObjects.Remove(destroyingObject);
            if (destroyed)
            {//if the given item was found in the list
                destroyingObject.SetActive(false);
                pooledObjects.Enqueue(destroyingObject);
            }
            return destroyed;
        }
    }

    public List<PoolType> pools;  //the definitions of pools to create
    private Dictionary<string, Pool> objectPools;  //pools of instantiated gameobjects from where we can get new instances based on string tag

    public void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        objectPools = new Dictionary<string, Pool>();
        foreach(PoolType poolType in pools)
        {
            InitializePool(poolType);
        }
    }

    public void InitializePool(PoolType poolType)
    {
        if (objectPools.ContainsKey(poolType.tag))
        {//pool has already been created for given tag
            Debug.LogWarning("Pool for "+ poolType.tag + " has already been created");
            return;
        }

        Pool poolTypePool = new Pool(poolType);
        objectPools.Add(poolType.tag, poolTypePool);  //add the pool to dictionary of pools
    }

    public Pool getPool(string poolTag)
    {
        if (!objectPools.ContainsKey(poolTag))
        {//pool has already been created for given tag
            Debug.LogWarning("Pool for " + poolTag + " not created");
            return null;
        }
        return objectPools[poolTag];
    }

    public GameObject spawn(string poolTag, Vector3 position, Quaternion rotation)
    {
        if (! objectPools.ContainsKey(poolTag))
        {//no pool for given tag
            Debug.LogWarning("Pool for " + poolTag + " is not created");
            return null;
        }
        return objectPools[poolTag].spawn(position, rotation);
    }

    public bool destroy(string poolTag, GameObject destroyObject)
    {
        if (!objectPools.ContainsKey(poolTag))
        {//no pool for given tag
            Debug.LogWarning("Pool for " + poolTag + " is not created");
            return false;
        }
        return objectPools[poolTag].destroy(destroyObject);
    }
}
