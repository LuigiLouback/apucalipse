using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacaoAtaque : MonoBehaviour
{
    // Tag que identifica os inimigos
    public string enemyTag = "Inimigo";

    // Método chamado quando um Collider entra na área de Trigger
    private void OnTriggerEnter2D(Collider2D other) // Certifique-se de usar Collider2D para jogos 2D
    {
        // Verifica se o objeto que entrou é um inimigo
        if (other.CompareTag(enemyTag))
        {
            Animator enemyAnimator = other.GetComponent<Animator>();
            if (enemyAnimator != null)
            {
                Debug.Log("Inimigo entrou: " + other.gameObject.name);

                // Ativa a animação de ataque
                enemyAnimator.SetBool("IsAttacking", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // Certifique-se de usar Collider2D para jogos 2D
    {
        // Verifica se o objeto que saiu é um inimigo
        if (other.CompareTag(enemyTag))
        {
            Animator enemyAnimator = other.GetComponent<Animator>();
            if (enemyAnimator != null)
            {
                Debug.Log("Inimigo saiu: " + other.gameObject.name);

                // Desativa a animação de ataque imediatamente quando o inimigo sai da área
                enemyAnimator.SetBool("IsAttacking", false);
            }
        }
    }
}
