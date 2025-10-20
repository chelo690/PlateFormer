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
    public AudioClip Break;
    public AudioClip Coin;
    public AudioClip Jump;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}