using System;
using System.Collections;
using UnityEngine;
using KanKikuchi.AudioManager;
using UnityEngine.Rendering;
using unityroom.Api;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Init, Ready, Playing, GameOver }
    private GameState _currentState = GameState.Init;

    public GameState CurrentState => _currentState;

    private int _crossCount = 0;

    [SerializeField, Header("制限時間")]
    private float _remainingTime = 30f;

    public event Action<float> OnTimeUpdated;
    public event Action<int> OnCrossCountUpdated;
    public event Action<int> OnGameOver;
    public event Action<GameState> OnGameStateChanged;
    public event Action OnCountdownStart;
    public event Action OnCountdownComplete;

    private const float TIME_SCALE_REDUCE_DURATION = 1.0f;

    private earthOrbit _earthOrbit;

    private IEnumerator _ReloadRoutine;
    private IEnumerator _GameRoutine;

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

    private void Start()
    {
        StartGameInit();
    }

    private void OnEnable()
    {
        Life.OnLifeDecreased += OnPlayerLifeDecreased;
        _earthOrbit = FindObjectOfType<earthOrbit>();

        _ReloadRoutine = SmoothTimeScaleToZero();
        _GameRoutine = StartGameRoutine();

        if (_earthOrbit != null)
        {
            _earthOrbit.OnCrossLine += IncreaseCrossCount;
        }
    }

    private void OnDisable()
    {
        Life.OnLifeDecreased -= OnPlayerLifeDecreased;

        if (_earthOrbit != null)
        {
            _earthOrbit.OnCrossLine -= IncreaseCrossCount;
        }
    }

    private void Update()
    {
        if (_earthOrbit == null)
        {
            _earthOrbit = FindObjectOfType<earthOrbit>();  
            _earthOrbit.OnCrossLine += IncreaseCrossCount;  
            OnCrossCountUpdated?.Invoke(_crossCount);
        }

        if (_currentState != GameState.Playing) return;

        UpdateTimer();
    }

    public void StartGameInit()
    {
        _currentState = GameState.Init;
        OnGameStateChanged?.Invoke(_currentState);

        InitializeGame();
    }

    private void InitializeGame()
    {
        _currentState = GameState.Ready;
        OnGameStateChanged?.Invoke(_currentState);

        _remainingTime = 30f;
        _crossCount = 0;

        OnTimeUpdated?.Invoke(_remainingTime);
        OnCrossCountUpdated?.Invoke(_crossCount);

        Time.timeScale = 1f;

        InitStartGameCoroutines();
    }

    private void UpdateTimer()
    {
        _remainingTime = Mathf.Max(0, _remainingTime - Time.deltaTime);
        OnTimeUpdated?.Invoke(_remainingTime);

        if (_remainingTime <= 0)
        {
            GameOver();
        }
    }

    private void IncreaseCrossCount()
    {
        if (_currentState != GameState.Playing) return;
        _crossCount++;
        OnCrossCountUpdated?.Invoke(_crossCount);
        SEManager.Instance.Play(SEPath.TANABATA_LINE);
    }

    private void OnPlayerLifeDecreased(int remainingLife)
    {
        if (remainingLife <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (_currentState == GameState.GameOver) return;

        _currentState = GameState.GameOver;
        OnGameStateChanged?.Invoke(_currentState);
        OnGameOver?.Invoke(_crossCount);
        
        UnityroomApiClient.Instance.SendScore(1, _crossCount, ScoreboardWriteMode.HighScoreDesc);

        InitTimeScale();
        
        StartCoroutine(WaitForRestartInput());
    }

    private IEnumerator WaitForRestartInput()
    {
        yield return new WaitForSecondsRealtime(0.5f); 

        while (!Input.anyKeyDown) 
        {
            yield return null;
        }

        RestartGame();
    }

    private void RestartGame()
    {
        StopCoroutine(_ReloadRoutine);
        StartGameInit();
        SceneController.Instance.ReloadGame();
    }

    private void InitTimeScale()
    {
        StopCoroutine(_ReloadRoutine);
        _ReloadRoutine = SmoothTimeScaleToZero();
        StartCoroutine(_ReloadRoutine);
    }

    private void InitStartGameCoroutines()
    {
        StopCoroutine(_GameRoutine);
        _GameRoutine = StartGameRoutine();
        StartCoroutine(_GameRoutine);
    }

    private IEnumerator SmoothTimeScaleToZero()
    {
        float startValue = Time.timeScale;
        float elapsedTime = 0f;

        while (elapsedTime < TIME_SCALE_REDUCE_DURATION)
        {
            Time.timeScale = Mathf.Lerp(startValue, 0f, elapsedTime / TIME_SCALE_REDUCE_DURATION);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        StartGameInit();
    }

    private IEnumerator StartGameRoutine()
    {
        yield return SceneController.Instance.FadeIn(0.5f);
        OnGameStateChanged?.Invoke(GameState.Ready);

        BGMManager.Instance.Stop(BGMPath.TITLE);
        SEManager.Instance.Stop();
        BGMManager.Instance.ChangeBaseVolume(1f);
        if (!BGMManager.Instance.IsPlaying())
        {
            BGMManager.Instance.Play(BGMPath.MAIN); 
        }

        yield return new WaitForSeconds(1f);

        OnCountdownStart?.Invoke();

        yield return new WaitForSeconds(2.5f);

        // ゲームスタート
        _currentState = GameState.Playing;
        OnGameStateChanged?.Invoke(_currentState);
        OnCountdownComplete?.Invoke();
    }
}
