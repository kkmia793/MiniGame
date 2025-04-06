using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using KanKikuchi.AudioManager;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    public event Action OnSceneLoadStarted;
    public event Action OnSceneLoadCompleted;
    public event Action<float> OnSceneLoadProgress;

    private CanvasGroup _fadeCanvasGroup;
    private int _KeyDownCount = 0;
    private bool _isTransitioning = false;
    private const float FADE_DURATION = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (_isTransitioning) return;

        if (IsTitleScene() && Input.anyKeyDown)
        {
            _KeyDownCount++;
            
            if (_KeyDownCount >= 2) 
            {
                _KeyDownCount = 0;
                LoadScene(ConstValues.MAIN_SCENE);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }

        if (BGMManager.Instance.IsPlaying()) return;
        BGMManager.Instance.Play(BGMPath.TITLE);
    }
    private bool IsTitleScene()
    {
        return SceneManager.GetActiveScene().name == ConstValues.TITLE_SCENE;
    }

    private void CreateFadeCanvas()
    {
        GameObject fadeCanvas = new GameObject("FadeCanvas");
        Canvas canvas = fadeCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        _fadeCanvasGroup = fadeCanvas.AddComponent<CanvasGroup>();
        _fadeCanvasGroup.alpha = 1f;
        _fadeCanvasGroup.blocksRaycasts = true;

        GameObject fadeImage = new GameObject("FadeImage");
        fadeImage.transform.SetParent(fadeCanvas.transform, false);
        RectTransform rect = fadeImage.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = fadeImage.AddComponent<Image>();
        image.color = Color.black;

        DontDestroyOnLoad(fadeCanvas);
    }

    public void LoadScene(string sceneName)
    {
        if (_isTransitioning) return;
        StartCoroutine(LoadSceneAsync(sceneName));
        
        BGMManager.Instance.FadeOut(BGMPath.TITLE);
    }

    public void LoadTitleScene()
    {
        if (_isTransitioning) return;
        StartCoroutine(LoadSceneAsync(ConstValues.TITLE_SCENE)); 
    }

    public void ReloadScene()
    {
        if (_isTransitioning) return;
        StartCoroutine(ReloadSceneAsync());
    }

    public void ReloadGame()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        _isTransitioning = true;
        OnSceneLoadStarted?.Invoke();

        yield return StartCoroutine(FadeOut(FADE_DURATION));

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            OnSceneLoadProgress?.Invoke(asyncLoad.progress);
            yield return null;
        }

        OnSceneLoadProgress?.Invoke(1.0f);
        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);

        yield return StartCoroutine(FadeIn(FADE_DURATION));

        OnSceneLoadCompleted?.Invoke();
        _isTransitioning = false;
    }

    private IEnumerator ReloadSceneAsync()
    {
        _isTransitioning = true;
        OnSceneLoadStarted?.Invoke();

        yield return StartCoroutine(FadeOut(FADE_DURATION));

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            OnSceneLoadProgress?.Invoke(asyncLoad.progress);
            yield return null;
        }

        OnSceneLoadProgress?.Invoke(1.0f);
        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);

        yield return StartCoroutine(FadeIn(FADE_DURATION));

        OnSceneLoadCompleted?.Invoke();
        _isTransitioning = false;
    }

    private IEnumerator FadeOut(float duration)
    {
        _fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
        if (_fadeCanvasGroup == null)
        {
            CreateFadeCanvas();
        }
        
        _fadeCanvasGroup.blocksRaycasts = true;
        _fadeCanvasGroup.interactable = true;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            _fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _fadeCanvasGroup.alpha = 1f;
    }

    public IEnumerator FadeIn(float duration)
    {
        _fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
        if (_fadeCanvasGroup == null)
        {
            CreateFadeCanvas();
        }
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            _fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _fadeCanvasGroup.alpha = 0f;
        _fadeCanvasGroup.blocksRaycasts = false;
        _fadeCanvasGroup.interactable = false;
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
