using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    private bool isPaused = false;

    private void OnEnable()
    {
        InputManager.PauseEvent += OnPause;
    }

    private void OnDisable()
    {
        InputManager.PauseEvent -= OnPause;
    }

    public void OnPause()
    {
        bool newPausedState = !isPaused;
        if (newPausedState)
            PauseScreen();
        else
            UnPauseScreen();
    }

    public void PauseScreen()
    {
        isPaused = true;
        Cursor.visible = true;
        FreezeTime();
        Cursor.lockState = CursorLockMode.Confined;
        _sceneLoader.LoadLevelAdditive(5);
    }

    public void UnPauseScreen()
    {
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UnfreezeTime();
        SceneManager.UnloadSceneAsync(5);
    }

    public void FreezeTime()
    {
        Time.timeScale = 0.0f;
    }

    public void UnfreezeTime()
    {
        Time.timeScale = 1.0f;
    }
}
