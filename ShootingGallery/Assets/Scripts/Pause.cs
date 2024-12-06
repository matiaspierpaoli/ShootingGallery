using UnityEngine;

public class Pause : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private string _sceneName;

    public static event System.Action FinishedPause;

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
        PauseScreen();
    }

    public void PauseScreen()
    {
        Cursor.visible = true;
        FreezeTime();
        Cursor.lockState = CursorLockMode.Confined;
        _sceneLoader.LoadLevelAdditive(_sceneName);
    }

    public void UnPauseScreen()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        UnfreezeTime();
        _sceneLoader.UnloadScene(_sceneName);
        FinishedPause?.Invoke();
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
