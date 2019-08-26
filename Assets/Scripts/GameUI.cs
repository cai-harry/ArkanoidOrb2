using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Animator anim;
    
    public void UpdateText(int level, int lives, float score)
    {
        // TODO: make these textboxes automatically update when the variables change
        Text levelText = transform.Find("LevelText").GetComponent<Text>();
        Text livesText = transform.Find("LivesText").GetComponent<Text>();
        Text scoreText = transform.Find("ScoreText").GetComponent<Text>();
        levelText.text = $"Level {level}";
        livesText.text = $"{lives} Lives";
        scoreText.text = $"{score} Points";
    }

    public void ShowGameControls()
    {
        anim.Play("GameUIShowControls");
    }
}