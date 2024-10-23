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

    public VidaInimigo vidaInimigoZ;
    public VidaInimigo vidaInimigoP;

    public Bala bala;

    public GameObject escolhaNivelUI;
    public Button aumentarVidaButton;
    public Button aumentarVelocidadeButton;
    public Button recuperarVidaButton;

    public Button aumentarGanhoXp;

    public Button aumentarDano;

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

        recuperarVidaButton.onClick.RemoveAllListeners();
        recuperarVidaButton.onClick.AddListener(RecuperarVida);

        aumentarGanhoXp.onClick.RemoveAllListeners();
        aumentarGanhoXp.onClick.AddListener(AumentarXp);

        aumentarDano.onClick.RemoveAllListeners();
        aumentarDano.onClick.AddListener(AumentarDano);
    }

    void AumentarVida()
    {
        sistemaVida.vidaMax += aumentoVida;
        sistemaVida.vida += aumentoVida;
        escolhaNivelUI.SetActive(false);
        Time.timeScale = 1;
    }

    void AumentarVelocidade()
    {
        jogador.velocidadeMovimento += aumentoVelocidade;
        escolhaNivelUI.SetActive(false);
        Time.timeScale = 1;
    }

    void RecuperarVida()
    {
        sistemaVida.vida = sistemaVida.vidaMax;
        escolhaNivelUI.SetActive(false);
        Time.timeScale = 1;
    }

    void AumentarXp()
    {
        vidaInimigoZ.xpRecompensa += 1;
        vidaInimigoP.xpRecompensa += 1;
        escolhaNivelUI.SetActive(false);
        Time.timeScale = 1;
    }

    void AumentarDano()
    {
        bala.dano += 5f;
        escolhaNivelUI.SetActive(false);
        Time.timeScale = 1;
    }

    void AtualizarUI()
    {
        nivelText.text = $"{xpAtual} / {xpParaProximoNivel} - lvl: {nivelAtual}";
        nivelSlider.value = (float)xpAtual / xpParaProximoNivel;
    }
}
