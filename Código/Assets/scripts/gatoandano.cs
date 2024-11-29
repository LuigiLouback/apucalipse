using UnityEngine;

public class gatoandano : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidade de movimento do gatinho
    public float areaRadius = 5f; // Raio da área onde ele pode se mover
    private Vector2 targetPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChooseNewPosition();
    }

    void Update()
    {
        MoveToTarget();
        
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            ChooseNewPosition();
        }
    }

    void ChooseNewPosition()
    {
        // Escolhe uma nova posição aleatória dentro do raio definido
        Vector2 randomDirection = Random.insideUnitCircle * areaRadius;
        targetPosition = (Vector2)transform.position + randomDirection;
    }

    void MoveToTarget()
    {
        // Move o gatinho para a nova posição aleatória
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Virar a sprite do gatinho dependendo da direção
        if (transform.position.x > targetPosition.x)
        {
            // Se estiver se movendo para a esquerda, inverte a sprite
            spriteRenderer.flipX = true;
        }
        else
        {
            // Se estiver se movendo para a direita, deixa a sprite normal
            spriteRenderer.flipX = false;
        }
    }
}
