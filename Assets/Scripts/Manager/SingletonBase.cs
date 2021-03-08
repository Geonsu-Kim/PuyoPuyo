using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
    public static T instance;
    public static T Instance
    {
        get
        {
            instance = GameObject.FindObjectOfType(typeof(T)) as T;
            return instance;
        }
    }
}
