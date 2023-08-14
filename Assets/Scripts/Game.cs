using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Agava.YandexGames;
using Agava.YandexGames.Utility;
using UnityEngine.Analytics;
using Lean.Localization;

public class Game : MonoBehaviour
{
    [SerializeField] private Net _net;
    [SerializeField] private TapMark[] _masks;
    [SerializeField] private ComboText _combo;
    [SerializeField] private PopupText _plusText;
    [SerializeField] private ComboText _score;
    [SerializeField] private Aviary[] _aviaries;
    [SerializeField] private PopupText _plusTextPrefab;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private HandPointer _pointer;
    [SerializeField] private LevelDoneScreen _doneScreen;
    [SerializeField] private ParticleSystem[] _finishEffects;
    [SerializeField] private Tries _tries;
    [SerializeField] private List<LevelType> _levelTypes;
    [SerializeField] private Leaderboard _totalScoreLeaderboard;
    [SerializeField] private LeanToken _levelToken;

    private Aviary _lastAviary;
    private int _level;
    private const int _levelsPerScene = 4;
    private bool _levelComplete;

    public event UnityAction<int, LevelType> LevelStarted;
    public event UnityAction LevelCompleted;

    private void Awake()
    {
        _doneScreen.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DB.ResetLevel();
            Debug.Log("Reset level!");
        }
#if UNITY_EDITOR
        else if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(FinishGame());
        }
#endif
    }

    private void OnEnable()
    {
        _net.Selected += OnSelectedAnimals;
        _net.Deselected += OnDeselectedAnimals;
        _net.AnimalsChanged += OnAnimalsChanged;
        _combo.WillDisappear += DoneCombo;
        foreach (var item in _aviaries)
        {
            item.GotAnimal += OnGotAnimal;
        }
    }

    private void OnDisable()
    {
        _net.Selected -= OnSelectedAnimals;
        _net.Deselected -= OnDeselectedAnimals;
        _net.AnimalsChanged -= OnAnimalsChanged;
        _combo.WillDisappear -= DoneCombo;
        foreach (var item in _aviaries)
        {
            item.GotAnimal -= OnGotAnimal;
        }
    }

    public void Restart()
    {
        int level = DB.GetLevel();
        Dictionary<string, object> eventParameters = new Dictionary<string, object>
        {
            { "Level number",  level},
            {"result",  "lose" },
            {"continues" , _tries.UsedAd }
        };

        eventParameters.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StartLevel()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        InterestialAd.Show();
#endif
        _level = DB.GetLevel();
        _levelToken.SetValue(_level);
        int rows = 1 + ((_level - 1) % _levelsPerScene + 1) * 2;
        int cols = 4;
        var typeIndex = ((DB.GetLevel() - 1) / _levelsPerScene) % _levelTypes.Count;
        LevelStarted?.Invoke(_level, _levelTypes[typeIndex]);
        _net.BuildLevel(rows, cols);
        Dictionary<string, object> eventParameters = new Dictionary<string, object>
        {
            { "Level number",  _level},
        };

        eventParameters.Clear();
    }

    private void OnInterstitialVideoShown()
    {
        int sceneIndex = (DB.GetLevel() - 1) / _levelsPerScene;
        if (_levelComplete)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSelectedAnimals()
    {
        foreach (var item in _masks)
            item.Show();
    }

    private void OnDeselectedAnimals()
    {
        foreach (var item in _masks)
            item.Hide();
    }

    private void OnGotAnimal(Aviary aviary)
    {
        _lastAviary = aviary;

        if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            _combo.transform.position = Camera.main.WorldToScreenPoint(aviary.DoorPosition + aviary.transform.forward * 1.5f);
        }
        else
        {
            Vector3 worldSpacePosition = aviary.DoorPosition + aviary.transform.forward * 2.5f + aviary.transform.up * 4;
            _combo.transform.position = worldSpacePosition;
        }

        _combo.Increase();
    }

    private void DoneCombo(int combo)
    {
        if (combo > 1)
        {
            int score = combo * 10 + combo * (combo + 3) / 2;
            if (_lastAviary != null)
            {
                if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    _plusText.transform.position = Camera.main.WorldToScreenPoint(_lastAviary.DoorPosition + _lastAviary.transform.forward * 2.5f);
                else
                    _plusText.transform.position = _lastAviary.DoorPosition + _lastAviary.transform.forward * 3.5f + _lastAviary.transform.up * 4.5f;

                _plusText.Show("+" + score.ToString());
            }
            StartCoroutine(MovePlusText(_plusText, 0.4f, score));
        }
    }

    private void OnAnimalsChanged(int count)
    {
        if (count == 0)
        {
            StartCoroutine(FinishGame());
            _levelComplete = true;
            LevelCompleted?.Invoke();
        }
    }

    private IEnumerator FinishGame()
    {
        int level = DB.GetLevel();
        Dictionary<string, object> eventParameters = new Dictionary<string, object>
        {
            { "Level number",  level},
            {"result",  "win" },
            {"continues" , _tries.UsedAd }
        };

        eventParameters.Clear();

        yield return new WaitForSeconds(1f);

        foreach (var item in _finishEffects)
            item.Play();

        yield return new WaitForSeconds(0.1f);
        _doneScreen.Appear(_score.Value, _level);
        DB.AddScore(_score.Value);
        DB.IncreaseLevel();

#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif
        Agava.YandexGames.Leaderboard.GetPlayerEntry(_totalScoreLeaderboard.Name, (result) =>
        {
            if (result != null)
                Agava.YandexGames.Leaderboard.SetScore(_totalScoreLeaderboard.Name, result.score + _score.Value);
            else
                Agava.YandexGames.Leaderboard.SetScore(_totalScoreLeaderboard.Name, _score.Value);
        });
    }

    private IEnumerator MovePlusText(PopupText plusText, float delay, int score)
    {
        yield return new WaitForSeconds(delay);

        float duration = 0.3f;
        float time = 0;
        Vector3 position = plusText.transform.position;
        Vector3 target = _score.transform.position;
        while (time < duration)
        {
            float value = Ease.EaseInEaseOut(time / duration);
            plusText.transform.position = Vector3.Lerp(position, target, value);
            yield return null;
            time += Time.deltaTime;
        }

        _score.Increase(score);
    }

    private void OnApplicationQuit()
    {
        int level = DB.GetLevel();
        Dictionary<string, object> eventParameters = new Dictionary<string, object>
        {
            { "Level number",  level},
            {"result",  "leave" },
            {"continues" , _tries.UsedAd }
        };

        eventParameters.Clear();
    }
}
