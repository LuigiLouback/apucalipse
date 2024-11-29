using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueInimigo : MonoBehaviour
{
    public float dano; // Dano que o inimigo causa
    public float tempoEntreAtaques = 1f; // Tempo de espera entre os ataques
    private bool podeAtacar = true;
    

    // Função que é chamada quando o inimigo colide com outro objeto
    void OnCollisionStay2D(Collision2D collision)
    {
        // Verifica se o objeto que ele está colidindo é o jogador
        if (collision.gameObject.CompareTag("Player") && podeAtacar)
        {
            // Obtém a referência ao script de vida do jogador e aplica dano
            SistemaVida jogador = collision.gameObject.GetComponent<SistemaVida>();
            if (jogador != null)
            {
                jogador.PerderVida(dano);
               
                StartCoroutine(EsperarProximoAtaque());
            }
        }
    }

    // Coroutine para controlar o tempo entre os ataques
    IEnumerator EsperarProximoAtaque()
    {
        podeAtacar = false; // Bloqueia novos ataques
        yield return new WaitForSeconds(tempoEntreAtaques); // Aguarda pelo tempo entre ataques
        podeAtacar = true; // Permite novos ataques
    }
}
