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
    public float firstBlockSeconds;
    public float addBlockRate;
    public float blockSpaceRadius;
    public int blocksPerNewLevel;

    public Light mainLight;
    public float initialMainLightIntensity;
    public Color initialMainLightColor;
    public Color eventualMainLightColor;

    public GameObject ball;

    public GameObject normalBlock;
    public GameObject rockBlock;
    public GameObject ballBlock;
    public GameObject explodeBlock;
    public GameObject magnetBlock;
    public GameObject factoryBlock;
    public GameObject squareBlock;
    public GameObject fireBlock;
    public GameObject iceBlock;
    public GameObject questionBlock;
    public GameObject glassGrenadeBlock;

    public AudioSource strikebeamSong;

    public GameObject gameUI;

    public GameObject pauseScreen;
    public GameObject winScreen;
    public GameObject gameOverScreen;
    public string menuSceneName;

    private GameObject _ui;

    private const int NumLevels = 11;

    private bool _paused;
    private int _level;
    private int _lives;

    private bool _gameOver;


    private readonly float[] _levelUpTimes =
    {
        0f, // bass
        17.14f, // drums
        30.85f, // bing bing
        44.57f, // synth
        58.28f, // metal
        72.0f, // choir
        85.71f, // bing bing & synth
        99.42f, // metal 2
        113.14f, // energy charge
        126.85f, // electric guitar
        140.57f, // electric guitar & synth
        154.28f, // end
    };

    void Start()
    {
        _ui = Instantiate(gameUI);

        _lives = 3;
        _level = 0;
        _gameOver = false;

        UpdateUI();

        InvokeRepeating("AddRandomBlock", firstBlockSeconds, addBlockRate);

        strikebeamSong.Play();
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
        KeyUpHandlers();

        if (_gameOver)
        {
            return;
        }

        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
        {
            RespawnOrDie();
        }

        if (strikebeamSong.time > _levelUpTimes[_level])
        {
            OnStrikebeamCheckpoint();
        }

        var lightMultiplier = Mathf.Clamp(
            1 - strikebeamSong.time / _levelUpTimes[NumLevels],
            0f, 1f);
        mainLight.intensity = initialMainLightIntensity * lightMultiplier;
        mainLight.color = Color.Lerp(eventualMainLightColor, initialMainLightColor, lightMultiplier);
    }

    private void KeyUpHandlers()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            SkipToLevel(_level + 1);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            SkipToLevel(_level - 1);
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SkipToLevel(1);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SkipToLevel(2);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SkipToLevel(3);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SkipToLevel(4);
        }

        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            SkipToLevel(5);
        }

        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            SkipToLevel(6);
        }

        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            SkipToLevel(7);
        }

        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            SkipToLevel(8);
        }

        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            SkipToLevel(9);
        }

        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            SkipToLevel(10);
        }

        if (Input.GetKeyUp(KeyCode.Delete))
        {
            foreach (var ball in GameObject.FindGameObjectsWithTag("Ball"))
            {
                Destroy(ball);
            }
        }
    }

    private void SkipToLevel(int level)
    {
        // go to the instance just before a level-up
        // the next frame, the game levels up to the desired level.
        _level = level - 1;
        strikebeamSong.time = _levelUpTimes[level - 1];
    }

    private void RespawnOrDie()
    {
        if (_lives > 0)
        {
            --_lives;
            UpdateUI();
            Pause();
            Instantiate(ball);
        }
        else if (!_gameOver)
        {
            Pause(gameOverScreen, true);
            _gameOver = true;
        }
    }

    private void OnStrikebeamCheckpoint()
    {
        ++_level;
        UpdateUI();
        for (int i = 0; i < blocksPerNewLevel; i++)
        {
            AddBlockCorrespondingToLevel(_level);
        }

        if (_level > NumLevels)
        {
            Pause(winScreen, false);
            _gameOver = true;
        }
    }

    private void LoadMenuScene()
    {
        SceneManager.LoadScene(menuSceneName);
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
        Pause(pauseScreen, true);
    }

    private void Pause(GameObject pauseScreenType, bool pauseMusic)
    {
        if (pauseMusic)
        {
            strikebeamSong.Pause();
        }

        Time.timeScale = 0f;
        _paused = true;
        Instantiate(pauseScreenType);
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
        _paused = false;

        if (_gameOver)
        {
            LoadMenuScene();
            return;
        }

        strikebeamSong.UnPause();
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
                InstantiateBlock(magnetBlock);
                break;
            case 5:
                InstantiateBlock(factoryBlock);
                break;
            case 6:
                InstantiateBlock(fireBlock);
                break;
            case 7:
                InstantiateBlock(iceBlock);
                break;
            case 8:
                InstantiateBlock(questionBlock);
                break;
            case 9:
                InstantiateBlock(glassGrenadeBlock);
                break;
            case 10:
                InstantiateBlock(squareBlock);
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
        Instantiate(block, startPosition, Quaternion.identity, transform);
    }
}