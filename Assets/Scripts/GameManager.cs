using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public bool isBossSpawned;
    [Header("# Player Info")]
    public int health;
    public int maxHealth;
    public int level;
    public int kill;
    public int exp;
    public int nextEXP;
    public bool maxLvl;

    public float magnetRange;
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public GameObject uiResult;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart()
    {
        health = maxHealth;
        uiLevelUp.Select(0);
        isLive = true;
    }
    public void GameOver()
    {
        StartCoroutine(GameoverRoutine());
    }
    IEnumerator GameoverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(1f);
        uiResult.SetActive(true);
        Stop();
    }
    public void GameRestart()
    {
        SceneManager.LoadScene(0);
        Resume();
    }

    void Update()
    {
        if (!isLive)
        {
            return;
        }
        gameTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)) {
            gameTime += 470;
        }
    }

    public void getExp(int earned)
    {
        exp += earned;

        if (exp >= nextEXP && level <= 37)
        {
            LvlUp();
        }
    }

    public void LvlUp()
    {
        level++;
        exp -= nextEXP;
        nextEXP += 15;
        uiLevelUp.Show();
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
