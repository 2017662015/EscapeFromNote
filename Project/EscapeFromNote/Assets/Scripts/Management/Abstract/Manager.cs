using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour {
    //Instances
    private static T instance;
    private static object _lock = new object();

    //Variables
    private static bool applicationIsQuitting = false;
    //Getter Methods
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

    //Unity Callback Methods
    private void OnDestroy() { applicationIsQuitting = true; }
    protected virtual void OnEnable() {   }

}
