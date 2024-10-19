using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacoes : MonoBehaviour
{
    public ParticleSystem dust;
    public Animator animator;
    public Rigidbody2D rigidbody2d;
    public SpriteRenderer spriteRenderer;

    private bool facingRight = true; // Variável para armazenar a direção atual

    // Update is called once per frame
    void Update()
    {
        Vector2 velocidade = this.rigidbody2d.velocity;
        if ((velocidade.x != 0) || (velocidade.y != 0))
        {
            this.animator.SetBool("correndodir", true);
        }
        else
        {
            this.animator.SetBool("correndodir", false);
        }

        if (velocidade.x > 0 && !facingRight)
        {
            FlipSprite(false);
        }
        else if (velocidade.x < 0 && facingRight)
        {
            FlipSprite(true);
        }
    }

    private void FlipSprite(bool flip)
    {
        facingRight = !flip;
        spriteRenderer.flipX = flip;
        dust.Play();
    }
}
