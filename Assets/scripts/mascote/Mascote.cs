using UnityEngine;

public class Mascote : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public float followDistance = 2f; // Distância para seguir o jogador
    public float speed = 3f; // Velocidade do mascote
    public GameObject trapPrefab; // Prefab da armadilha
    public Transform trapSpawnPoint; // Local onde a armadilha será colocada
    public float trapCooldown = 3f; // Tempo entre cada armadilha

    // Atributos para máquina de estados
    public float healThreshold = 30f; // Vida mínima do jogador para curar
    public float healAmount = 20f; // Quantidade de cura
    public float attackRange = 1f; // Distância para atacar um inimigo
    public float damageAmount = 5f; // Dano do mascote
    private MascoteState currentState; // Estado atual do mascote
    private GameObject closestEnemy; // Referência ao inimigo mais próximo
    private SistemaVida sistemaVida; // Referência ao SistemaVida do jogador

    public float healCooldown = 5f; // Tempo de cooldown entre curas
    public float attackCooldown = 3f; // Tempo de cooldown entre ataques
    private float healTimer = 0f; // Timer para controle do cooldown de cura
    private float attackTimer = 0f; // Timer para controle do cooldown de ataque

    private float trapTimer = 0f; // Contador para o cooldown das armadilhas
    private Vector3 lastPosition; // Posição anterior do mascote
    private Vector3 currentDirection; // Direção do movimento do mascote

    // Enum para a máquina de estados
    public enum MascoteState
    {
        FollowingPlayer, // Seguir o jogador
        Healing,         // Curar o jogador
        Attacking        // Atacar inimigos
    }

    void Start()
    {
        lastPosition = transform.position; // Inicializa a posição anterior
        currentDirection = Vector3.right; // Direção inicial
        currentState = MascoteState.FollowingPlayer; // Estado inicial
        sistemaVida = player.GetComponent<SistemaVida>(); // Obtém o componente SistemaVida do jogador
    }

    void Update()
    {

        healTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        trapTimer += Time.deltaTime;

        switch (currentState)
        {
            case MascoteState.FollowingPlayer:
                FollowPlayer();
                UpdateTrapSpawnPoint();
                HandleTrapPlacement();
                CheckForNearbyEnemies();
                CheckPlayerHealth();
                break;

            case MascoteState.Healing:
                HealPlayer();
                break;

            case MascoteState.Attacking:
                AttackEnemy();
                break;
        }
    }

    void FollowPlayer()
    {
        // Calcula a posição desejada para o mascote, baseada no jogador
        Vector3 targetPosition = player.position + new Vector3(followDistance, 0, 0); // Mantém uma distância fixa no eixo X
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void UpdateTrapSpawnPoint()
    {
        // Calcula a direção do movimento
        currentDirection = (transform.position - lastPosition).normalized;

        // Atualiza o TrapSpawnPoint para ficar na frente do mascote
        if (currentDirection != Vector3.zero)
        {
            trapSpawnPoint.localPosition = currentDirection * 1f; // 1 unidade na frente do mascote
        }

        lastPosition = transform.position; // Atualiza a posição anterior
    }

    void HandleTrapPlacement()
    {
        // Atualiza o timer para o cooldown das armadilhas
        trapTimer += Time.deltaTime;

        // Solta a armadilha se o cooldown acabou e a tecla for pressionada
        if (trapTimer >= trapCooldown && Input.GetKeyDown(KeyCode.Space)) // Usa tecla Space como teste
        {
            PlaceTrap();
            trapTimer = 0f; // Reseta o timer
        }
    }

    void PlaceTrap()
    {
        // Instancia a armadilha no TrapSpawnPoint
        if (trapPrefab != null && trapSpawnPoint != null)
        {
            Instantiate(trapPrefab, trapSpawnPoint.position, Quaternion.identity);
            Debug.Log("Armadilha colocada pelo mascote!");
        }
        else
        {
            Debug.LogWarning("TrapPrefab ou TrapSpawnPoint não configurados no Inspector!");
        }
    }

    void CheckForNearbyEnemies()
    {
        float nearestDistance = attackRange;
        closestEnemy = null;

        // Encontrar o inimigo mais próximo
        foreach (Collider2D enemy in Physics2D.OverlapCircleAll(transform.position, attackRange))
        {
            if (enemy.CompareTag("Inimigo"))
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    closestEnemy = enemy.gameObject;
                }
            }
        }

        // Se o inimigo estiver perto o suficiente, mudar para o estado de ataque
        if (closestEnemy != null)
        {
            currentState = MascoteState.Attacking;
            attackTimer = 0f;
        }
    }

    void CheckPlayerHealth()
    {
        // Verifique a vida do jogador. Supondo que o jogador tenha uma variável de vida pública
        if (sistemaVida != null && sistemaVida.vida < sistemaVida.vidaMax * (healThreshold / 100f))
        {
            currentState = MascoteState.Healing; // Se a vida do jogador estiver baixa, curar
            healTimer = 0f;
        }
    }

    void HealPlayer()
    {
        // Curando o jogador
        if (sistemaVida != null)
        {
            sistemaVida.PerderVida(-healAmount); // Passa valor negativo para curar
            Debug.Log("Mascote curou o jogador!");
            currentState = MascoteState.FollowingPlayer; // Voltar ao estado de seguir o jogador
        }
    }

    void AttackEnemy()
{
    if (closestEnemy != null)
    {
        // Obtém o componente VidaInimigo do inimigo
        VidaInimigo enemyHealth = closestEnemy.GetComponent<VidaInimigo>();

        if (enemyHealth != null)
        {
            // Aplica dano ao inimigo
            enemyHealth.PerderVida(damageAmount); // A função PerderVida aplica o dano
            Debug.Log("Mascote atacou o inimigo!");

            // Volta para o estado de seguir o jogador após atacar
            currentState = MascoteState.FollowingPlayer; 
        }
    }
}

}
