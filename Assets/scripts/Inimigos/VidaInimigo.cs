using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaInimigo : MonoBehaviour
{
    public float vidaInimigo; // Vida do inimigo
    public int xpRecompensa; // XP que o jogador ganha ao matar o inimigo

    // Função para aplicar dano ao inimigo
    public void PerderVida(float dano)
    {
        vidaInimigo -= dano;

        // Verifica se a vida do inimigo chegou a zero ou abaixo
        if (vidaInimigo <= 0)
        {
            Morrer();
        }
    }

    // Função chamada quando o inimigo morre
    void Morrer()
    {
        // Dá XP para o jogador ao morrer
        SistemaNivel player = FindObjectOfType<SistemaNivel>();
        if (player != null)
        {
            player.GanharXP(xpRecompensa);
        }

        // Destrói o inimigo
        Destroy(gameObject);
    }

    // Detecta a colisão com o objeto "Bullet"
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bala bullet = collision.gameObject.GetComponent<Bala>();
            if (bullet != null)
            {
                PerderVida(bullet.dano);
            }
        }
    }
}
