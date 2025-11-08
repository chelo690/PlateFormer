using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Fuente de Audio")] // Me gustaría decirte que entiendo esto pero te estaría chimbeando porque no, no lo sé. Estoy viendo un tutorial y ya
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Clip Audio")] // Asumo que uno habrá de ser el mezclador y el otro el que controla los propios audios
    public AudioClip LifeUp;
    public AudioClip EnemyBreak;
    public AudioClip Coin;
    public AudioClip Jump;
    public AudioClip Arepickup;
    public AudioClip Checkpoint;
    public AudioClip Damage;
    public AudioClip DeathFem;
    public AudioClip DeathMasc;
    public AudioClip Receta;
    public AudioClip Fall;
    public AudioClip Finish;
    public AudioClip Grab;
    public AudioClip MenuConfirm;
    public AudioClip MenuSelect;
    public AudioClip Stomp;
    public AudioClip Throw;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}