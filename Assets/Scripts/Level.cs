using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
        FindObjectOfType<GameSession>().ResetGame();
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(GameOverAfterSec());
    }

    IEnumerator GameOverAfterSec()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Game Over");
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

}
