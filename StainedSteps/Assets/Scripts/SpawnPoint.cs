using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //grabs player from another scene and puts it in spawn position
        GameObject.FindGameObjectWithTag("Player").transform.position = gameObject.transform.position;
    }
}
