using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float velocidadeMovimento;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Inicializando o script");
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
