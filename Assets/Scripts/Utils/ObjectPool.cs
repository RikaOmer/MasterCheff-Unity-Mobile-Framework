using UnityEngine;
using System.Collections.Generic;

namespace MasterCheff.Utils
{
    public class ObjectPool : MonoBehaviour
    {
        [System.Serializable] public class Pool { public string tag; public GameObject prefab; public int size = 10; public bool expandable = true; }
        [SerializeField] private List<Pool> _pools;

        private Dictionary<string, Queue<GameObject>> _poolDict = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, Pool> _settings = new Dictionary<string, Pool>();
        private Dictionary<string, int> _activeCount = new Dictionary<string, int>();

        private static ObjectPool _instance;
        public static ObjectPool Instance => _instance ?? (_instance = FindObjectOfType<ObjectPool>() ?? new GameObject("[ObjectPool]").AddComponent<ObjectPool>());

        private void Awake()
        {
            if (_instance == null) { _instance = this; DontDestroyOnLoad(gameObject); Initialize(); }
            else if (_instance != this) Destroy(gameObject);
        }

        private void Initialize()
        {
            if (_pools == null) return;
            foreach (var p in _pools) CreatePool(p);
        }

        private void CreatePool(Pool pool)
        {
            var q = new Queue<GameObject>();
            var container = new GameObject($"Pool_{pool.tag}"); container.transform.SetParent(transform);
            for (int i = 0; i < pool.size; i++) { var obj = Instantiate(pool.prefab, container.transform); obj.SetActive(false); q.Enqueue(obj); }
            _poolDict[pool.tag] = q; _settings[pool.tag] = pool; _activeCount[pool.tag] = 0;
        }

        public void RegisterPool(string tag, GameObject prefab, int size = 10) { if (!_poolDict.ContainsKey(tag)) CreatePool(new Pool { tag = tag, prefab = prefab, size = size }); }

        public GameObject Spawn(string tag, Vector3 pos, Quaternion rot)
        {
            if (!_poolDict.ContainsKey(tag)) return null;
            var pool = _poolDict[tag];
            GameObject obj = pool.Count > 0 ? pool.Dequeue() : (_settings[tag].expandable ? Instantiate(_settings[tag].prefab) : null);
            if (obj == null) return null;
            obj.transform.SetPositionAndRotation(pos, rot); obj.SetActive(true); _activeCount[tag]++;
            return obj;
        }

        public void Despawn(string tag, GameObject obj)
        {
            if (!_poolDict.ContainsKey(tag)) { Destroy(obj); return; }
            obj.SetActive(false); _poolDict[tag].Enqueue(obj); _activeCount[tag]--;
        }

        public void DespawnDelayed(string tag, GameObject obj, float delay) { StartCoroutine(DelayedDespawn(tag, obj, delay)); }
        private System.Collections.IEnumerator DelayedDespawn(string tag, GameObject obj, float delay) { yield return new WaitForSeconds(delay); if (obj && obj.activeInHierarchy) Despawn(tag, obj); }
    }

    public class PooledObject : MonoBehaviour
    {
        public string PoolTag { get; set; }
        public virtual void OnSpawn() { }
        public virtual void OnDespawn() { }
        public void ReturnToPool() { if (!string.IsNullOrEmpty(PoolTag)) ObjectPool.Instance.Despawn(PoolTag, gameObject); else gameObject.SetActive(false); }
    }
}
