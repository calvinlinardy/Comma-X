﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //config params
    [Header("Enemy Stats")]
    [SerializeField] int health = 100;
    [SerializeField] int scoreValue = 200;

    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;

    [Header("Sound FX")]
    [SerializeField] GameObject deathVFX = null;
    [SerializeField] GameObject hitVFX = null;
    [SerializeField] float durationOfExplosion = 0.8f;
    [SerializeField] AudioClip deathSFX = null;
    [SerializeField] AudioClip hitSFX = null;
    [SerializeField] [Range(0, 1)] float SFXVolume = 0.7f;

    [Header("Enemy Projectile")]
    [SerializeField] GameObject projectile = null;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] AudioClip shootSFX = null;
    [SerializeField] [Range(0, 1)] float shootSFXVolume = 0.25f;

    //cached reference
    GameSession gameSession;
    Level level;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        level = FindObjectOfType<Level>();
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
        pointReached();
    }

    private void pointReached()
    {
        int currentScore = gameSession.GetScore();
        int scoreToWin = level.GetScoreToWin();
        if (currentScore >= scoreToWin)
        {
            Destroy(gameObject);
            GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
            Destroy(explosion, durationOfExplosion);
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, 0.1f);
            Destroy(FindObjectOfType<EnemySpawner>().gameObject);
            level.LoadGameOver();
        }
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject enemyLaser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (health > 1)
        {
            GameObject hits = Instantiate(hitVFX, transform.position, transform.rotation);
            Destroy(hits, 0.1f);
            AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position, SFXVolume);
        }
        if (!damageDealer)
        {
            return;
        }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameSession.AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, SFXVolume);
    }
}
