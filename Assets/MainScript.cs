﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainScript : MonoBehaviour
{
    public GameObject stage;
    public GameObject paddles;
    public GameObject ball;

    public float firstBlockSeconds;
    public float addBlockRate;
    public float blockSpaceRadius;
    public int blocksPerNewLevel;

    public GameObject normalBlock;
    public GameObject rockBlock;
    public GameObject ballBlock;
    public GameObject explodeBlock;

    public AudioSource blackFlowerSong;
    public AudioSource strikebeamSong;

    public GameObject gameUI;
    
    public GameObject startScreen;
    public GameObject pauseScreen;
    public GameObject winScreen;

    private GameObject _ui;
    
    private const int NumLevels = 4;

    private bool _paused;
    private int _level;
    private int _lives;

    private readonly float[] _levelUpTimes =
    {
        0f,
        17.14f,
        30.85f,
        44.57f,
        58.28f,
        72.0f,
        85.71f,
        99.42f,
        113.14f,
        126.85f,
        140.57f,
        154.28f,
    };

    void Start()
    {
        Instantiate(stage);
        Instantiate(paddles);
        Instantiate(ball);

        _ui = Instantiate(gameUI);

        _lives = 3;
        _level = 0;

        UpdateUI();

        InvokeRepeating("AddRandomBlock", firstBlockSeconds, addBlockRate);

        Time.timeScale = 0f;
        _paused = true;
        Instantiate(startScreen);
        strikebeamSong.Play();
        strikebeamSong.Pause();
        blackFlowerSong.Play();
    }

    private void UpdateUI()
    {
        Text levelText = _ui.transform.Find("LevelText").GetComponent<Text>();
        Text livesText = _ui.transform.Find("LivesText").GetComponent<Text>();
        levelText.text = $"Level {_level}";
        livesText.text = $"{_lives} Lives";
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause();
        }

        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
        {
            LoseLife();
        }

        if (strikebeamSong.time > _levelUpTimes[_level])
        {
            ++_level;
            UpdateUI();
            if (_level > NumLevels)
            {
                Time.timeScale = 0f;
                _paused = true;
                Instantiate(winScreen);
            }

            OnStrikebeamCheckpoint();
        }
    }

    private void LoseLife()
    {
        --_lives;
        UpdateUI();
        if (_lives > 0)
        {
            Instantiate(ball);
            Pause();
        }
        else
        {
            RestartGame();
        }
    }

    private void OnStrikebeamCheckpoint()
    {
        for (int i = 0; i < blocksPerNewLevel; i++)
        {
            AddBlockCorrespondingToLevel(_level);
        }
    }

    private void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Pause();
    }

    private void TogglePause()
    {
        if (_paused)
        {
            Unpause();
            return;
        }

        Pause();
    }

    private void Pause()
    {
        strikebeamSong.Pause();
        Time.timeScale = 0f;
        _paused = true;
        Instantiate(pauseScreen);
    }

    private void Unpause()
    {
        blackFlowerSong.Stop();
        strikebeamSong.UnPause();
        Time.timeScale = 1f;
        _paused = false;
        Destroy(GameObject.FindWithTag("PauseScreen"));
    }

    private void AddRandomBlock()
    {
        var blockLevel = Random.Range(1, _level + 1); // upper bound is exclusive
        AddBlockCorrespondingToLevel(blockLevel);
    }

    private void AddBlockCorrespondingToLevel(int level)
    {
        switch (level)
        {
            case 1:
                InstantiateBlock(normalBlock);
                break;
            case 2:
                InstantiateBlock(rockBlock);
                break;
            case 3:
                InstantiateBlock(ballBlock);
                break;
            case 4:
                InstantiateBlock(explodeBlock);
                break;
            default:
                Debug.Log("No block corresponding to level " + level);
                break;
        }
    }

    private void InstantiateBlock(GameObject block)
    {
        // TODO: don't instantiate on top of other blocks or balls
        var randomUnitCirclePosition = Random.insideUnitCircle;
        var startPosition = new Vector3(
            blockSpaceRadius * randomUnitCirclePosition.x,
            block.transform.position.y,
            blockSpaceRadius * randomUnitCirclePosition.y
        );
        Instantiate(block, startPosition, Quaternion.identity);
    }
}