using System;
using UnityEngine;
/// <summary>
///     Singleton for inheritance. Adds an Instance property to this class which can be used from other classes.
///     Does NOT create itself if it doesn't exist.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null) _instance = (T)FindObjectOfType(typeof(T));
            if (_instance == null)
                throw new NullReferenceException(
                    $"An instance of {typeof(T)} is needed in the scene, but there is none.");
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this as T;
        else if (_instance != this)
            Destroy(this);
        InitializeInstance();
    }

    /// <summary>
    ///     Initialize this instance.
    /// </summary>
    protected virtual void InitializeInstance()
    {
    }
}