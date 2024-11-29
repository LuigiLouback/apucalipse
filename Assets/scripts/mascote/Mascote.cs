using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mascote : MonoBehaviour
{
    private GameObject player; 
    public float followDistance = 2f; 
    [SerializeField] private float speed;
    public GameObject trapPrefab; 
    public Transform trapSpawnPoint;
    public float trapCooldown = 3f; 
    [SerializeField] private ParticleSystem cura;

    // Atributos da máquina de estados
    public float healThreshold = 30f; 
    public float healAmount = 20f; 
    public float attackRange = 1f;
    public float damageAmount = 1f;
    private MascoteState currentState;
    private GameObject closestEnemy;
    private SistemaVida sistemaVida;

    public float healCooldown = 5f;
    public float attackCooldown = 3f;
    private float healTimer = 0f;
    private float attackTimer = 0f;
    private float trapTimer = 0f;

    private Vector3 lastPosition;
    private Vector3 currentDirection;
    private Animator animator;

    // Para o pathfinding
    private Pathfinding2 pathfinding;
    private List<Node> path;
    private int targetIndex = 0;
    private Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Animator anim;
    public GameObject lightObject;

    // Intervalo de atualização de estado
    private float stateUpdateTimer = 0f;
    public float stateUpdateInterval = 0.5f;  // Atualiza a cada 0.5 segundos

    // Enum da máquina de estados
    public enum MascoteState
    {
        FollowingPlayer, // Seguir o jogador
        Healing,         // Curar o jogador
        Attacking        // Atacar inimigos
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        lastPosition = transform.position;
        currentDirection = Vector3.right;
        currentState = MascoteState.FollowingPlayer; 
        sistemaVida = player.GetComponent<SistemaVida>();
        pathfinding = GetComponent<Pathfinding2>(); 
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Atualiza os timers de cooldown
        healTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        trapTimer += Time.deltaTime;

        // Atualiza o timer para trocar de estado
        stateUpdateTimer += Time.deltaTime;

        // Verifica e executa a lógica de seguir o jogador continuamente
        FollowPlayer();

        // Atualiza o estado a cada intervalo de tempo
        if (stateUpdateTimer >= stateUpdateInterval)
        {
            // Reseta o timer de atualização de estado
            stateUpdateTimer = 0f;

            // Verifica a necessidade de curar ou atacar
            CheckForNearbyEnemies();
            CheckPlayerHealth();

            // Realiza ações baseadas no estado
            switch (currentState)
            {
                case MascoteState.FollowingPlayer:
                    // O mascote já está seguindo o jogador, não há necessidade de ações adicionais
                    break;
                case MascoteState.Healing:
                    HealPlayer();
                    break;
                case MascoteState.Attacking:
                    AttackEnemy();
                    break;
            }

            // Atualiza a posição do ponto de spawn da armadilha
            UpdateTrapSpawnPoint();

            // Atualiza e lida com as armadilhas
            HandleTrapPlacement();
        }
    }

    void FollowPlayer()
    {
        // Calcular caminho até o jogador usando Pathfinding
        pathfinding.FindPath(transform.position, player.transform.position);
        path = GridManager.grid.path;

        if (path != null && path.Count > 0)
        {
            Node currentNode = path[targetIndex];
            Vector2 targetPosition = new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.y);
            Vector2 currentPosition = rigidbody.position;
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
            rigidbody.MovePosition(newPosition);

            // Se o mascote alcançar o próximo nó
            if (Vector2.Distance(currentPosition, targetPosition) < 0.3f)
            {
                targetIndex++;
                if (targetIndex >= path.Count)
                {
                    targetIndex = 0;
                    path = null;
                }
            }

            // Ajusta a direção do sprite
            Vector2 direction = (player.transform.position - transform.position).normalized;
            if (direction.x > 0)
            {
                lightObject.transform.localPosition = new Vector2(Mathf.Abs(lightObject.transform.localPosition.x), lightObject.transform.localPosition.y);
                lightObject.transform.rotation = Quaternion.Euler(0, 0, 270);
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                lightObject.transform.localPosition = new Vector2(-Mathf.Abs(lightObject.transform.localPosition.x), lightObject.transform.localPosition.y);
                lightObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                spriteRenderer.flipX = true;
            }
        }
    }

    void UpdateTrapSpawnPoint()
    {
        // Atualiza a direção do mascote
        currentDirection = (transform.position - lastPosition).normalized;

        // Atualiza a posição do TrapSpawnPoint para ficar na frente do mascote
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

        if (closestEnemy != null)
        {
            currentState = MascoteState.Attacking;
            attackTimer = 0f;
        }
    }

    void CheckPlayerHealth()
    {
        if (sistemaVida != null && sistemaVida.vida < sistemaVida.vidaMax * (healThreshold / 100f))
        {
            if (healTimer >= healCooldown)
            {
                currentState = MascoteState.Healing;
                healTimer = 0f;
            }
        }
    }

    void HealPlayer()
    {
        if (sistemaVida != null)
        {
            sistemaVida.PerderVida(-healAmount); // Cura o jogador
            currentState = MascoteState.FollowingPlayer; // Volta para seguir o jogador após curar
        }
    }

    void AttackEnemy()
    {
        if (closestEnemy != null)
        {
            VidaInimigo enemyHealth = closestEnemy.GetComponent<VidaInimigo>();
            if (enemyHealth != null)
            {
                enemyHealth.PerderVida(damageAmount); // Aplica dano
                currentState = MascoteState.FollowingPlayer; // Volta para seguir o jogador após atacar
            }
        }
    }
}
