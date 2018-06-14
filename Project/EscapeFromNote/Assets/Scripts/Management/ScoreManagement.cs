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
    private float timeFactor = 0;
    private GameManagement.GameState currentState;
    private GameManagement.GameState previousState;
    private Character.BehaviourState currentBehaviourState;
    private Character.BehaviourState previousBehaviourState;

    //Constants
    private const int SCORE_TIME_INIT = 3;
    private const int SCORE_TIME_UNIT = 2;
    private const int SCORE_STAGE_UNIT = 100;

    public int GetKilledEnemyCount() { return this.killedEnemyCount; }
    public int GetElapsedStage() { return this.elapsedStage; }
    public int GetElapsedTime() { return (int)this.elapsedTime; }
    public int GetStageScoreSum() { return this.stageScoreSum; }
    
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
        SumTimeScore();
    }

    private void Init()
    {
        stageManagement = StageManagement.GetInstance();
        killedEnemyCount = 0;
        elapsedTime = 0;
        StartCoroutine(CheckState());
        StartCoroutine(CheckBehaviourState());
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
        nextStageScore = SCORE_STAGE_UNIT * (elapsedStage - 1);
        stageScoreSum += nextStageScore;
    }

    private void OnGoToNextStage()
    {
        if (elapsedStage > 1)
        {
            SumStageScore();
        }
        currentBehaviourState = Character.BehaviourState.IDLE;
    }

    private void SumTimeScore()
    {
        if (currentState == GameManagement.GameState.PLAY)
        {
            if (timeFactor < 1.0f)
            {
                timeFactor += Time.deltaTime;
            }
            else
            {
                stageScoreSum += (elapsedStage * SCORE_TIME_UNIT) + SCORE_TIME_INIT;
                timeFactor = 0;
            }
        }
    }
    private void OnBackToTitle()
    {
        killedEnemyCount = 0;
        elapsedStage = 0;
        stageScoreSum = 0;
        nextStageScore = 0;
        elapsedTime = 0;
        timeFactor = 0;
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
                        OnBackToTitle();
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

