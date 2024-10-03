using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaTiro : MonoBehaviour
{
    private Vector2 mousePosi;
    private Vector2 dirArma;
    private float angle;
    [SerializeField] private SpriteRenderer srGun; // SpriteRenderer para a arma
    [SerializeField] private float tempoEntreTiros;
    private bool podeAtirar = true;
    [SerializeField] private Transform pontoDeFogo;
    [SerializeField] private GameObject tiro;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0) && podeAtirar)
        {
            podeAtirar = false;
            Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
            Invoke("CDTiro", tempoEntreTiros);
        }
    }
   
    private void FixedUpdate()
    {
        dirArma = mousePosi - (Vector2)transform.position;
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg;

        // Verifica se o cursor está à direita ou à esquerda do objeto
        if (mousePosi.x > transform.position.x)
        {
            srGun.flipX = false;  // Se o cursor está à direita, mantém a sprite normal
            transform.rotation = Quaternion.Euler(0, 0, angle);  // Rotação normal
            pontoDeFogo.localPosition = new Vector2(Mathf.Abs(pontoDeFogo.localPosition.x), pontoDeFogo.localPosition.y); // Mantém o ponto de fogo na posição correta
            pontoDeFogo.localRotation = Quaternion.Euler(0, 0, 270); // Mantém a rotação normal do ponto de fogo
        }
        else
        {
            srGun.flipX = true;   // Se o cursor está à esquerda, inverte a sprite
            transform.rotation = Quaternion.Euler(0, 0, angle + 180f);  // Ajusta a rotação ao inverter
            pontoDeFogo.localPosition = new Vector2(-Mathf.Abs(pontoDeFogo.localPosition.x), pontoDeFogo.localPosition.y); // Inverte a posição do ponto de fogo
            pontoDeFogo.localRotation = Quaternion.Euler(0, 0, 90f); // Ajusta a rotação do ponto de fogo
        }
    }

    private void CDTiro()
    {
        podeAtirar = true;
    }
}
