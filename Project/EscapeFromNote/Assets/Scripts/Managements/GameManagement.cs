using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// GameManagement는 게임의 시스템을 관리하는 Component입니다.
/// </summary>

public class GameManagement : MonoBehaviour {

    /* File Name : GameManagement.cs
     * Author Name  : Jun-Hyeok Lim
     * Write Date : 2018-04-24 01:58
     */

    //Singleton Pattern용 본 Class의 Instance
    /// <summary>GameManagement.instance는 GameManagement Class의 Instance입니다.</summary>
    private static GameManagement instance;

    //GameManagement.instance의 Getter
    /// <summary>GameManagement.GetInstance는 GameManagement.instance를 반환하는 Method입니다. 
    /// 인스턴스가 Null인 경우 World에 있는 GameManager를 찾아서 컴포넌트를 가져와 반환합니다.</summary>
    /// <returns>GameManagement.instance를 반환합니다.</returns>
    public static GameManagement GetInstance()
    {
        if(!instance)
        {
            instance = GameObject.Find("GameManager").GetComponent<GameManagement>();
        }
        return instance;
    }

    //Resource Field
    private List<Scene> scenes_level = new List<Scene>();

    //Enum Field
    /// <summary>GameManagement.GameState는 게임의 상태를 정의한 열거자입니다.</summary>
    public enum GameState
    {
        NULL = -1,
        INIT,
        TITLE,
        OPTION,
        PLAY,
        PAUSE,
        RESUME,
        INGAMEOPTION,
        GAMEOVER,
        FINALIZE
    };

    //Instatnce Field
    private static GameObject playerManager;
    private static GameObject uiManager;
    private static GameObject stageManager;
    private static Coroutine checkState;
    private static SceneManager sceneManager;

    //Variable Field
    private static int stage;
    private static GameState mGameState;
    private static GameState previousGameState;

    //Getter
    public static int GetStage() { return stage; }

    //Unity Callback Method Field
    private void Awake()
    {
        Init();
    }

    //Initialize Method
    private void Init()
    {
        DontDestroyOnLoad(this.gameObject);
        mGameState = GameState.INIT;
        previousGameState = GameState.NULL;
        checkState = StartCoroutine(CheckState());
    }

    //State Machine Callback Method Field
    private void OnInit()
    {
        playerManager = SpawnManager<PlayerManagement>();
        mGameState = GameState.TITLE;
    }
    private void OnTitle()
    {
    }
    private void OnOption()
    {
        //TODO: 타이틀 화면에서 옵션을 누를때 동작할 기능을 넣어주세요.
    }
    private void OnPlay()
    {
        //TODO: 게임 최초 시작시 동작할 기능을 넣어주세요.
    }
    private void OnPause()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //TODO: 게임 일시 정지시 동작할 기능을 넣어주세요.
    }
    private void OnInGameOption()
    {
        //TODO: 게임 일시 정지시에 옵션 버튼을 누르면 동작할 기능을 넣어주세요.
    }
    private void OnResume()
    {
        //TODO: 게임 일시 정지 해제 시 동작할 기능을 넣어주세요.
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    private void OnGameOver()
    {
        //TODO: 게임 종료 후 판정 결과 창 때 동작할 기능을 넣어주세요.
    }
    private void OnFinalize()
    {
        //TODO: 프로그램 종료 시에 동작할 기능을 넣    어주세요.
    }
    
    //Method
    public static void IncreaseStage() { stage++; }
    /// <summary>GameObject.SpawnManager는 Management Component를 가지고 있는 Manager Object를 만들어주는 Method입니다.</summary>
    /// <typeparam name="T">Management Component</typeparam>
    /// <returns>Management Component를 가진 GameObject를 반환합니다.</returns>
    private GameObject SpawnManager<T>() where T : Component
    {
        GameObject _manager = new GameObject();
        string _managerName = "Manager";
        string _managementName = "Management";
        _manager.AddComponent<T>();
        string _objectName = _manager.GetComponent<T>().GetType().ToString().Replace(_managementName, _managerName);
        Debug.Log(_objectName);
        _manager.name = _objectName;
        DontDestroyOnLoad(_manager);
        return _manager;
    }

    //Coroutine Field
    /// <summary>GameManagement.StateMachine은 게임의 상태가 변경되면 Callback Method를 호출하는 State Machine Coroutine입니다.</summary>
    private IEnumerator CheckState()
    {
        do
        {
            if(previousGameState != mGameState)
            {
                previousGameState = mGameState;
                switch (mGameState)
                {
                    case GameState.INIT:
                        OnInit();
                        break;
                    case GameState.TITLE:
                        OnTitle();
                        break;
                    case GameState.OPTION:
                        OnOption();
                        break;
                    case GameState.PLAY:
                        OnPlay();
                        break;
                    case GameState.PAUSE:
                        OnPause();
                        break;
                    case GameState.INGAMEOPTION:
                        OnInGameOption();
                        break;
                    case GameState.RESUME:
                        OnResume();
                        break;
                    case GameState.GAMEOVER:
                        OnGameOver();
                        break;
                    case GameState.FINALIZE:
                        OnFinalize();
                        break;
                }
            }
            yield return null;
        } while (true);
    }
}
