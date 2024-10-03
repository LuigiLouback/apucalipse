using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animacoes : MonoBehaviour
{
    
    public Animator animator;
    public Rigidbody2D rigidbody2d;
    public SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update()
    {
       Vector2 velocidade = this.rigidbody2d.velocity;
       if((velocidade.x != 0)||(velocidade.y != 0)){
        this.animator.SetBool("correndodir",true);
       }else{
        this.animator.SetBool("correndodir",false);
       }
       if(velocidade.x > 0){
        this.spriteRenderer.flipX=false;
       }else if(velocidade.x < 0){
        this.spriteRenderer.flipX=true;
       }
    }
}
