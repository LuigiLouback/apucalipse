using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject player;
    private Animator anim;
    private Rigidbody2D rigidbody; // Rigidbody2D do inimigo
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Pathfinding pathfinding; // Referência ao componente Pathfinding
    private List<Node> path; // Caminho até o jogador
    private int targetIndex = 0; // Índice do nó atual do caminho

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>(); // Inicialize o Rigidbody2D
        pathfinding = GetComponent<Pathfinding>(); // Inicialize o Pathfinding

        InvokeRepeating("AtualizarCaminho", 0f, 0.5f); // Atualiza o caminho a cada 0.5 segundos
    }

    void AtualizarCaminho()
    {
        if (player != null)
        {
            Debug.Log("Atualizando caminho para o jogador...");

            // Calcula o caminho até o jogador
            pathfinding.FindPath(transform.position, player.transform.position);
            path = pathfinding.grid.path;

            if (path != null && path.Count > 0)
            {
                Debug.Log("Caminho encontrado com " + path.Count + " nós.");
                targetIndex = 0; // Reseta o índice do alvo
            }
            else
            {
                Debug.LogWarning("Nenhum caminho encontrado!");
            }
        }
    }

    void Update()
    {
        if (player != null && path != null && path.Count > 0)
        {
            Node currentNode = path[targetIndex];
            Vector2 targetPosition = new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.z);
            Vector2 currentPosition = rigidbody.position;

            // Move o inimigo ao longo do caminho
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
            rigidbody.MovePosition(newPosition);

            // Verificar se o inimigo alcançou o nó atual e atualizar o índice para o próximo nó
            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                Debug.Log("Alcançou o nó " + targetIndex);
                targetIndex++;
                if (targetIndex >= path.Count)
                {
                    Debug.Log("Caminho concluído!");
                    path = null; // Caminho concluído
                }
            }

            // Calcular a direção do movimento
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Verificar se o inimigo está se movendo e ajustar a orientação do sprite
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // Olha para a direita
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // Olha para a esquerda
            }

            // Atualizar a animação
            anim.SetBool("movendo", true);
        }
        else
        {
            // Parar a animação se não houver jogador ou caminho
            anim.SetBool("movendo", false);
        }
    }
}
