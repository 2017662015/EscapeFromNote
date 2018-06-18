using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : Manager<GameManagement> {
    //Prefab Instances
    private GameObject prefab_walls;
    private GameObject prefab_background;

    //Enums
    public enum GameState
    {
        NULL = -2,
        INIT,
        TITLE,
        OPTION_TITLE,
        EXIT_TITLE,
        INIT_PLAY,
        PLAY,
        PAUSE,
        OPTION_PAUSE,
        RESUME,
        GAMEOVER,
        BACK_TO_TITLE,
        FINALIZE
    };

    //Instances
    private GameObject walls;
    private GameObject background;
    private Transform uiRoot;
    private Coroutine checkState;
    private StageManagement stageManagement;
    private EnemyManagement enemyManagement;
    private PlayerManagement playerManagement;
    private UIManagement uiManagment;
    private ItemManagement itemManagement;
    private ScoreManagement scoreManagement;
    private SoundManagement soundManagement;
    private UIRoot uiRootComp;

    //Variables
    private GameState currentState;
    private GameState previousState;

    //Constants
    public static readonly int DEVICE_SCREEN_HEIGHT = Screen.height;
    public static readonly int DEVICE_SCREEN_WIDTH = Screen.width;

    //Setter Methods
    public void SetCurrentState(GameState state) { currentState = state; }
    public GameState GetCurrentState() { return this.currentState; }
    
    private void Awake()
    {
        uiRoot = GameObject.Find("UI Root").transform;
        uiRootComp = uiRoot.GetComponent<UIRoot>();
    }
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
        prefab_walls = Resources.Load("Prefabs/Walls") as GameObject;
        prefab_background = Resources.Load("Prefabs/Background") as GameObject;
        
        currentState = GameState.INIT;
        previousState = GameState.NULL;
        stageManagement = StageManagement.GetInstance();
        playerManagement = PlayerManagement.GetInstance();
        enemyManagement = EnemyManagement.GetInstance();
        uiManagment = UIManagement.GetInstance();
        itemManagement = ItemManagement.GetInstance();
        scoreManagement = ScoreManagement.GetInstance();
        soundManagement = SoundManagement.GetInstance();
        StartCoroutine(CheckState());
    }

    //State Machine Callback Methods
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
        SpawnWall();
        SpawnBackground();
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
    private void OnExitTitle() { }
    private void OnResume()
    {

    }
    private void OnGameOver()
    {

    }
    private void OnBackToTitle()
    {
        DestroyWall();
        DestroyBackground();
        currentState = GameState.TITLE;
    }       
    private void OnFinalize()
    {
    }
    
    //Methods
    private void SpawnWall()
    {
        if (GameObject.Find("Walls") == null)
        {
            walls = Instantiate<GameObject>(prefab_walls, uiRoot);
            walls.name = "Walls";
        }
        else
        {
            Debug.LogError("GameObject 'Walls' is already spawned in the world!!!");
        }
    }
    private void DestroyWall()
    {
        if (walls != null)
        {
            Destroy(walls);
            walls = null;
        }
    }
    private void SpawnBackground()
    {
        if (GameObject.Find("Background") == null)
        {
            background = Instantiate<GameObject>(prefab_background, uiRoot);
            background.name = "Background";
        }
        else
        {
            Debug.LogError("GameObject 'Background' is already spawned in the world!!!");
        }
    }
    private void DestroyBackground()
    {
        if(background != null)
        {
            Destroy(background);
            background = null;
        }
    }

    //Coroutines
    private IEnumerator CheckState()
    {
        do
        {
            if (previousState != currentState)
            {
                previousState = currentState;
                stageManagement.SetCurrentState(currentState);
                scoreManagement.SetCurrentState(currentState);
                playerManagement.SetCurrentState(currentState);
                enemyManagement.SetCurrentState(currentState);
                uiManagment.SetCurrentState(currentState);
                itemManagement.SetCurrentState(currentState);
                soundManagement.SetCurrentState(currentState);
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
                    case GameState.EXIT_TITLE:
                        OnExitTitle();
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
                    case GameState.BACK_TO_TITLE:
                        OnBackToTitle();
                        break;
                    case GameState.FINALIZE:
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
