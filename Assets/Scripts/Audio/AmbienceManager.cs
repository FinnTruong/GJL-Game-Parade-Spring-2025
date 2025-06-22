using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AmbienceData
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    public float fadeDuration;
}

public class AmbienceManager : MonoBehaviour
{
    [SerializeField] AmbienceData[] ambienceData;
    public void Start()
    {
        for (int i = 0; i < ambienceData.Length; i++)
        {
            var data = ambienceData[i];
            AudioManager.Instance.PlayAmbience(data.clip, data.volume, data.fadeDuration);
        }
    }
}
