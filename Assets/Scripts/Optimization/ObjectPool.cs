using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    public static Dictionary<string, GameObject> poolLookup = new Dictionary<string, GameObject>();
    public static Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    public static void EnqueueObject(GameObject item, string name)
    {
        if (!item.activeSelf) return;

        //item.transform.position = Vector3.zero;
        item.SetActive(false);
        poolDictionary[name].Enqueue(item);
    }

    public static GameObject DequeueObject(string key)
    {
        if (poolDictionary[key].TryDequeue(out var item))
        {
            return item;
        }

        return EnqueueNewInstance(poolLookup[key], key);
    }

    public static GameObject EnqueueNewInstance(GameObject prefab, string key)
    {
        GameObject newInstance = Object.Instantiate(prefab);
        newInstance.SetActive(false);
        newInstance.transform.position = Vector3.zero;
        poolDictionary[key].Enqueue(newInstance);
        return newInstance;
    }

    public static void SetupPool(GameObject prefab, int poolSize, string dictionaryEntry)
    {
        if (!poolDictionary.ContainsKey(dictionaryEntry))
        {
            poolDictionary.Add(dictionaryEntry, new Queue<GameObject>());
            poolLookup.Add(dictionaryEntry, prefab);

            for (int i = 0; i < poolSize; i++)
            {
                GameObject pooledInstance = Object.Instantiate(prefab);
                pooledInstance.SetActive(false);
                poolDictionary[dictionaryEntry].Enqueue(pooledInstance);
            }
        }
        else
        {
            Debug.LogWarning($"Pool with key '{dictionaryEntry}' already exists!");
        }
    }
}
