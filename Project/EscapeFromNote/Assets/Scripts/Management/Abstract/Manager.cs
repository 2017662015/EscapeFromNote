using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;
    private static object _lock = new object();

    private static bool applicationIsQuitting = false;
    private static int screenHeight, screenWidth;
    private static float maxRelScreenHeight, minRelScreenHeight, maxRelScreenWidth, minRelScreenWidth;

    public static T GetInstance()
    {
        if (applicationIsQuitting)
        {
            Debug.LogWarning(typeof(T) + " instance which you called is already destroyed on Application Quit");
            return null;
        }
        else
        {
            lock (_lock)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (!instance)
                {
                    GameObject _gameObject = new GameObject();
                    instance = _gameObject.AddComponent<T>();
                    _gameObject.name = typeof(T).ToString().Replace("Management", "Manager");
                    DontDestroyOnLoad(_gameObject);
                    return instance;
                }
                else
                {
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogWarning("One more then " + typeof(T) + " instances are in the world!! Please check your scene!! Might be fixed with reloading scene");
                        return null;
                    }
                    return instance;
                }
            }
        }
    }

    protected static float GetScreenHeight() { return screenHeight; }
    protected static float GetScreenWidth() { return screenWidth; }
    protected static float GetMaxRelScreenHeight() { return maxRelScreenHeight; }
    protected static float GetMaxRelScreenWidth() { return maxRelScreenWidth; }
    protected static float GetMinRelScreenHeight() { return minRelScreenHeight; }
    protected static float GetMinRelScreenWidth() { return minRelScreenWidth; }

    private void OnDestroy() { applicationIsQuitting = true; }
    protected virtual void OnEnable()
    {
        SetResolution();
    }
    
    private void SetResolution()
    {
        Resolution[] _resoulutions = Screen.resolutions;
        screenHeight = _resoulutions[0].height;
        screenWidth = _resoulutions[0].width;
        maxRelScreenHeight = screenHeight / 2;
        minRelScreenHeight = -maxRelScreenHeight;
        maxRelScreenWidth = screenWidth / 2;
        minRelScreenWidth = -minRelScreenWidth;
    }
}
