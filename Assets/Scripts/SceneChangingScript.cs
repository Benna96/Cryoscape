using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangingScript : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject loadingText;
    public void LoadStartScene()
    {
        StartCoroutine(LoadYourAsyncScene());
        startButton.SetActive(false);
        loadingText.SetActive(true);
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
