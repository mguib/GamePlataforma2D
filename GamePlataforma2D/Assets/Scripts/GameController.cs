using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int score;
    public Text scoreText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Time.timeScale = 1;            

        if(PlayerPrefs.GetInt("score") != null)
        {
            score = PlayerPrefs.GetInt("score");
            scoreText.text = "x " + score.ToString();
        }
        //PlayerPrefs.DeleteKey("score");   //Apagando os dados do PlayerPreferens
    }

    // Start is called before the first frame update 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCoin()
    {
        score++;
        scoreText.text = "x " +score.ToString();

        PlayerPrefs.SetInt("score", score);
    }
    
    public void ShowGameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
