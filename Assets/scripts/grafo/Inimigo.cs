using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject player;
    private Animator anim;
    private Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Pathfinding pathfinding;
    private List<Node> path;
    private int targetIndex = 0;

    void Start()
    {
        // Obtém a referência ao jogador através do Jogador
        player = Jogador.PlayerTransform.gameObject;
        anim = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        pathfinding = GetComponent<Pathfinding>();

        InvokeRepeating("AtualizarCaminho", 0f, 2f);
    }

    void AtualizarCaminho()
    {
        if (player != null)
        {
            Debug.Log("Atualizando caminho para o jogador...");

            // Usa o Pathfinding para calcular o caminho até o jogador
            pathfinding.FindPath(transform.position, player.transform.position);
            path = GridManager.grid.path; // Obtém o caminho a partir do GridManager

            if (path != null && path.Count > 0)
            {
                Debug.Log("Caminho encontrado com " + path.Count + " nós.");
                targetIndex = 0;
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
            Vector2 targetPosition = new Vector2(currentNode.worldPosition.x, currentNode.worldPosition.y); // Mudança para o eixo Y
            Vector2 currentPosition = rigidbody.position;

            // Move o inimigo ao longo do caminho
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
            rigidbody.MovePosition(newPosition);

            // Verifica se o inimigo alcançou o nó atual e atualiza o índice para o próximo nó
            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                Debug.Log("Alcançou o nó " + targetIndex);
                targetIndex++;
                if (targetIndex >= path.Count)
                {
                    Debug.Log("Caminho concluído!");
                    path = null;
                }
            }

            // Calcular a direção do movimento
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
            anim.SetBool("movendo", true);
        }
        else
        {
            // Para a animação se não houver jogador ou caminho
            anim.SetBool("movendo", false);
        }
    }
}
