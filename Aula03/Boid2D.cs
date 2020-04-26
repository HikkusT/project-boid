using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid2D : MonoBehaviour
{
    public Vector2 velocity;
    public Vector2 acceleration;
    public float maxSpeed;
    public float minSpeed;
    public Vector2 target;
    public List<Boid2D> neighbors;
    public float viewRadius;
    public float viewAngle;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Random.insideUnitCircle.normalized * maxSpeed;
    }

    // Field of View simples. Calcula a distância e vê se é maior do que um raio definido para o objeto
    public bool IsInsideFieldOfView(Vector2 point)
    {
        float distance = (point - (Vector2)transform.position).magnitude;
        return distance <= viewRadius && Vector2.Angle((point - (Vector2)transform.position), velocity) <= viewAngle / 2;
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = Vector2.zero;
        // Debug de vizinhos
        foreach (Boid2D neighbor in neighbors)
            Debug.DrawLine(transform.position, neighbor.transform.position, Color.magenta);

        if (neighbors.Count > 0)
        {
            Align();
            Cohesion();
            Separation();
        }

        // Atualizando os vetores
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed); // Travando numa velocidade máxima
        if (velocity.magnitude < minSpeed)
            velocity = velocity.normalized * minSpeed;
        transform.position = transform.position + (Vector3)velocity * Time.deltaTime;

        // Alinhando a rotação
        transform.right = velocity;

        // Debug para visualizar os vetores
        Debug.DrawRay(transform.position, velocity, Color.red);
        Debug.DrawRay(transform.position + (Vector3)velocity, acceleration, Color.yellow);
    }

    void SteerTowards(Vector2 desiredVelocity, float intensity)
    {
        acceleration += (desiredVelocity.normalized * maxSpeed - velocity) * intensity;
    }

    void Align()
    {
        Vector2 sumVelocities = Vector2.zero;

        foreach (Boid2D neighbor in neighbors)
        {
            sumVelocities += neighbor.velocity;
        }

        sumVelocities /= neighbors.Count;
        SteerTowards(sumVelocities, 1);
    }

    void Cohesion()
    {
        Vector2 sumPositions = Vector2.zero;

        foreach (Boid2D neighbor in neighbors)
        {
            sumPositions += (Vector2)neighbor.transform.position;
        }

        Vector2 center = sumPositions / neighbors.Count;
        Vector2 desiredVelocity = center - (Vector2)transform.position;
        SteerTowards(desiredVelocity, 2);
    }

    void Separation()
    {
        Vector2 sumSeparations = Vector2.zero;

        foreach (Boid2D neighbor in neighbors)
        {
            Vector2 separation = transform.position - neighbor.transform.position;
            sumSeparations += separation / separation.sqrMagnitude;
        }

        Vector2 desiredVelocity = sumSeparations / neighbors.Count;
        SteerTowards(desiredVelocity, 2);
    }
}