using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonHoverAudio : MonoBehaviour
{
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void OnPointerEnter()
    {
        audioSource.Play();
    }
}
