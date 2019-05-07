using System;
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

    public AudioSource blackFlowerSong;
    public AudioSource strikebeamSong;

    public GameObject gameUI;

    public GameObject startScreen;
    public GameObject pauseScreen;
    public GameObject winScreen;

    private GameObject _ui;

    private const int NumLevels = 11;

    private bool _paused;
    private int _level;
    private int _lives;


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

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            strikebeamSong.time = _levelUpTimes[_level];
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            strikebeamSong.time = _levelUpTimes[_level - 2];
            --_level;
            UpdateUI();
            OnStrikebeamCheckpoint();
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

        var lightMultiplier = Mathf.Clamp(
            1 - strikebeamSong.time / strikebeamSong.clip.length,
            0f, 1f);
        mainLight.intensity = initialMainLightIntensity * lightMultiplier;
        mainLight.color = Color.Lerp(eventualMainLightColor, initialMainLightColor, lightMultiplier);
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