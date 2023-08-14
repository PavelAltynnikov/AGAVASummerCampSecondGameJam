using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class GlobalVolume : MonoBehaviour
{
    [SerializeField] [Range(_min, _max)] private float _volume;

    private const float _min = 0f;
    private const float _max = 1f;

    private bool _muted;

    public SoundType Type => GetSoundType();

    public float Volume
    {
        get
        {
            if (_muted)
                return 0;
            else
                return _volume;
        }

        set
        {
            if (value > _max || value < _min)
            {
                throw new System.Exception("Volume value out of range");
            }
            _volume = value;
            VolumeChanged?.Invoke(Volume);
        }
    }

    public event UnityAction<float> VolumeChanged;
    public event UnityAction<bool> Muted;

    public enum SoundType
    {
        Music,
        InGameSounds
    }

    private void OnValidate()
    {
        VolumeChanged?.Invoke(Volume);
    }

    private void Start()
    {
        SetMute(LoadMute());
    }

    public static bool TryFind(SoundType type, out GlobalVolume foundVolume)
    {
        var volumes = FindObjectsOfType<GlobalVolume>();
        foundVolume = null;
        foreach (var volume in volumes)
        {
            if (volume.Type == type)
            {
                if(foundVolume == null)
                {
                    foundVolume = volume;
                }
                else
                {
                    throw new System.Exception("Multiple copies of GlobalVolume of this type found");
                }
            }
        }
        return foundVolume != null;
    }

    public void SwitchMute()
    {
        SetMute(!_muted);
    }

    private void SetMute(bool mute)
    {
        _muted = mute;
        VolumeChanged?.Invoke(Volume);
        Muted?.Invoke(_muted);
        UploadMute(mute);
    }

    public abstract bool LoadMute();

    public abstract void UploadMute(bool mute);

    public abstract SoundType GetSoundType();
}
