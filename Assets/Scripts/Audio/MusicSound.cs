using UnityEngine;

public class MusicSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    void Start()
    {
        _audio.Play();
    }


}
