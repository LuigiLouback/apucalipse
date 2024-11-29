using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
   
   [SerializeField] AudioSource musicSource;
   [SerializeField] AudioSource SFXSource;

 
   public AudioClip background;
   public AudioClip Hubbackground;
   public AudioClip audiotiro;
   public AudioClip audiozombie;

   private void Start(){
    musicSource.clip=background;
    musicSource.Play();
   }
   public void PlaySFX(AudioClip clip){
      SFXSource.PlayOneShot(clip);
   }
}
