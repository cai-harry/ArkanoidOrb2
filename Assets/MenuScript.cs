using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public string gameSceneName;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            LoadGameScene();
        }
    }

    private void LoadGameScene()
    {
        Debug.Log("Loading game scene");
        SceneManager.LoadScene(gameSceneName);
    }
}
