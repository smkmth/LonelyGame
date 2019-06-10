using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    public bool shouldExpand;
    public int amountToPool;
    public GameObject objectToPool;


}

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler PoolerInstance;
    public List<PoolItem> itemsToPool;
    public List<GameObject> pooledObjects;
    GameObject sceneManager;

    // Use this for initialization
    void Awake()
    {
        PoolerInstance = this;
        sceneManager = GameObject.Find("SceneManager");
    }


    void Start()
    {


        pooledObjects = new List<GameObject>();
        foreach (PoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.transform.parent = sceneManager.transform;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }


    }



    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (PoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            
            }
        }
        return null;
    }

}


