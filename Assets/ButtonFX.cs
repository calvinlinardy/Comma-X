using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    public AudioSource myFX;
    public AudioClip hoverFX;
    public AudioClip clickFX;
    [SerializeField] [Range(0, 1)] float SFXVolume = 0.7f;

    public void HoverSound()
    {
        myFX.PlayOneShot(hoverFX, SFXVolume);
    }

    public void ClickSound()
    {
        myFX.PlayOneShot(clickFX, SFXVolume);
    }
}
