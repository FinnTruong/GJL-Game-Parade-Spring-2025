using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    #region Singleton
    public static AmbienceManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void StopSecretTrack()
    {
        //if(GameManager.instance.music != null)
        //AudioManager.instance.PlayMusic(GameManager.instance.music, 1f);
    }
}
