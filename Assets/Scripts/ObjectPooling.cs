using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool, transform);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    public GameObject GetPooledObject(Vector3 spawnPoint, Quaternion spawnRotation)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeSelf)
            {
                pooledObjects[i].transform.position = spawnPoint;
                pooledObjects[i].transform.rotation = spawnRotation;
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }
        return null;
    }
    public void DeactivatePooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (pooledObjects[i].activeSelf)
            {
                pooledObjects[i].SetActive(false);
            }
        }
    }
}