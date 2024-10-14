using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMenuScene();

        }
    }

    public static void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public static void LoadServerScene()
    {
        SceneManager.LoadScene(1);
    }

    public static void LoadClientScene()
    {
        SceneManager.LoadScene(2);
    }

    // Function to be called when the button is pressed
    public void ExitGame()
    {
        // Exit the application
        Application.Quit();

        // This line is helpful for testing in the Unity editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
