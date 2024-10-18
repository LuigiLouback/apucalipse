using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SistemaNivel : MonoBehaviour
{
    public int nivelAtual = 1;
    public int xpAtual = 0;
    public int xpParaProximoNivel = 100;
    public float aumentoVida = 10f;
    public float aumentoVelocidade = 1f;

    public Slider nivelSlider;
    public TextMeshProUGUI nivelText;

    public Jogador jogador;
    public SistemaVida sistemaVida;

    public GameObject escolhaNivelUI;
    public Button aumentarVidaButton;
    public Button aumentarVelocidadeButton;

    void Start()
    {
        escolhaNivelUI.SetActive(false);
        AtualizarUI();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            GanharXP(20);
        }
    }

    public void GanharXP(int xp)
    {
        xpAtual += xp;

        if (xpAtual >= xpParaProximoNivel)
        {
            SubirDeNivel();
        }

        AtualizarUI();
    }

    void SubirDeNivel()
    {
        nivelAtual++;
        xpAtual = 0;
        xpParaProximoNivel += 50; // Aumenta a quantidade de XP necessária a cada nível
        escolhaNivelUI.SetActive(true); // Mostrar a UI de escolha de nível

        // Pausar o jogo
        Time.timeScale = 0;

        // Conectar os botões de escolha
        aumentarVidaButton.onClick.RemoveAllListeners();
        aumentarVidaButton.onClick.AddListener(AumentarVida);

        aumentarVelocidadeButton.onClick.RemoveAllListeners();
        aumentarVelocidadeButton.onClick.AddListener(AumentarVelocidade);
    }

    void AumentarVida()
    {
        sistemaVida.vidaMax += aumentoVida;
        sistemaVida.vida += aumentoVida; // Recupera a vida total ao aumentar a vida máxima
        escolhaNivelUI.SetActive(false); // Esconder a UI de escolha de nível
        Time.timeScale = 1;
    }

    void AumentarVelocidade()
    {
        jogador.velocidadeMovimento += aumentoVelocidade;
        escolhaNivelUI.SetActive(false); // Esconder a UI de escolha de nível
        Time.timeScale = 1;
    }

    void AtualizarUI()
    {
        nivelText.text = $"{xpAtual} / {xpParaProximoNivel} - lvl: {nivelAtual}";
        nivelSlider.value = (float)xpAtual / xpParaProximoNivel;
    }
}
