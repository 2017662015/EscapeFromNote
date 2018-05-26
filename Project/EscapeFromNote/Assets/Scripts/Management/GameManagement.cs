using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : Manager<GameManagement> {

    public enum GameState { NULL = -2, INIT, TITLE, OPTION_TITLE, INIT_PLAY, PLAY, PAUSE, OPTION_PAUSE, RESUME, GAMEOVER, FINALIZE };

    private GameState currentState;
    private GameState previousState;

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    private void Init()
    {

    }

    private void OnInit()
    {

    }
    private void OnTitle()
    {

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


    private IEnumerator CheckState()
    {
        do
        {
            switch (currentState)
            {
                case GameState.NULL:
                    break;
                case GameState.INIT:
                    break;
                case GameState.TITLE:
                    break;
                case GameState.OPTION_TITLE:
                    break;
                case GameState.INIT_PLAY:
                    break;
                case GameState.PLAY:
                    break;
                case GameState.PAUSE:
                    break;
                case GameState.OPTION_PAUSE:
                    break;
                case GameState.RESUME:
                    break;
                case GameState.GAMEOVER:
                    break;
                case GameState.FINALIZE:
                    break;
                default:
                    break;
            }
        } while (true);
    }
}
