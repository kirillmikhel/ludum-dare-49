using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isPause;
    public bool isVictory;
    public bool isGameOver;
    public GameObject victoryGO;
    public GameObject gameOverGO;
    public GameObject titleGO;
    public string gameOverMessage;
    public static GameManager Instance;
    public int playerDistance;
    public int enemyDistance;
    public Slider playerDistanceSlider;
    public Slider enemyDistanceSlider;
    public int enemyStep = 1;
    public float enemyStepDuration = 1.0f;
    public PlayRandomSound randomIceCrackingSound;
    public AudioSource breathingSound;
    public AudioSource victorySound;
    public AudioSource gameOverSound;
    public AudioSource iceDrowningSound;

    private void Awake()
    {
        Instance = this;
    }

    public void Win()
    {
        isVictory = true;
        isPause = true;
        
        victoryGO.SetActive(true);
        victorySound.Play();
    }

    public void GameOver(string message)
    {
        gameOverMessage = message;
        isGameOver = false;
        isPause = true;

        gameOverGO.SetActive(true);
        gameOverGO.GetComponentInChildren<Text>().text = message;
        gameOverSound.Play();
        breathingSound.Stop();
    }

    public void PlayRandomIceCrackSound()
    {
        randomIceCrackingSound.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveEnemy());
    }

    private IEnumerator MoveEnemy()
    {
        while (true)
        {
            if (!isPause)
            {
                enemyDistance += enemyStep;

                if (enemyDistance >= playerDistance)
                {
                    GameOver("The hunter got you!");
                }
            }

            yield return new WaitForSeconds(enemyStepDuration);   
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerDistanceSlider.value = playerDistance;
        enemyDistanceSlider.value = enemyDistance;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }

    public void StartTheGame()
    {
        titleGO.SetActive(false);
        isPause = false;
    }
}
