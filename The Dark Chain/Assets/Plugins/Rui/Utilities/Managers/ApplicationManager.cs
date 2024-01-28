using JetBrains.Annotations;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
#if UNITY_WEBPLAYER
    public static string webplayerQuitURL = "http://google.com";
#endif
    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }

    [UsedImplicitly]
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}