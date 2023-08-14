using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InGameSound : GlobalVolume
{
    public override SoundType GetSoundType()
    {
        return SoundType.InGameSounds;
    }

    public override bool LoadMute()
    {
        return !DB.GetInGameSound();
    }

    public override void UploadMute(bool mute)
    {
        DB.SetInGameSound(!mute);
    }
}
