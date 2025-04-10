using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanetObjectPool : MonoBehaviour
{
   public static PlanetObjectPool Instance;
    [Serializable] public class PlanetPool
    {
        public GameObject prefab;
        public int size;
    }
    [Header("Pool cho planet sau khi ném (có Rigidbody)")]
    public List<PlanetPool> planetPhysicsPools;

    [Header("Pool cho planet chờ ném (không Rigidbody)")]
    public List<PlanetPool> planetNoPhysicsPools;

    private Dictionary<GameObject, Stack<GameObject>> _physicsPool;
    private Dictionary<GameObject, Stack<GameObject>> _noPhysicsPool;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        InitializePools();
    }
    private void InitializePools()
    {
        _physicsPool = new Dictionary<GameObject, Stack<GameObject>>();
        _noPhysicsPool = new Dictionary<GameObject, Stack<GameObject>>();

        // Khởi tạo pool cho physics
        foreach (var pool in planetPhysicsPools)
        {
            var objectStack = new Stack<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectStack.Push(obj);
            }
            _physicsPool.Add(pool.prefab, objectStack);
        }

        // Khởi tạo pool cho no physics
        foreach (var pool in planetNoPhysicsPools)
        {
            var objectStack = new Stack<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectStack.Push(obj);
            }
            _noPhysicsPool.Add(pool.prefab, objectStack);
        }
    }
    public GameObject GetPhysicsPlanet(GameObject prefab)
    {
        return GetPlanetFromPool(_physicsPool, prefab);
    }

    public GameObject GetNoPhysicsPlanet(GameObject prefab)
    {
        return GetPlanetFromPool(_noPhysicsPool, prefab);
    }

    private GameObject GetPlanetFromPool(Dictionary<GameObject, Stack<GameObject>> poolDict, GameObject prefab)
    {
        if (poolDict.ContainsKey(prefab))
        {
            if (poolDict[prefab].Count == 0)
            {
                ExpandPool(poolDict, prefab);
            }

            GameObject obj = poolDict[prefab].Pop();
            obj.SetActive(true);
            return obj;
        }

        Debug.LogError($"Prefab {prefab.name} not found in pool dictionary!");
        return null;
    }

    public void ReturnPhysicsPlanet(GameObject prefab, GameObject obj)
    {
        ReturnPlanetToPool(_physicsPool, prefab, obj);
    }

    public void ReturnNoPhysicsPlanet(GameObject prefab, GameObject obj)
    {
        ReturnPlanetToPool(_noPhysicsPool, prefab, obj);
    }
    private void ReturnPlanetToPool(Dictionary<GameObject, Stack<GameObject>> poolDict, GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);

        if (poolDict.ContainsKey(prefab))
        {
            // Reset vật lý nếu cần
            var rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            poolDict[prefab].Push(obj);
        }
        else
        {
            Debug.LogError($"Prefab {prefab.name} not found in pool dictionary!");
        }
    }
    private void ExpandPool(Dictionary<GameObject, Stack<GameObject>> poolDict, GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(false);
        poolDict[prefab].Push(newObj);
    }

}
