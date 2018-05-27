using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : Manager<GameManagement> {

    //Enums
    public enum GameState { NULL = -2, INIT, TITLE, OPTION_TITLE, INIT_PLAY, PLAY, PAUSE, OPTION_PAUSE, RESUME, GAMEOVER, FINALIZE };

    //Instances
    private Coroutine checkState;
    private StageManagement stageManagement;

    //Variables
    private GameState currentState;
    private GameState previousState;

    //Unity Callback Methods
    protected override void OnEnable()
    {
        base.OnEnable();
        DontDestroyOnLoad(this.gameObject);
        Init();
    }

    //Initialize Method of this class
    private void Init()
    {
        currentState = GameState.INIT;
        previousState = GameState.NULL;
        stageManagement = StageManagement.GetInstance();
        StartCoroutine(CheckState());
    }

    //State Machine Callback Methods
    private void OnInit()
    {
        currentState = GameState.TITLE;
    }
    private void OnTitle()
    {
        currentState = GameState.PLAY;
    }
    private void OnOptionTitle()
    {

    }
    private void OnInitPlay()
    {

    }
    private void OnPlay()
    {

    }
    private void OnPause()
    {

    }
    private void OnOptionPause()
    {

    }
    private void OnResume()
    {

    }
    private void OnGameOver()
    {

    }
    private void OnFinalize()
    {
    }
    
    //Methods

    //Coroutines
    private IEnumerator CheckState()
    {
        do
        {
            if (previousState != currentState)
            {
                previousState = currentState;
                switch (currentState)
                {
                    case GameState.INIT:
                        OnInit();
                        break;
                    case GameState.TITLE:
                        OnTitle();
                        break;
                    case GameState.OPTION_TITLE:
                        OnOptionTitle();
                        break;
                    case GameState.INIT_PLAY:
                        OnInitPlay();
                        break;
                    case GameState.PLAY:
                        OnPlay();
                        break;
                    case GameState.PAUSE:
                        OnPause();
                        break;
                    case GameState.OPTION_PAUSE:
                        OnOptionPause();
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
                    default:
                        break;
                }
                stageManagement.SetCurrentState(currentState);
            }
            yield return null;
        } while (true);
    }
}
