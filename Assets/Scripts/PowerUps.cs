using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public GameObject powerVFX = null;
    public AudioClip powerSFX = null;
    [Range(0, 1)] public float SFXVolume = 0.25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        if (collision.tag == "Player")
        {
            GameObject powerEffect = Instantiate(powerVFX, collision.transform.position, transform.rotation) as GameObject;
            Destroy(powerEffect, 0.2f);
            AudioSource.PlayClipAtPoint(powerSFX, Camera.main.transform.position, SFXVolume);
        }
    }
}
