using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoAtualizado : MonoBehaviour
{
    [SerializeField] float speed;
    GameObject player;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null){
            transform.position = Vector2.MoveTowards(transform.position,player.transform.position,speed*Time.deltaTime);
        }
    }
}
