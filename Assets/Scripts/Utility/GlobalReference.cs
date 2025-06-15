using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReference<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T singleton;

    public static bool IsInstanceValid => singleton != null;

    protected virtual void Awake()
    {
        singleton = (T)(MonoBehaviour)this;
    }

    public static T Instance
    {
        get
        {
            if (singleton == null)
            {
                singleton = (T)FindObjectOfType(typeof(T));
            }

            return singleton;
        }
    }

}
