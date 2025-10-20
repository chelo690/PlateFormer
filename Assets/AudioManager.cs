using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Fuente de Audio")] // Me gustar�a decirte que entiendo esto pero te estar�a chimbeando porque no, no lo s�. Estoy viendo un tutorial y ya
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Clip Audio")] // Asumo que uno habr� de ser el mezclador y el otro el que controla los propios audios
    public AudioClip LifeUp;
    public AudioClip Break;
    public AudioClip Coin;
    public AudioClip Jump;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}