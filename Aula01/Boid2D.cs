using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid2D : MonoBehaviour
{
    public Vector2 velocity;
    public float maxSpeed;
    public Vector2 target;

    private Vector2 acceleration = Vector2.zero;

    void Start()
    {
        
    }

    void Update()
    {
        // (DEMO) Seguindo o mouse
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SteerTowards((mousePos - (Vector2)transform.position));

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

    public void SteerTowards(Vector2 targetVelocity)
    {
        acceleration = targetVelocity - velocity;
    }
}
