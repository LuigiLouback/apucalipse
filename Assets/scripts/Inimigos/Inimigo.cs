using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject player;
    private Animator anim;
    private Rigidbody2D rigidbody; // Declare o Rigidbody2D
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>(); // Inicialize o Rigidbody2D
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Move o inimigo em direção ao jogador
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.position = targetPosition;

            // Calcular a direção do movimento
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Verificar se o inimigo está se movendo
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
            // Parar a animação se não houver jogador
            anim.SetBool("movendo", false);
        }
    }
}
