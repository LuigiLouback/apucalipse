using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour
{ 
    public Rigidbody2D rigidbody;
    public float velocidadeMovimento;
    public static Transform PlayerTransform { get; private set; }

    private void Awake()
    {
        // Garante que apenas um objeto PlayerManager seja carregado
        if (PlayerTransform == null)
        {
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 direcao = new Vector2(horizontal,vertical);
        direcao = direcao.normalized;
        this.rigidbody.velocity = direcao*this.velocidadeMovimento;
    }

}
