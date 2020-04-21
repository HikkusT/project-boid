using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Boid2D prefab;
    public int numBoids;
    public List<Boid2D> boids;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn boids
        for (int i = 0; i < numBoids; i++)
        {
            Boid2D boid = Instantiate(prefab, Random.insideUnitCircle * 3, Quaternion.identity);
            boids.Add(boid);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // A cada frame temos que lembrar de atualizar os vizinhos de cada boid
        foreach (Boid2D boid in boids)
            UpdateNeighbors(boid);
    }

    void UpdateNeighbors(Boid2D subject)
    {
        List<Boid2D> updateNeighbors = new List<Boid2D>();

        // Para cada boid do mundo(caso não seja ele mesmo) perguntamos ao boid em questão se é vizinho 
        foreach (Boid2D boid in boids)
            if (boid != subject && subject.IsInsideFieldOfView(boid.transform.position))
                updateNeighbors.Add(boid);
        subject.neighbors = updateNeighbors;
    }
}
