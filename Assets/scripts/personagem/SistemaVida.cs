using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class SistemaVida : MonoBehaviour
{
    public float vida;
    public float vidaMax;
   
 [SerializeField] private ParticleSystem sangue;
    // Referências ao Slider e TextMeshProUGUI
    public Slider healthSlider;
    public TextMeshProUGUI vidaTexto; // TextMeshPro para exibir a vida

    // Start is called before the first frame update
    void Start()
    {
        vidaMax = vida;
        AtualizarSlider();
    }

    // Update is called once per frame
    void Update()
    {
        AtualizarSlider();
    }

    public void PerderVida(float dano)

    {
        sangue.Play();
        vida -= dano;

        // Certifique-se de que a vida não fique negativa
        if (vida <= 0)
        {
            vida = 0;
            GameOver();
        }

        AtualizarSlider();
    }

    void AtualizarSlider()
    {
        // Atualiza o valor do slider e o texto de vida
        healthSlider.value = Mathf.Clamp(vida / vidaMax, 0, 1);
        vidaTexto.text = $"{vida} / {vidaMax}";
    }

    void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("GameOver");
    }
}


