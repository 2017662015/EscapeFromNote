using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagement : Manager<UIManagement>
{
    //Prefab Instances
    private GameObject prefab_beginBG;
    private GameObject prefab_exitBG;
    private GameObject prefab_gameOverBG;
    private GameObject prefab_gameSceneBG;
    private GameObject prefab_optionBG;
    private GameObject prefab_startSceneBG;

    //Instances
    private GameObject beginBG;
    private GameObject exitBG;
    private GameObject gameOverBG;
    private GameObject gameSceneBG;
    private GameObject startSceneBG;
    private GameObject optionBG;
    private GameObject startSceneBG_play;
    private GameObject startSceneBG_option;
    private GameObject startSceneBG_exit;
    private GameObject optionBG_back;
    private GameObject exitBG_yes;
    private GameObject exitBG_no;
    private GameObject gameOverBG_back;
    private GameObject gameSceneBG_white;
    private Transform uiTransform;
    private GameManagement gameManagement;
    private StageManagement stageManagement;

    //Variables
    private float titleFactor = 0.0f;
    private GameManagement.GameState currentState;
    private GameManagement.GameState previousState;

    //Constants
    private const float TITLE_SPLESH_LENGTH = 3.0f;
    
    public GameObject GetGameSceneBG_white() { return this.gameSceneBG_white; }

    public void SetCurrentState(GameManagement.GameState state) { this.currentState = state; }

    private void Update()
    {
        WaitForTitleSplash();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    private void Init()
    {
        currentState = GameManagement.GameState.INIT;
        previousState = GameManagement.GameState.NULL;
        prefab_beginBG = Resources.Load("Prefabs/UI/BeginBG") as GameObject;
        prefab_exitBG = Resources.Load("Prefabs/UI/ExitBG") as GameObject;
        prefab_gameOverBG = Resources.Load("Prefabs/UI/GameOverBG") as GameObject;
        prefab_gameSceneBG = Resources.Load("Prefabs/UI/GameSceneBG") as GameObject;
        prefab_startSceneBG = Resources.Load("Prefabs/UI/StartSceneBG") as GameObject;
        prefab_optionBG = Resources.Load("Prefabs/UI/OptionBG") as GameObject;

        uiTransform = GameObject.Find("UI").transform;
        gameManagement = GameManagement.GetInstance();

        beginBG = Instantiate<GameObject>(prefab_beginBG, uiTransform);
        exitBG = Instantiate<GameObject>(prefab_exitBG, uiTransform);
        gameOverBG = Instantiate<GameObject>(prefab_gameOverBG, uiTransform);
        startSceneBG = Instantiate<GameObject>(prefab_startSceneBG, uiTransform);
        gameSceneBG = Instantiate<GameObject>(prefab_gameSceneBG, uiTransform);
        optionBG = Instantiate<GameObject>(prefab_optionBG, uiTransform);

        startSceneBG_play = startSceneBG.transform.GetChild(1).gameObject;
        startSceneBG_option = startSceneBG.transform.GetChild(2).gameObject;
        startSceneBG_exit = startSceneBG.transform.GetChild(3).gameObject;

        optionBG_back = optionBG.transform.GetChild(3).gameObject;

        exitBG_yes = exitBG.transform.GetChild(1).gameObject;
        exitBG_no = exitBG.transform.GetChild(2).gameObject;

        gameOverBG_back = gameOverBG.transform.GetChild(4).gameObject;

        gameSceneBG_white = gameSceneBG.transform.GetChild(2).gameObject;

        AddOnClick(startSceneBG_play, "OnPressedPlayInTitle");
        AddOnClick(startSceneBG_option, "OnPressedOptionInTitle");
        AddOnClick(startSceneBG_exit, "OnPressedExitInTitle");
        AddOnClick(optionBG_back, "OnPressedBackInOption");
        AddOnClick(exitBG_yes, "OnPressedYesInExit");
        AddOnClick(exitBG_no, "OnPressedNoInExit");
        AddOnClick(gameOverBG_back, "OnPressedBackInResult");
        AddOnClick(gameSceneBG_white, "OnPressedWhiteInGame");

        beginBG.SetActive(false);
        exitBG.SetActive(false);
        gameOverBG.SetActive(false);
        optionBG.SetActive(false);
        startSceneBG.SetActive(false);
        gameSceneBG.SetActive(false);

        StartCoroutine(CheckState());
    }

    //State Machine Callback Methods
    private void OnInit()
    {
        beginBG.SetActive(true);
    }
    private void OnTitle()
    {
        beginBG.SetActive(false);
        startSceneBG.SetActive(true);
        exitBG.SetActive(false);
        optionBG.SetActive(false);
        gameOverBG.SetActive(false);
    }
    private void OnOptionTitle()
    {
        optionBG.SetActive(true);
    }
    private void OnExitTitle()
    {
        exitBG.SetActive(true);
    }
    private void OnInitPlay()
    {
        startSceneBG.SetActive(false);
        gameSceneBG.SetActive(true);
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
        gameSceneBG_white.SetActive(false);
        gameSceneBG.SetActive(false);
        gameOverBG.SetActive(true);
    }
    private void OnBackToTitle()
    {
        
    }
    private void OnFinalize()
    {

    }

    //Methods
    private void AddOnClick(GameObject btn, string method)
    {
        EventDelegate _delegate = new EventDelegate(this, method);
        EventDelegate.Add(btn.GetComponent<UIButton>().onClick, _delegate);
    }
    private void WaitForTitleSplash()
    {
        if(currentState == GameManagement.GameState.INIT)
        {
            if(titleFactor < TITLE_SPLESH_LENGTH)
            {
                titleFactor += Time.deltaTime;
            }
            else
            {
                titleFactor = 0;
                gameManagement.SetCurrentState(GameManagement.GameState.TITLE);
            }
        }
    }
    private void OnPressedPlayInTitle()
    {
        gameManagement.SetCurrentState(GameManagement.GameState.INIT_PLAY);
    }
    private void OnPressedOptionInTitle()
    {
        gameManagement.SetCurrentState(GameManagement.GameState.OPTION_TITLE);
    }
    private void OnPressedExitInTitle()
    {
        gameManagement.SetCurrentState(GameManagement.GameState.EXIT_TITLE);
    }
    private void OnPressedYesInExit()
    {
        Application.Quit();
    }
    private void OnPressedNoInExit()
    {
        gameManagement.SetCurrentState(GameManagement.GameState.TITLE);
    }
    private void OnPressedBackInOption()
    {
        gameManagement.SetCurrentState(GameManagement.GameState.TITLE);
    }
    private void OnPressedBackInResult()
    {
        gameManagement.SetCurrentState(GameManagement.GameState.BACK_TO_TITLE);
    }
    private void OnPressedWhiteInGame()
    {
        GameObject.Find("Player").GetComponent<PlayerInf>().SetCurrentState(Character.BehaviourState.SKILL);
        gameSceneBG_white.SetActive(false);
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
                    case GameManagement.GameState.INIT:
                        OnInit();
                        break;
                    case GameManagement.GameState.TITLE:
                        OnTitle();
                        break;
                    case GameManagement.GameState.OPTION_TITLE:
                        OnOptionTitle();
                        break;
                    case GameManagement.GameState.EXIT_TITLE:
                        OnExitTitle();
                        break;
                    case GameManagement.GameState.INIT_PLAY:
                        OnInitPlay();
                        break;
                    case GameManagement.GameState.PLAY:
                        OnPlay();
                        break;
                    case GameManagement.GameState.PAUSE:
                        OnPause();
                        break;
                    case GameManagement.GameState.OPTION_PAUSE:
                        OnOptionPause();
                        break;
                    case GameManagement.GameState.RESUME:
                        OnResume();
                        break;
                    case GameManagement.GameState.GAMEOVER:
                        OnGameOver();
                        break;
                    case GameManagement.GameState.BACK_TO_TITLE:
                        OnBackToTitle();
                        break;
                    case GameManagement.GameState.FINALIZE:
                        OnFinalize();
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        } while (true);
    }
}
