using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectPoolerTest
{
    [Test]
    public void initialize_pool_size_to_2_and_verify()
    {
        GameObject emptyObject = new GameObject();

        GameObject objectPoolerBase = new GameObject();
        objectPoolerBase.AddComponent<ObjectPooler>();
        ObjectPooler objectPooler = objectPoolerBase.GetComponent<ObjectPooler>();
        objectPooler.pools = new List<ObjectPooler.PoolType>();
        ObjectPooler.PoolType poolType = new ObjectPooler.PoolType();

        poolType.initialPoolSize = 2;
        poolType.tag = "empty";
        poolType.maxPoolSize = 2;
        poolType.newObjectsBatchSize = 1;
        poolType.objectPrefab = emptyObject;

        objectPooler.pools.Add(poolType);
        objectPooler.Initialize();

        ObjectPooler.Pool emptyPool = objectPooler.getPool("empty");
        Assert.AreEqual(2, emptyPool.getPoolItemsCount());

    }

}
