using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Recebe um gameobject que vai atrás pelo inspetor
    public GameObject target;
    
    // Componentes da velocidade
    Vector3 dir;
    public float speed = 2;

    void Start()
    {
        // Pega referência do componente de MeshRenderer e troca a cor
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        // A cada frame calcula a nova direção que deve andar pra chegar no gameobject target
        dir = target.transform.position - transform.position;

        // Calcula a velocidade
        Vector3 velocity = speed * dir.normalized;

        // Anda na direção o equivalente a Time.deltaTime, que é o tempo de duração do frame
        transform.position = transform.position + velocity * Time.deltaTime;
    }
}
