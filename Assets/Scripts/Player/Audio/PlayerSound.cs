using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip footFall;
    public AudioClip land;
    public AudioClip jump;
    public AudioClip swordSlash;
    public AudioClip kick;
    public AudioClip shootPowerUp;

    void FootL()
    {
        audioSource.clip = footFall;
        audioSource.Play();
    }

    void FootR()
    {
        audioSource.clip = footFall;
        audioSource.Play();
    }

    void Land()
    {
        audioSource.clip = land;
        audioSource.Play();
    }

    void Jump()
    {
        audioSource.clip = jump;
        audioSource.Play();
    }

    void SwordSlash()
    {
        audioSource.clip = swordSlash;
        audioSource.Play();
    }

    void Kick()
    {
        audioSource.clip = kick;
        audioSource.Play();
    }

    void ShootPowerUp()
    {
        audioSource.clip = shootPowerUp;
        audioSource.Play();
    }
}
