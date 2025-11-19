using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.6f;

    private void Reset()
    {
        fadeCanvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (fadeCanvasGroup != null)
        {
            yield return fadeCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true).WaitForCompletion();
        }

        var async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone) yield return null;

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            yield return fadeCanvasGroup.DOFade(0f, fadeDuration).SetUpdate(true).WaitForCompletion();
        }
    }
}

