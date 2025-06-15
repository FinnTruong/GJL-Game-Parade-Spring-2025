using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip soundTrack;

    public void PlayMusic()
    {
        if (soundTrack != null)
            AudioManager.Instance.PlayMusic(soundTrack);
    }

    public void StopMusic()
    {
        AudioManager.Instance.PlayMusic(null);
    }    
}
