using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public void NextScene()
    {
        // Get the current scene's build index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the next scene's build index
        // Assuming you want to loop back to the first scene after the last one
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        DOTween.KillAll();
        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }
}