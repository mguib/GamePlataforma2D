using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    private AudioSource audioSouce;

    //Variáveis para cada audio de ação do player
    public AudioClip coinSound;
    public AudioClip jumpSoud;
    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSouce = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip sfx)
    {
        audioSouce.PlayOneShot(sfx);
    }
}
