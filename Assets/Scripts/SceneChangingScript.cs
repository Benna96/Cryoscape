using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangingScript : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject loadingText;
    [SerializeField] GameObject controlsWindow;
    public void LoadStartScene()
    {
        StartCoroutine(LoadYourAsyncScene());
        startButton.SetActive(false);
        loadingText.SetActive(true);
    }
    public void OpenControls()
    {
        startButton.SetActive(false);
        controlsWindow.SetActive(true);
    }
    public void CloseControls()
    {
        startButton.SetActive(true);
        controlsWindow.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
