using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SistemaVida : MonoBehaviour
{
    public float vida;
    public float vidaMax;

    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        vidaMax = vida;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(vida / vidaMax, 0, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            Debug.Log("oi");
            PerderVida(10); // Ajuste o valor de dano conforme necessário
        }
    }

    void PerderVida(float dano)
    {
        vida -= dano;

        // Certifique-se de que a vida não fique negativa
        if (vida <= 0)
        {
            vida = 0;
        }
    }
}
