using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public int blockRingNumBlocks;
    public float blockRingRadius;

    public float emergencySlowMotionRadius;
    
    public GameObject ball;

    public GameObject normalBlock;
    public GameObject rockBlock;
    public GameObject ballBlock;
    public GameObject magnetBlock;
    public GameObject factoryBlock;
    public GameObject fireBlock;
    public GameObject iceBlock;
    public GameObject spinBlock;
    public GameObject glassGrenadeBlock;
    public GameObject squareBlock;
    public GameObject spinningSquareBlock;

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
    private int _score;

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
        _score = 0;

        UpdateUI();

        InvokeRepeating(nameof(InstantiateBlock), firstBlockSeconds, addBlockRate);

        strikebeamSong.Play();
    }

    private void UpdateUI()
    {
        // TODO: make these textboxes automatically update when the variables change
        Text levelText = _ui.transform.Find("LevelText").GetComponent<Text>();
        Text livesText = _ui.transform.Find("LivesText").GetComponent<Text>();
        Text scoreText = _ui.transform.Find("ScoreText").GetComponent<Text>();
        levelText.text = $"Level {_level}";
        livesText.text = $"{_lives} Lives";
        scoreText.text = $"{_score} Points";
    }

    void Update()
    {
        KeyUpHandlers();

        if (_gameOver)
        {
            return;
        }

        var ballsInPlay = GameObject.FindGameObjectsWithTag("Ball");

        if (ballsInPlay.Length == 0)
        {
            RespawnOrDie();
        }

        if (!_paused)
        {
            if (AboutToGameOver(ballsInPlay))
            {
                Time.timeScale = 0.5f;
                strikebeamSong.pitch = 0.5f;
            }
            else
            {
                Time.timeScale = 1f;
                strikebeamSong.pitch = 1f;
            }
        }

        if (strikebeamSong.time > _levelUpTimes[_level])
        {
            OnStrikebeamCheckpoint();
        }

        var lightMultiplier = GetDaylightIntensity();
        mainLight.intensity = initialMainLightIntensity * lightMultiplier;
        mainLight.color = Color.Lerp(eventualMainLightColor, initialMainLightColor, lightMultiplier);
    }

    private bool AboutToGameOver(GameObject[] ballsInPlay)
    {
        if (_lives > 0)
        {
            return false;
        }

        foreach (var ball in ballsInPlay)
        {
            if (ball.transform.position.magnitude < emergencySlowMotionRadius)
            {
                return false;
            }
        }

        return true;
    }

    public float GetDaylightIntensity()
    {
        return Mathf.Clamp(
            1 - strikebeamSong.time / _levelUpTimes[NumLevels],
            0f, 1f);
    }

    private void KeyUpHandlers()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Return))
        {
            TogglePause();
        }

        if (_paused)
        {
            if (Input.GetKeyUp(KeyCode.M))
            {
                LoadMenuScene();
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Application.Quit();
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                var ballsInPlay = GameObject.FindGameObjectsWithTag("Ball");
                foreach (var ball in ballsInPlay)
                {
                    var ballScript = ball.GetComponent<Ball>();
                    ballScript.SpeedUp(ballScript.maxSpeedAfterCollision);
                }
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                var ballsInPlay = GameObject.FindGameObjectsWithTag("Ball");
                foreach (var ball in ballsInPlay)
                {
                    var ballScript = ball.GetComponent<Ball>();
                    ballScript.SlowDown(ballScript.minSpeed);
                }
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

            if (Input.GetKeyUp(KeyCode.Minus))
            {
                SkipToLevel(11);
            }

            if (Input.GetKeyUp(KeyCode.Equals))
            {
                SkipToLevel(12);
            }

            if (Input.GetKeyUp(KeyCode.Delete))
            {
                foreach (var ball in GameObject.FindGameObjectsWithTag("Ball"))
                {
                    Destroy(ball);
                }
            }

            if (Input.GetKeyUp(KeyCode.Insert))
            {
                Instantiate(ball);
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

        if (_level > NumLevels)
        {
            Pause(winScreen, false);
            _gameOver = true;
            return;
        }

        for (int i = 0; i < blocksPerNewLevel; i++)
        {
            InstantiateBlock(GetBlockTypeByLevel(_level));
        }
    }

    private void LoadMenuScene()
    {
        Time.timeScale = 1f;
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
        if (_gameOver)
        {
            LoadMenuScene();
            return;
        }

        Time.timeScale = 1f;
        _paused = false;

        strikebeamSong.UnPause();
        Destroy(GameObject.FindWithTag("PauseScreen"));
    }

    private GameObject GetBlockTypeByLevel(int level)
    {
        switch (level)
        {
            case 1:
                return normalBlock;
            case 2:
                return spinBlock;
            case 3:
                return ballBlock;
            case 4:
                return magnetBlock;
            case 5:
                return rockBlock;
            case 6:
                return fireBlock;
            case 7:
                return iceBlock;
            case 8:
                return factoryBlock;
            case 9:
                return glassGrenadeBlock;
            case 10:
                return squareBlock;
            case 11:
                return spinningSquareBlock;
            default:
                Debug.LogError("No block corresponding to level " + level);
                return null;
        }
    }

    private Vector3 GetRandomPositionInBlockSpace()
    {
        // TODO: don't instantiate on top of other blocks or balls
        var randomUnitCirclePosition = Random.insideUnitCircle;
        return new Vector3(
            blockSpaceRadius * randomUnitCirclePosition.x,
            normalBlock.transform.position.y, // TODO: hacky
            blockSpaceRadius * randomUnitCirclePosition.y
        );
    }

    private void InstantiateBlock()
    {
        var blockLevel = Random.Range(1, _level + 1); // upper bound is exclusive
        var blockType = GetBlockTypeByLevel(blockLevel);
        InstantiateBlock(blockType);
    }

    private void InstantiateBlock(GameObject blockType)
    {
        var blockPosition = GetRandomPositionInBlockSpace();
        InstantiateBlock(blockType, blockPosition);
    }

    private void InstantiateBlock(Vector3 blockPosition)
    {
        var blockLevel = Random.Range(1, _level + 1); // upper bound is exclusive
        var blockType = GetBlockTypeByLevel(blockLevel);
        InstantiateBlock(blockType, blockPosition);
    }

    private void InstantiateBlock(GameObject blockType, Vector3 position)
    {
        var newBlock = Instantiate(blockType, position, Quaternion.identity, transform);
    }

    public void InstantiateBlockRing(Vector3 centre)
    {
        for (int i = 0; i < blockRingNumBlocks; i++)
        {
            var angle = 2 * Mathf.PI * i / blockRingNumBlocks;
            var offsetDirection = new Vector3(
                Mathf.Sin(angle),
                0,
                Mathf.Cos(angle)
            );
            var offset = offsetDirection * blockRingRadius;
            var position = centre + offset;

            if (position.magnitude < blockSpaceRadius)
            {
                InstantiateBlock(normalBlock, position);
            }
        }
    }

    public void IncreaseScore(int numPoints, int combo)
    {
        _score += numPoints * combo;
        UpdateUI();
    }
}