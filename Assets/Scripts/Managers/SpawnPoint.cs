using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public void Spawn(GameObject prefab)
    {
        //Debug.Log("Spawn");
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

}
