using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //configuration parameters
    [Header("Player")]
    [SerializeField] float padding = 1f;
    public int health = 20;
    [SerializeField] Image[] hearts = null;

    [Header("SoundFX")]
    [SerializeField] AudioClip deathSFX = null;
    [SerializeField] AudioClip hitSFX = null;
    [SerializeField] AudioClip teleportOutSFX = null;
    [SerializeField] AudioClip teleportInSFX = null;
    [SerializeField] [Range(0, 1)] float SFXVolume = 0.7f;

    [Header("VFX")]
    [SerializeField] GameObject deathVFX = null;
    [SerializeField] GameObject hitVFX = null;
    [SerializeField] GameObject teleportVFX = null;
    [SerializeField] GameObject teleportInVFX = null;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab = null;
    [SerializeField] float projectileSpeed = 20f;
    public float projectileFiringPeriod = 0.1f;
    [SerializeField] AudioClip shootSFX = null;
    [SerializeField] [Range(0, 1)] float shootSFXVolume = 0.25f;

    //cached reference
    Level level;
    PowerUpSpawner powerUpSpawner;
    GameSession gameSession;
    SpriteRenderer mySprite;
    PolygonCollider2D myCollider;

    //state
    Coroutine firingCoroutine;
    Coroutine movingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    void Start()
    {
        MyCached();
        SetUpMoveBoundaries();
        PowerUps();
    }

    private void MyCached()
    {
        gameSession = FindObjectOfType<GameSession>();
        powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
        level = FindObjectOfType<Level>();
        mySprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<PolygonCollider2D>();
    }

    private void PowerUps()
    {
        InvokeRepeating("SpawnMeds", 0.1f, 6f);
        InvokeRepeating("SpawnPlane", 0.1f, 2f);
        InvokeRepeating("SpawnBigMeds", 0.1f, 15f);
    }

    void Update()
    {
        TeleportIn();
        Move();
        Fire();
        HeartsMinus();
    }

    private void SpawnMeds()
    {
        if (health < 20)
        {
            powerUpSpawner.Meds();
        }
    }
    private void SpawnPlane()
    {
        if (gameSession.GetScore() >= 0)
        {
            powerUpSpawner.Plane();
        }
    }

    private void SpawnBigMeds()
    {
        if (health < 15)
        {
            powerUpSpawner.BigMeds();
        }
    }

    private void HeartsMinus()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    IEnumerator FiringPeriodBoost()
    {
        projectileFiringPeriod = 0.05f;
        yield return new WaitForSeconds(3f);
        projectileFiringPeriod = 0.2f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (other.tag == "Meds")
        {
            if (health < 19)
            {
                health += 2;
            }
            else
            {
                health += 1;
            }
        }
        if (other.tag == "BigMeds")
        {
            if (health <= 10)
            {
                health += 10;
            }
            else
            {
                health = 20;
            }
        }
        if (other.tag == "Plane")
        {
            StartCoroutine(FiringPeriodBoost());
        }
        if (!damageDealer)
        {
            return;
        }
        if (health > 1)
        {
            GameObject hits = Instantiate(hitVFX, transform.position, transform.rotation);
            Destroy(hits, 0.1f);
            AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position, SFXVolume);
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
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = false;
        }
        level.LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, 0.58f);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, SFXVolume);
    }

    private void Move()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Appear();
            movingCoroutine = StartCoroutine(PlayerMovement());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(movingCoroutine);
        }
    }

    private void Appear()
    {
        mySprite.enabled = true;
        myCollider.enabled = true;
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject teleportSwish = Instantiate(teleportVFX, cursorPos, Quaternion.identity);
        Destroy(teleportSwish, 0.4f);
        AudioSource.PlayClipAtPoint(teleportOutSFX, Camera.main.transform.position, SFXVolume);
    }

    private void TeleportIn()
    {
        if (health > 1)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                if (mySprite.enabled == true && myCollider.enabled == true)
                {
                    StartCoroutine(Disappear());
                }
            }
        }
    }

    IEnumerator Disappear()
    {
        mySprite.enabled = false;
        myCollider.enabled = false;
        GameObject teleportIn = Instantiate(teleportInVFX, transform.position, Quaternion.identity);
        Destroy(teleportIn, 0.4f);
        AudioSource.PlayClipAtPoint(teleportInSFX, Camera.main.transform.position, SFXVolume);
        health--;
        yield return new WaitForSeconds(5f);
        AppearAfterDisappear();
    }

    private void AppearAfterDisappear()
    {
        mySprite.enabled = true;
        myCollider.enabled = true;
        if (health > 10)
        {
            health -= 10;
        }
        else
        {
            Die();
        }
        GameObject teleportSwish = Instantiate(teleportVFX, transform.position, Quaternion.identity);
        Destroy(teleportSwish, 0.4f);
        AudioSource.PlayClipAtPoint(teleportOutSFX, Camera.main.transform.position, SFXVolume);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }
    IEnumerator PlayerMovement()
    {
        while (true)
        {
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerPos.x = Mathf.Clamp(cursorPos.x, xMin, xMax);
            playerPos.y = Mathf.Clamp(cursorPos.y, yMin, yMax);
            transform.position = playerPos;
            yield return new WaitForSeconds(0.01f);
        }
        /*
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);*/
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
