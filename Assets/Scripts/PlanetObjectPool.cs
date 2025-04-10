using UnityEngine;
using System.Collections.Generic;

public class PlanetObjectPool : MonoBehaviour
{
    public static PlanetObjectPool Instance;

    [System.Serializable]
    public class PlanetPool
    {
        public string tag;
        public GameObject physicsPrefab;
        public GameObject nonPhysicsPrefab;
        public int size;
    }

    [SerializeField] private List<PlanetPool> planetPools;
    private Dictionary<string, Queue<GameObject>> physicsPool;
    private Dictionary<string, Queue<GameObject>> nonPhysicsPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePools();
    }

    private void InitializePools()
    {
        physicsPool = new Dictionary<string, Queue<GameObject>>();
        nonPhysicsPool = new Dictionary<string, Queue<GameObject>>();

        foreach (PlanetPool planetPool in planetPools)
        {
            Queue<GameObject> physicsObjects = new Queue<GameObject>();
            Queue<GameObject> nonPhysicsObjects = new Queue<GameObject>();

            for (int i = 0; i < planetPool.size; i++)
            {
                CreateAndAddToPool(planetPool.physicsPrefab, planetPool.tag, true, physicsObjects);
                CreateAndAddToPool(planetPool.nonPhysicsPrefab, planetPool.tag, false, nonPhysicsObjects);
            }

            physicsPool.Add(planetPool.tag, physicsObjects);
            nonPhysicsPool.Add(planetPool.tag, nonPhysicsObjects);
        }
    }

    private void CreateAndAddToPool(GameObject prefab, string tag, bool isPhysics, Queue<GameObject> queue)
    {
        GameObject obj = Instantiate(prefab);
        obj.tag = tag;
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        
        // Reset components
        if (isPhysics)
        {
            var rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
        
        var informer = obj.GetComponent<ColliderInformer>();
        if (informer != null)
        {
            informer.ResetCollisionState();
        }

        queue.Enqueue(obj);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, bool isPhysics, Transform parent = null)
    {
        Dictionary<string, Queue<GameObject>> targetPool = isPhysics ? physicsPool : nonPhysicsPool;

        if (!targetPool.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} not found (physics: {isPhysics})");
            return null;
        }

        Queue<GameObject> queue = targetPool[tag];
        if (queue.Count == 0)
        {
            // Attempt to expand pool
            PlanetPool poolConfig = planetPools.Find(p => p.tag == tag);
            if (poolConfig != null)
            {
                CreateAndAddToPool(isPhysics ? poolConfig.physicsPrefab : poolConfig.nonPhysicsPrefab, 
                                 tag, isPhysics, queue);
                Debug.LogWarning($"Expanding pool for tag {tag} (physics: {isPhysics})");
            }
            else
            {
                Debug.LogError($"Pool exhausted for tag {tag} (physics: {isPhysics})");
                return null;
            }
        }

        GameObject obj = queue.Dequeue();
        ResetObject(obj, position, rotation, parent);
        return obj;
    }

    private void ResetObject(GameObject obj, Vector3 position, Quaternion rotation, Transform parent)
    {
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        
        var rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        var informer = obj.GetComponent<ColliderInformer>();
        if (informer != null)
        {
            informer.ResetCollisionState();
        }

        obj.SetActive(true);
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;

        string tag = obj.tag;
        bool isPhysics = obj.GetComponent<Rigidbody2D>() != null;
        Dictionary<string, Queue<GameObject>> targetPool = isPhysics ? physicsPool : nonPhysicsPool;

        if (!targetPool.ContainsKey(tag))
        {
            Debug.LogError($"Cannot return object to pool: pool with tag {tag} not found");
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(transform);
        
        var rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        targetPool[tag].Enqueue(obj);
    }
}
