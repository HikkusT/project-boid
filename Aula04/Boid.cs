using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public List<Boid> neighbors;
    public Vector3 velocity;
    public float minSpeed;
    public float maxSpeed;
    public float maxAcceleration;
    public float viewRadius;
    public float viewAngle;
    public float separationRadius;
    public bool isSelected;
    public int phiStep;

    private Vector3 acceleration = Vector3.zero;

    void Start()
    {
        separationRadius = viewRadius / 2;
    }

    void Update()
    {
        if (isSelected)
            foreach (Boid neighbor in neighbors)
                Debug.DrawLine(transform.position, neighbor.transform.position, Color.magenta);

        acceleration = Vector3.zero;

        Align();
        Cohesion();
        Separation();
        if (IsHeadingForCollision())
            AvoidObstacles();

        // Atualizando os vetores
        acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed); // Travando numa velocidade máxima
        if (velocity.magnitude < minSpeed)
            velocity = velocity.normalized * minSpeed;
        transform.position = transform.position + velocity * Time.deltaTime;

        // Alinhando a rotação
        transform.right = velocity;

        // Debug para visualizar os vetores
        if (isSelected)
        {
            Debug.DrawRay(transform.position, velocity, Color.red);
            Debug.DrawRay(transform.position + velocity, acceleration, Color.yellow);
        }
    }

    public Vector3 SteerTowards(Vector3 targetVelocity)
    {
        return targetVelocity.normalized * maxSpeed - velocity;
    }

    public bool isInsideFOV(Vector3 target)
    {
        Vector3 distance = target - transform.position;
        float angle = Vector3.Angle(velocity, distance);
        return distance.magnitude <= viewRadius && angle <= viewAngle / 2;
    }

    private void Align()
    {
        Vector3 accumulatedVelocity = Vector3.zero;
        if (neighbors.Count != 0)
        {
            foreach (Boid neighbor in neighbors)
                accumulatedVelocity += neighbor.velocity.normalized;

            Vector3 desiredVelocity = accumulatedVelocity / neighbors.Count;
            acceleration += SteerTowards(desiredVelocity);
        }
    }

    private void Cohesion()
    {
        Vector3 accumulatedPosition = Vector3.zero;
        if (neighbors.Count != 0)
        {
            foreach (Boid neighbor in neighbors)
                accumulatedPosition += neighbor.transform.position;

            accumulatedPosition /= neighbors.Count;

            if (isSelected)
                Debug.DrawLine(transform.position, accumulatedPosition, Color.green);
            Vector3 desiredVelocity = accumulatedPosition - transform.position;
            acceleration += SteerTowards(desiredVelocity);
        }
    }

    private void Separation()
    {
        Vector3 accumulatedAvoiding = Vector3.zero;
        int count = 0;
        if (neighbors.Count != 0)
        {
            foreach (Boid neighbor in neighbors)
            {
                Vector3 delta = transform.position - neighbor.transform.position;
                if (delta.magnitude <= separationRadius)
                {
                    accumulatedAvoiding += delta / delta.sqrMagnitude;
                    count++;
                }
            }

            if (count > 0)
            {
                Vector3 desiredVelocity = accumulatedAvoiding / count;
                acceleration += SteerTowards(desiredVelocity) * 1.5f;
            }
        }
    }

    // Verifica se vai bater
    private bool IsHeadingForCollision()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position, 2, velocity, out hit, viewRadius);
    }

    private void AvoidObstacles()
    {
        // For para gerar o phi, aumentando de pouco em pouco(phistep)
        for (int i = 0; i < viewAngle / 2; i += phiStep)
        {
            float phi = i * Mathf.Deg2Rad;

            // For para gerar o theta, que tem mais pontos conforme phi(i) vai aumentando
            for(int j = 0; j < 4 * i; j++)
            {
                float theta = (j / (float)(4 * i)) * 360 * Mathf.Deg2Rad;

                // Transformando de coordenadas esféricas para coordenadas cartesianas
                float x = Mathf.Cos(phi);
                float y = Mathf.Sin(phi) * Mathf.Cos(theta);
                float z = Mathf.Sin(phi) * Mathf.Sin(theta);
                Vector3 dir = new Vector3(x, y, z);

                // Aplicando a rotação do nosso boid na nossa direção
                dir = transform.TransformDirection(dir);

                // Testando se essa direção ainda colide. Se não, girar para essa direção e retornar a função
                // Já que não faz mais sentido buscar novas direções
                RaycastHit hit;
                if (!Physics.SphereCast(transform.position, 2, dir, out hit, viewRadius))
                {
                    acceleration = SteerTowards(dir) * 30;
                    return;
                }

                Debug.DrawRay(transform.position, 4 * dir, Color.cyan);
            }
        }
    }
}
