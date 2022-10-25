using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject playerobject;


    // Start is called before the first frame update
    void Start()
    {
        //grabs player from another scene and puts it in spawn position

        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject.Instantiate(playerobject, gameObject.transform);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = gameObject.transform.position;
        }
    }
}
