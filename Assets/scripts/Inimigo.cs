using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [SerializeField]
private Transform alvo;
 [SerializeField]
 private float velocidadeMovimento;
  [SerializeField]
  private Rigidbody2D rigidbody;
 [SerializeField]
 private SpriteRenderer spriteRenderer;
  [SerializeField]
  private float distanciaMinima;
[SerializeField]
 private Animator animator;
    // Update is called once per frame
    void Update()
    {
       Vector2 posicaoAlvo= this.alvo.position;
       Vector2 posicaoAtual = this.transform.position;
       float distancia = Vector2.Distance(posicaoAtual,posicaoAlvo);
       if(distancia>= this.distanciaMinima){

       
       Vector2 direcao = posicaoAlvo - posicaoAtual;
       direcao = direcao.normalized;
       this.rigidbody.velocity = (this.velocidadeMovimento * direcao);
       if(this.rigidbody.velocity.x>0){
       
        this.spriteRenderer.flipX=false;
       }else if(this.rigidbody.velocity.x<0){
         
        this.spriteRenderer.flipX=true;
        
       }this.animator.SetBool("movendo",true);
       }else{
        this.rigidbody.velocity = Vector2.zero;
        this.animator.SetBool("movendo",false);
       }
    }
}
