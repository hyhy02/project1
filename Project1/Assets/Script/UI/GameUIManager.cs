using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private CanvasGroup gameClearCanvasGroup;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;

    [SerializeField] private float FadeDuration = 1f; // 페이드 효과 시간

    private void Start()
    {
        gameClearPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }
    public void GameClear()
    {
        gameClearPanel.SetActive(true);
        StartCoroutine(FadeOut(gameClearCanvasGroup));
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        StartCoroutine(FadeOut(gameOverCanvasGroup));
    }

    public void GameRestart()
    {
        Time.timeScale = 1f;
        GameManager.Instance.HideCursor();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        yield return new WaitForSeconds(2.5f);

        canvasGroup.alpha = 0f;

        float elapsed = 0f;

        while (elapsed < FadeDuration)
        {
            elapsed += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / FadeDuration);

            yield return null;
        }

        canvasGroup.alpha = 1f;
        Time.timeScale = 0f;
        GameManager.Instance.ShowCursor();
    }
}
