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
    private GameObject prefab_inGameOptionBG;

    //Instances
    private GameObject beginBG;
    private GameObject exitBG;
    private GameObject gameOverBG;
    private GameObject gameSceneBG;
    private GameObject startSceneBG;
    private GameObject optionBG;
    private GameObject inGameOptionBG;
    private GameObject startSceneBG_play;
    private GameObject startSceneBG_option;
    private GameObject startSceneBG_exit;
    private GameObject optionBG_back;
    private GameObject exitBG_yes;
    private GameObject exitBG_no;
    private GameObject gameOverBG_back;
    private GameObject gameSceneBG_white;
    private GameObject gameSceneBG_score;
    private GameObject gameSceneBG_time;
    private GameObject gameSceneBG_hp;
    private GameObject gameSceneBG_option;
    private GameObject inGameOptionBG_back;
    private GameObject inGameOptionBG_title;
    private Transform uiTransform;
    private GameManagement gameManagement;
    private StageManagement stageManagement;
    private ScoreManagement scoreManagement;
    private PlayerManagement playerManagement;
    private UILabel gameSceneBG_scoreText;
    private UILabel gameSceneBG_timeText;
    private UILabel gameSceneBG_hpText;
    private UILabel gameOverBG_killTheEnemyText;
    private UILabel gameOverBG_timeText;
    private UILabel gameOverBG_scoreText;

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
        GameScoreUpdate();
        PlayerHPUpdate();
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
        prefab_inGameOptionBG = Resources.Load("Prefabs/UI/InGameOptionBG") as GameObject;

        uiTransform = GameObject.Find("UI").transform;
        gameManagement = GameManagement.GetInstance();
        scoreManagement = ScoreManagement.GetInstance();
        stageManagement = StageManagement.GetInstance();
        playerManagement = PlayerManagement.GetInstance();

        beginBG = Instantiate<GameObject>(prefab_beginBG, uiTransform);
        exitBG = Instantiate<GameObject>(prefab_exitBG, uiTransform);
        gameOverBG = Instantiate<GameObject>(prefab_gameOverBG, uiTransform);
        startSceneBG = Instantiate<GameObject>(prefab_startSceneBG, uiTransform);
        gameSceneBG = Instantiate<GameObject>(prefab_gameSceneBG, uiTransform);
        optionBG = Instantiate<GameObject>(prefab_optionBG, uiTransform);
        inGameOptionBG = Instantiate<GameObject>(prefab_inGameOptionBG, uiTransform);

        startSceneBG_play = startSceneBG.transform.GetChild(1).gameObject;
        startSceneBG_option = startSceneBG.transform.GetChild(2).gameObject;
        startSceneBG_exit = startSceneBG.transform.GetChild(3).gameObject;

        optionBG_back = optionBG.transform.GetChild(3).gameObject;

        exitBG_yes = exitBG.transform.GetChild(1).gameObject;
        exitBG_no = exitBG.transform.GetChild(2).gameObject;

        gameOverBG_killTheEnemyText = gameOverBG.transform.GetChild(5).GetComponent<UILabel>();
        gameOverBG_scoreText = gameOverBG.transform.GetChild(7).GetComponent<UILabel>();
        gameOverBG_timeText = gameOverBG.transform.GetChild(6).GetComponent<UILabel>();
        gameOverBG_back = gameOverBG.transform.GetChild(4).gameObject;

        gameSceneBG_score = gameSceneBG.transform.GetChild(0).gameObject;
        gameSceneBG_time = gameSceneBG.transform.GetChild(1).gameObject;
        gameSceneBG_white = gameSceneBG.transform.GetChild(2).gameObject;
        gameSceneBG_option = gameSceneBG.transform.GetChild(6).gameObject;
        gameSceneBG_hp = gameSceneBG.transform.GetChild(9).gameObject;
        gameSceneBG_scoreText = gameSceneBG.transform.GetChild(7).GetComponent<UILabel>();
        gameSceneBG_timeText = gameSceneBG.transform.GetChild(8).GetComponent<UILabel>();
        gameSceneBG_hpText = gameSceneBG.transform.GetChild(10).GetComponent<UILabel>();

        inGameOptionBG_back = inGameOptionBG.transform.GetChild(5).gameObject;
        inGameOptionBG_title = inGameOptionBG.transform.GetChild(4).gameObject;

        AddOnClick(startSceneBG_play, "OnPressedPlayInTitle");
        AddOnClick(startSceneBG_option, "OnPressedOptionInTitle");
        AddOnClick(startSceneBG_exit, "OnPressedExitInTitle");
        AddOnClick(optionBG_back, "OnPressedBackInOption");
        AddOnClick(exitBG_yes, "OnPressedYesInExit");
        AddOnClick(exitBG_no, "OnPressedNoInExit");
        AddOnClick(gameOverBG_back, "OnPressedBackInResult");
        AddOnClick(gameSceneBG_white, "OnPressedWhiteInGame");
        AddOnClick(inGameOptionBG_back, "OnPressedBackInGameOption");
        AddOnClick(inGameOptionBG_title, "OnPressedTitleInGameOption");
        AddOnClick(gameSceneBG_option, "OnPressedOptionInGame");

        beginBG.SetActive(false);
        exitBG.SetActive(false);
        gameOverBG.SetActive(false);
        optionBG.SetActive(false);
        startSceneBG.SetActive(false);
        gameSceneBG.SetActive(false);
        inGameOptionBG.SetActive(false);

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
        inGameOptionBG.SetActive(false);
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
        gameSceneBG_score.SetActive(false);
        gameSceneBG_time.SetActive(false);
        gameSceneBG_scoreText.gameObject.SetActive(false);
        gameSceneBG_timeText.gameObject.SetActive(false);
        gameSceneBG_option.SetActive(false);
        gameSceneBG_hp.SetActive(false);
        gameSceneBG_hpText.gameObject.SetActive(false);
    }
    private void OnPlay()
    {
        gameSceneBG_score.SetActive(true);
        gameSceneBG_time.SetActive(true);
        gameSceneBG_timeText.gameObject.SetActive(true);
        gameSceneBG_scoreText.gameObject.SetActive(true);
        gameSceneBG_option.SetActive(true);
        gameSceneBG_hp.SetActive(true);
        gameSceneBG_hpText.gameObject.SetActive(true);
    }
    private void OnPause()
    {

    }
    private void OnOptionPause()
    {
        inGameOptionBG.SetActive(true);
    }
    private void OnResume()
    {
        inGameOptionBG.SetActive(false);
    }
    private void OnGameOver()
    {
        gameSceneBG_white.SetActive(false);
        gameSceneBG.SetActive(false);
        gameOverBG.SetActive(true);
        GameOverScoreUpdate();
    }
    private void OnBackToTitle()
    {
        gameSceneBG_white.SetActive(false);
        gameSceneBG.SetActive(false);
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
        if (currentState == GameManagement.GameState.INIT)
        {
            if (titleFactor < TITLE_SPLESH_LENGTH)
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
        EnemyManagement.GetInstance().CallSkillEnabled();
        gameSceneBG_white.SetActive(false);
    }
    private void OnPressedOptionInGame()
    {
        gameManagement.SetCurrentState(GameManagement.GameState.PAUSE);
    }
    private void OnPressedBackInGameOption()
    {
        gameManagement.SetCurrentState(GameManagement.GameState.RESUME);
    }
    private void OnPressedTitleInGameOption()
    {
        gameManagement.SetCurrentState(GameManagement.GameState.BACK_TO_TITLE);
    }
    private void GameScoreUpdate()
    {
        if (currentState == GameManagement.GameState.PLAY)
        {
            gameSceneBG_scoreText.text = scoreManagement.GetStageScoreSum().ToString();
            gameSceneBG_timeText.text = scoreManagement.GetElapsedTime().ToString();
        }
    }
    private void PlayerHPUpdate()
    {
        if(currentState == GameManagement.GameState.PLAY)
        {
            gameSceneBG_hpText.text = (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInf>().GetCurrentHP() + 1).ToString();
        }
    }

    private void GameOverScoreUpdate()
    {
        gameOverBG_killTheEnemyText.text = scoreManagement.GetKilledEnemyCount().ToString();
        gameOverBG_scoreText.text = scoreManagement.GetStageScoreSum().ToString();
        gameOverBG_timeText.text = scoreManagement.GetElapsedTime().ToString();
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
