using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //[SerializeField] private string sceneName;

    public void LoadLevel(string sceneName)
    {
        //SceneManager.LoadScene(indexLoader);
        //int sceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelAdditive(string sceneName)
    {
        //SceneManager.LoadScene(indexLoader, LoadSceneMode.Additive);
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
