using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<TutorialStage> _stages;
    [SerializeField] private CanvasGroupPopUp _title;

    private int _currentIndex = -1;

    private TutorialStage _currentStage
    {
        get
        {
            if (_currentIndex < 0)
                return null;
            if (_currentIndex < _stages.Count )
                return _stages[_currentIndex];
            else
                return null;
        }
    }

    public event UnityAction<TutorialStage> StageChanged;
    public event UnityAction Completed;

    private void Awake()
    {
        foreach (var stage in _stages)
        {
            stage.Init(this);
        }
    }

    private void OnEnable()
    {
        foreach (var stage in _stages)
        {
            stage.Completed += OnStageCompleted;
        }
    }

    private void OnDisable()
    {
        foreach (var stage in _stages)
        {
            stage.Completed -= OnStageCompleted;
        }
    }

    private void Start()
    {
        if(DB.GetLevel() == 1)
        {
            _title.Show();
            TryNext();
        }
    }

    private void OnStageCompleted(TutorialStage stage)
    {
        if(_currentStage == stage)
            TryNext();
    }

    private void TryNext()
    {
        _currentIndex++;
        if (_currentIndex >= _stages.Count)
        {
            Win();
        }
        StageChanged?.Invoke(_currentStage);
    }

    private void Win()
    {
        _title.Hide();
        Completed?.Invoke();
    }
}
