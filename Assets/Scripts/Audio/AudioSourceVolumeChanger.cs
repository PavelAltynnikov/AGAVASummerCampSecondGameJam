using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioSourceVolumeChanger : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private GlobalVolume.SoundType _type;

    private GlobalVolume _globalVolume;
    private float _baseVolume;

    public event UnityAction<AudioSourceVolumeChanger> Destroying;

    private void Awake()
    {
        _baseVolume = _audio.volume;
        if (GlobalVolume.TryFind(_type, out GlobalVolume volume))
        {
            _globalVolume = volume;
        }
        else
        {
            Debug.LogError("No GlobalVolumes of needed type found");
        }    
    }

    private void OnEnable()
    {
            _globalVolume.VolumeChanged += OnGlobalVolumeChanged;
    }

    private void OnDisable()
    {
        _globalVolume.VolumeChanged -= OnGlobalVolumeChanged;
    }

    private void OnGlobalVolumeChanged(float volume)
    {
        _audio.volume = _baseVolume * volume;
    }
}
