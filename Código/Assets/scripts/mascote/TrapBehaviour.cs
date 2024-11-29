using UnityEngine;

public class TrapBehavior : MonoBehaviour
{
    public Sprite activatedSprite; // Sprite para mostrar a armadilha ativada
    public int damage = 20; // Dano causado pela armadilha
    public float slowEffect = 0.5f; // Percentual de redução de velocidade
    public float slowDuration = 2f; // Duração do efeito de lentidão
    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer
    private bool isActivated = false; // Garante que a armadilha só ative uma vez

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o SpriteRenderer
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou é um inimigo
        if (!isActivated && collision.CompareTag("Inimigo"))
        {
            isActivated = true; // Marca a armadilha como ativada

            // Troca o sprite para o sprite ativado
            if (spriteRenderer != null && activatedSprite != null)
            {
                spriteRenderer.sprite = activatedSprite;
            }

            // Aplica dano ao inimigo
            VidaInimigo enemyHealth = collision.GetComponent<VidaInimigo>();
            if (enemyHealth != null)
            {
                enemyHealth.PerderVida(damage); // Reduz a vida do inimigo
            }

            // Reduz a velocidade do inimigo
            InimigoAtualizado enemyMovement = collision.GetComponent<InimigoAtualizado>();
            if (enemyMovement != null)
            {
                StartCoroutine(SlowEnemy(enemyMovement)); // Aplica lentidão
            }

            // Destroi a armadilha após um tempo
            Destroy(gameObject, 1f); // Dê tempo para ver a animação
        }
    }

    // Coroutine para aplicar lentidão temporária
    private System.Collections.IEnumerator SlowEnemy(InimigoAtualizado inimigo)
    {
        float originalSpeed = inimigo.speed; // Salva a velocidade original
        inimigo.speed *= slowEffect; // Aplica a redução de velocidade

        yield return new WaitForSeconds(slowDuration); // Aguarda o tempo da lentidão

        inimigo.speed = originalSpeed; // Restaura a velocidade original
    }
}
