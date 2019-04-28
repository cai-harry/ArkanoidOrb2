using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    public GameObject stage;
    public GameObject paddles;
    public GameObject ball;
    
    public float firstBlockSeconds;
    public float addBlockRate;
    public float blockSpaceRadius;

    public GameObject normalBlock;
    public GameObject rockBlock;
    public GameObject ballBlock;
    public GameObject explodeBlock;

    public AudioSource blackFlowerSong;
    public AudioSource strikebeamSong;

    public GameObject startScreen;
    public GameObject pauseScreen;
    
    private bool _paused;
    
    void Start()
    {
        Instantiate(stage);
        Instantiate(paddles);
        Instantiate(ball);
        
        InvokeRepeating("AddRandomBlock", firstBlockSeconds, addBlockRate);
        Time.timeScale = 0f;
        _paused = true;
        Instantiate(startScreen);
        strikebeamSong.Play();
        strikebeamSong.Pause();
        blackFlowerSong.Play();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause();
        }

        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
        {
            RestartGame();
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
        var randFloat = Random.Range(0f, 1f);
        if (randFloat < 0.25f)
        {
            InstantiateBlock(explodeBlock);
        }
        else if (randFloat < 0.5f)
        {
            InstantiateBlock(ballBlock);
        }
        else if (randFloat < 0.75f)
        {
            InstantiateBlock(rockBlock);
        }
        else
        {
            InstantiateBlock(normalBlock);
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