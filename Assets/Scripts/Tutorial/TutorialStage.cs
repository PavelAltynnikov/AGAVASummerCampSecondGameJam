using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TutorialStage : MonoBehaviour
{
    private Tutorial _tutorial;
    private bool _active = false;

    public bool Active => _active;

    public event UnityAction<TutorialStage> Completed;

    protected void Complete()
    {
        _active = false;
        Completed?.Invoke(this);
    }

    protected abstract void AtStart();

    public void Init(Tutorial tutorial)
    {
        if (_tutorial != null)
            throw new System.Exception("already inited");
        _tutorial = tutorial;
        _tutorial.StageChanged += OnStageChanged;
    }

    private void OnDestroy()
    {
        if (_tutorial != null) 
         _tutorial.StageChanged -= OnStageChanged;
    }

    private void OnStageChanged(TutorialStage stage)
    {
        if (stage == this)
        {
            _active = true;
            AtStart();
        }
    }
}
