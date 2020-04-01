using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Passa um prefab pelo inspetor
    public GameObject gameObject;

    void Start()
    {
        // Cria um objeto na scene a partir desse prefab
        Instantiate(gameObject);
    }

    void Update()
    {
        Instantiate(gameObject);
    }
}
