using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mascote : MonoBehaviour
{
    private GameObject player; // Referência ao jogador
    public float followDistance = 2f; // Distância para seguir o jogador
    [SerializeField] private float speed;
    public GameObject trapPrefab; // Prefab da armadilha
    public Transform trapSpawnPoint; // Local onde a armadilha será colocada
    public float trapCooldown = 3f; // Tempo entre cada armadilha
    [SerializeField] private ParticleSystem cura;


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
    private Animator animator;

    // Para o pathfinding
    private Pathfinding2 pathfinding;
    private List<Node> path;
    private int targetIndex = 0;
    private Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Animator anim;




    // Enum para a máquina de estados
    public enum MascoteState
    {
        FollowingPlayer, // Seguir o jogador
        Healing,         // Curar o jogador
        Attacking        // Atacar inimigos
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player"); // Inicializa a referência ao jogador usando a tag "Player"
        lastPosition = transform.position; // Inicializa a posição anterior
        currentDirection = Vector3.right; // Direção inicial
        currentState = MascoteState.FollowingPlayer; // Estado inicial
        sistemaVida = player.GetComponent<SistemaVida>(); // Obtém o componente SistemaVida do jogador
        pathfinding = GetComponent<Pathfinding2>(); // Obtém o Pathfinding
        rigidbody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        healTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        trapTimer += Time.deltaTime;

        switch (currentState)
        {
            case MascoteState.FollowingPlayer:
                InvokeRepeating("FollowPlayer", 0f, 3f);
                UpdateTrapSpawnPoint();
                HandleTrapPlacement();
                CheckForNearbyEnemies();
                CheckPlayerHealth();
                break;

            case MascoteState.Healing:
                HealPlayer();
                cura.Play();
                break;

            case MascoteState.Attacking:
                AttackEnemy();
                break;
        }
    }

    void FollowPlayer()
    {
        // Calcular caminho até o jogador usando Pathfinding
        pathfinding.FindPath(transform.position, player.transform.position);
        path = GridManager.grid.path; // Obtém o caminho calculado do GridManager

        if (path != null && player != null && path.Count > 0)
        {
            // Seguir o caminho
            Node currentNode = path[targetIndex];
            Vector2 targetPosition = new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.y);
            Vector2 currentPosition = rigidbody.position;
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
            rigidbody.MovePosition(newPosition);


            // Verifica se o mascote alcançou o próximo nó
            if (Vector2.Distance(currentPosition, targetPosition) < 0.3f)
            {
                targetIndex++;
                if (targetIndex >= path.Count)
                {
                    targetIndex = 0;
                    path=null;
                }
            }

            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Ajusta a orientação do sprite
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }

            // Atualiza a animação
            anim.SetBool("andando", true);
            
            
        }
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
