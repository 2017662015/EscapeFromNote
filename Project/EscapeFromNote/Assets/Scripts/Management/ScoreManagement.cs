using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagement : Manager<ScoreManagement>
{
    private StageManagement stageManagement;

    private int killedEnemyCount;
    private int elapsedStage;
    private int stageScoreSum;
    private int nextStageScore = 0;
    private float elapsedTime;
    private GameManagement.GameState currentState;
    private GameManagement.GameState previousState;
    private Character.BehaviourState currentBehaviourState;
    private Character.BehaviourState previousBehaviourState;

    //Constants
    private const int SCORE_TIME_INIT = 3;
    private const int SCORE_TIME_UNIT = 2;
    private const int SCORE_STAGE_UNIT = 100;

    public void IncreaseKilledEnemyCount() { killedEnemyCount++; }
    public void SetCurrentState(GameManagement.GameState state) { this.currentState = state; }
    public void SetCurrentBehaviourState(Character.BehaviourState state) { this.currentBehaviourState = state; }

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }
    private void Update()
    {
        GettingElaspedTime();
    }

    private void Init()
    {
        stageManagement = StageManagement.GetInstance();
        killedEnemyCount = 0;
        elapsedTime = 0;
        StartCoroutine(CheckState());
    }

    private void GettingElaspedTime()
    { 
        if(currentState == GameManagement.GameState.PLAY)
        {
            elapsedTime += Time.deltaTime;
        }
    }
    private void SumStageScore()
    {
        stageScoreSum += nextStageScore;
        nextStageScore = SCORE_STAGE_UNIT * elapsedStage;
    }

    private void OnGoToNextStage()
    {
        SumStageScore();
        currentBehaviourState = Character.BehaviourState.IDLE;
    }

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
                    case GameManagement.GameState.NULL:
                        break;
                    case GameManagement.GameState.INIT:
                        break;
                    case GameManagement.GameState.TITLE:
                        break;
                    case GameManagement.GameState.OPTION_TITLE:
                        break;
                    case GameManagement.GameState.INIT_PLAY:
                        break;
                    case GameManagement.GameState.PLAY:
                        StartCoroutine(CheckStage());
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
        } while (currentState != GameManagement.GameState.FINALIZE);
    }
    private IEnumerator CheckBehaviourState()
    {
        do
        {
            if (previousBehaviourState != currentBehaviourState)
            {
                previousBehaviourState = currentBehaviourState;
                switch (currentBehaviourState)
                {
                    case Character.BehaviourState.IDLE:
                        break;
                    case Character.BehaviourState.GO_TO_NEXT_STAGE:
                        OnGoToNextStage();
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        } while (true);
    }
    private IEnumerator CheckStage()
    {
        do
        {
            if (elapsedStage != stageManagement.GetStage())
            {
                elapsedStage = stageManagement.GetStage();
                currentBehaviourState = Character.BehaviourState.GO_TO_NEXT_STAGE;
            }
            yield return null;
        } while (currentState != GameManagement.GameState.GAMEOVER);
    }
}

