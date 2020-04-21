using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid2D : MonoBehaviour
{
    public Vector2 velocity;
    public Vector2 acceleration;
    public float maxSpeed;
    public Vector2 target;
    public List<Boid2D> neighbors;
    public float viewRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Field of View simples. Calcula a distância e vê se é maior do que um raio definido para o objeto
    public bool IsInsideFieldOfView(Vector2 point)
    {
        float distance = (point - (Vector2)transform.position).magnitude;
        return distance <= viewRadius;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug de vizinhos
        foreach (Boid2D neighbor in neighbors)
            Debug.DrawLine(transform.position, neighbor.transform.position, Color.magenta);

        // Removendo a Demo de seguir o mouse
        //acceleration = Vector2.zero;
        //target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 desiredVelocity = target - (Vector2)transform.position;
        //SteerTowards(desiredVelocity);

        // Atualizando os vetores
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed); // Travando numa velocidade máxima
        transform.position = transform.position + (Vector3)velocity * Time.deltaTime;

        // Alinhando a rotação
        transform.right = velocity;

        // Debug para visualizar os vetores
        Debug.DrawRay(transform.position, velocity, Color.red);
        Debug.DrawRay(transform.position + (Vector3)velocity, acceleration, Color.yellow);
    }

    void SteerTowards(Vector2 desiredVelocity)
    {
        acceleration += desiredVelocity - velocity;
    }
}