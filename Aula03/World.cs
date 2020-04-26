using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Boid2D prefab;
    public int numBoids;
    public List<Boid2D> boids;
    private Vector2 bl, tr, worldDimensions;

    // Start is called before the first frame update
    void Start()
    {
        bl = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -10));
        tr = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -10));
        worldDimensions = new Vector2(tr.x - bl.x, tr.y - bl.y);

        for (int i = 0; i < numBoids; i++)
        {
            Boid2D boid = Instantiate(prefab, Random.insideUnitCircle * 3, Quaternion.identity);
            boids.Add(boid);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Boid2D boid in boids)
            FixPosition(boid);

        // A cada frame temos que lembrar de atualizar os vizinhos de cada boid
        foreach (Boid2D boid in boids)
            UpdateNeighbors(boid);
    }

    // Faz dar a volta no mapa
    void FixPosition(Boid2D subject)
    {
        float boundedX = Mathf.Repeat(subject.transform.position.x - bl.x, worldDimensions.x) + bl.x;
        float boundedY = Mathf.Repeat(subject.transform.position.y - bl.y, worldDimensions.y) + bl.y;
        subject.transform.position = new Vector2(boundedX, boundedY);
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
