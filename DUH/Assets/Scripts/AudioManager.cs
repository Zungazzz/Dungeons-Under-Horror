using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource SFXAudioSource;

    public AudioClip backgroundMusic;
    public AudioClip Walking;
    public AudioClip LaserBeam;
    public AudioClip BowShooting;
    public AudioClip Summon;
    public AudioClip Fire;
    public AudioClip Damage;
    public AudioClip CoinCollect;
    public AudioClip HealthCollect;
    public AudioClip Death;
    public AudioClip LevelUp;
    public AudioClip Dash;
    public AudioClip Explosion;
    public AudioClip Sword;
    private void Start()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXAudioSource.PlayOneShot(clip);
    }
}
