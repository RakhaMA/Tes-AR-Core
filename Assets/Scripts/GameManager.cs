using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverPanel;
    public bool isGameOver = false;

    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void GameOver(){
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
