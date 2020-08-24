using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;
    int healthBonus = 0;
    int healthBonusScore = 1000;

    private void Update()
    {
        if (score != 0 && score % healthBonusScore == 0)
        {
            healthBonus = 1;
            StartCoroutine(HealthBonusMinus());
        }
    }

    IEnumerator HealthBonusMinus()
    {
        yield return new WaitForSeconds(1);
        healthBonus -= 1;
    }

    public int GetHealthBonus()
    {
        return healthBonus;
    }

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
