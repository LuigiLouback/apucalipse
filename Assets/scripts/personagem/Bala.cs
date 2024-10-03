using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
   [SerializeField] private float speed;
  [SerializeField] private float tempoDeVida;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,tempoDeVida);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(transform.up*speed*Time.fixedDeltaTime,Space.World);
    }
   
   public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Inimigo")){
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }
    }
}
