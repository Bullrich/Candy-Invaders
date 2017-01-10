using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich
namespace Game.Manager {
    public class PoolManager : MonoBehaviour {
        [SerializeField]
        public PoolValues[] Pool;
        GameObject[] poolContainer;

        List<GameObject>[] pooledObjects;

        /// <summary>Call the methods by it's instance</summary>
        //public static PoolManager instance;

        public void Start()
        {
            //instance = this;
            poolContainer = new GameObject[Pool.Length];
            pooledObjects = new List<GameObject>[Pool.Length];

            for (int i = 0; i < Pool.Length; i++)
            {
                CreatePool(i);
            }
        }

        void CreatePool(int index)
        {
            poolContainer[index] = new GameObject();
            poolContainer[index].name = Pool[index].pooledObject.name + " container";
            pooledObjects[index] = new List<GameObject>();
            Pool[index].objectName = Pool[index].pooledObject.name;
            for (int i = 0; i < Pool[index].pooledAmount; i++)
            {
                GameObject obj = (GameObject)Instantiate(Pool[index].pooledObject);
                obj.SetActive(false);
                pooledObjects[index].Add(obj);
                obj.transform.SetParent(poolContainer[index].transform);
            }
        }
        /// <summary>Get a non active pooled object, or, if the pool was allowed to grow, it instance a new object and returns it</summary>
        /// <param name="objectToGet">The pooled object name (be careful, it is the Prefab name, not the one assigned in the "object name" string field.</param>
        /// <returns></returns>
        public GameObject GetPooledObject(string objectToGet)
        {
            int index = 0;
            for (int i = 0; i < Pool.Length; i++)
                if (Pool[i].objectName == objectToGet)
                    index = i;

            for (int i = 0; i < pooledObjects[index].Count; i++)
            {
                if (!pooledObjects[index][i].activeInHierarchy)
                {
                    return pooledObjects[index][i];
                }
            }

            if (Pool[index].willGrow)
            {
                GameObject obj = (GameObject)Instantiate(Pool[index].pooledObject);
                pooledObjects[index].Add(obj);
                obj.transform.SetParent(poolContainer[index].transform);
                return obj;
            }
            return null;
        }

        /// <summary>Get a non active pooled object, or, if the pool was allowed to grow, it instance a new object and returns it</summary>
        /// <param name="objectToGet">The pooled object type, you can send the script of the object you wish to be returned</param>
        /// <returns></returns>
        public GameObject GetPooledObject(object objectToGet)
        {
            int index = 0;
            for (int i = 0; i < Pool.Length; i++)
                if (Pool[i].pooledObject.GetComponent<IPoolObject>().getObject().Equals(objectToGet))
                    index = i;

            for (int i = 0; i < pooledObjects[index].Count; i++)
            {
                if (!pooledObjects[index][i].activeInHierarchy)
                {
                    return pooledObjects[index][i];
                }
            }

            if (Pool[index].willGrow)
            {
                GameObject obj = (GameObject)Instantiate(Pool[index].pooledObject);
                pooledObjects[index].Add(obj);
                obj.transform.SetParent(poolContainer[index].transform);
                return obj;
            }
            return null;
        }

        public IPoolObject GetPooledInterface(string objectToGet)
        {
            int index = 0;
            for (int i = 0; i < Pool.Length; i++)
                if (Pool[i].objectName == objectToGet)
                    index = i;

            for (int i = 0; i < pooledObjects[index].Count; i++)
            {
                if (!pooledObjects[index][i].activeInHierarchy)
                {
                    return pooledObjects[index][i].GetComponent<IPoolObject>();
                }
            }

            if (Pool[index].willGrow)
            {
                GameObject obj = (GameObject)Instantiate(Pool[index].pooledObject);
                pooledObjects[index].Add(obj);
                obj.transform.SetParent(poolContainer[index].transform);
                return obj.GetComponent<IPoolObject>();
            }
            return null;
        }
    }

    public interface IPoolObject
    {
        void SetUp(Vector2 position);
        object getObject();
    }

    [System.Serializable]
    public class PoolValues
    {
        [Tooltip("This field autocompletes when the game is started, it will transform into the pooled object name, ignoring whatever you wrote there")]
        public string objectName = "pooledObject name";
        [Tooltip("Object that will ocupy a pool")]
        public GameObject pooledObject;
        [Range(1, 35)]
        public int pooledAmount = 20;
        [Tooltip("If the pooled amount is surpassed, can the game spawn a new one?")]
        public bool willGrow = true;
    }
}
