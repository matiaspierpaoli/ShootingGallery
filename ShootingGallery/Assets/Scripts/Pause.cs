using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
 
    public void PauseScreen()
    {
        Cursor.visible = true;
        FreezeTime();
        Cursor.lockState = CursorLockMode.Confined;
        _sceneLoader.LoadLevelAdditive(5);
    }

    public void UnPauseScreen()
    {
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
