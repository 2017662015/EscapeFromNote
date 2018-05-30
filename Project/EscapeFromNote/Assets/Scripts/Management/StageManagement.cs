using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagement : Manager<StageManagement>
{
    //Instances
    private GameManagement gameManagement;

    //Variables
    private int stage;
    private float currentTime;
    private GameManagement.GameState currentState;
    private GameManagement.GameState previousState;

    //Constatnts
    public const float STAGE_INTERVAL_TIME = 30.0f;

    //Getter Methods
    public int GetStage() { return stage; }

    //Setter Methods
    public void SetCurrentState(GameManagement.GameState state) { this.currentState = state; }

    //Unity Callback Methods
    private void FixedUpdate()
    {
        IncreraseStage();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }
    
    //Initialize Method of this class
    private void Init()
    {
        currentState = GameManagement.GameState.INIT;
        previousState = GameManagement.GameState.NULL;
        gameManagement = GameManagement.GetInstance();
        StartCoroutine(CheckState());
    }
    
    //Methods
    private void IncreraseStage()
    {
       if(currentState == GameManagement.GameState.PLAY)
        {
            if(currentTime < STAGE_INTERVAL_TIME * stage)
            {
                currentTime += Time.fixedDeltaTime;
            }
            else
            {
                stage++;
            }
        }
    }

    //Coroutines
    private IEnumerator CheckState()
    {
        do
        {
            if(previousState != currentState)
            {
                previousState = currentState;
                switch (currentState)
                {
                    case GameManagement.GameState.INIT:
                        break;
                    case GameManagement.GameState.TITLE:
                        break;
                    case GameManagement.GameState.OPTION_TITLE:
                        break;
                    case GameManagement.GameState.INIT_PLAY:
                        break;
                    case GameManagement.GameState.PLAY:
                        break;
                    case GameManagement.GameState.PAUSE:
                        break;
                    case GameManagement.GameState.OPTION_PAUSE:
                        break;
                    case GameManagement.GameState.RESUME:
                        break;
                    case GameManagement.GameState.GAMEOVER:
                        break;
                    case GameManagement.GameState.BACK_TO_TITLE:
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        } while (currentState != GameManagement.GameState.BACK_TO_TITLE);
    }
}
