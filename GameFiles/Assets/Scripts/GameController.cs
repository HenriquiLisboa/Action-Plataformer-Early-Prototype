using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int CoinPurse;
    public int StoredMoney;
    public Text MoneyText;
    public Text lifeText;
    public GameObject InventoryScreen;
    public GameObject PauseScreen;
    public GameObject GameOverScreen;
    public static GameController instance;
    private bool isPaused;
    private bool isMenu;
    private bool isGameOver;

    //Awake é chamado antes de todos os métodos Start
    void Awake()
    {
        instance = this;
        StoredMoney = PlayerPrefs.GetInt("Current Money");
    }

    void Update()
    {
        PauseMenu();
        InventoryMenu();
    }

    public void UpdateMoney(int value)
    {
        CoinPurse += value;
        MoneyText.text = "x " + CoinPurse.ToString();

        PlayerPrefs.SetInt("Current Money", CoinPurse + StoredMoney);
    }

    public void UpdateLife(int value)
    {
        lifeText.text = "x " + value.ToString();
    }

    public void PauseMenu()
    {
        if(Input.GetButtonDown("Pause") && isMenu == false && isGameOver == false)
        {
            isPaused = !isPaused;
            PauseScreen.SetActive(isPaused);
        }

        if(isPaused && isMenu == false && isGameOver == false)
        {
            Time.timeScale = 0f;
        }
        else
        {
            if(!isPaused && !isMenu && !isGameOver)
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void InventoryMenu()
    {
        if(Input.GetButtonDown("Menu") && isPaused == false && isGameOver == false)
        {
            isMenu = !isMenu;
            InventoryScreen.SetActive(isMenu);
        }

        if(isMenu && isPaused == false && isGameOver == false)
        {
            Time.timeScale = 0f;
        }
        else
        {
            if(!isPaused && !isMenu && !isGameOver)
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;   
        GameOverScreen.SetActive(true);

        if(isGameOver && isPaused == false && isMenu == false)
        {
            Time.timeScale = 0f;
        }
        else
        {
            if(!isPaused && !isMenu && !isGameOver)
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
