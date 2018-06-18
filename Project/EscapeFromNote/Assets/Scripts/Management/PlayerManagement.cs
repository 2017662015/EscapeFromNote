using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : Manager<PlayerManagement> {
    //Prefab Instances
    private GameObject prefab_player;

    //Instances
    private GameManagement gameManagement;
    private GameObject player;
    private Transform uiRoot;

    //Variables
    private float delayTime = 0.0f;
    private GameManagement.GameState currentState;
    private GameManagement.GameState previousState;

    //Constants
    private const float DELAY_INTERVAL_AFTER_SPAWN = 5.0f;

    //Getter Methods
    public GameManagement.GameState GetCurrentState() { return this.currentState; }

    //Setter Methods
    public void SetCurrentState(GameManagement.GameState state) { this.currentState = state; }

    //Unity Callback Method
    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }
    private void Update()
    {
        WaitingToStart();
    }
    //Initialize Method of Class
    private void Init()
    {
        prefab_player = Resources.Load("Prefabs/Player") as GameObject;
        uiRoot = GameObject.Find("UI Root").transform;
        gameManagement = GameManagement.GetInstance();
        currentState = GameManagement.GameState.INIT;
        previousState = GameManagement.GameState.NULL;
        StartCoroutine(CheckState());
    }

    //Methods
    private void SpawnPlayer()
    {
        if(!GameObject.Find("Player"))
        {
            player = Instantiate<GameObject>(prefab_player, uiRoot);
            player.name = "Player";
        }
        else
        {
            Debug.LogError("GameObject 'Player' is already spawned in the world!!");
        }
    }
    private void SpawnPlayer(Transform spawnPos)
    {
        if (!GameObject.Find("Player"))
        {
            player = Instantiate<GameObject>(prefab_player, spawnPos);
            player.name = "Player";
        }
        else
        {
            Debug.LogError("GameObject 'Player' is already spawned in the world!!");
        }
    }
    private void DestroyPlayer()
    {
        if(player != null)
        {
            Destroy(player);
            player = null;
        }
    }
    private void WaitingToStart()
    {
        if (currentState == GameManagement.GameState.INIT_PLAY && player != null)
        {
            if (delayTime < DELAY_INTERVAL_AFTER_SPAWN)
            {
                delayTime += Time.deltaTime;
            }
            else
            {
                gameManagement.SetCurrentState(GameManagement.GameState.PLAY);
                delayTime = 0.0f;
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
                    case GameManagement.GameState.NULL:
                        break;
                    case GameManagement.GameState.INIT:
                        break;
                    case GameManagement.GameState.TITLE:
                        break;
                    case GameManagement.GameState.OPTION_TITLE:
                        break;
                    case GameManagement.GameState.INIT_PLAY:
                        SpawnPlayer();
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
                        DestroyPlayer();
                        break;
                    case GameManagement.GameState.BACK_TO_TITLE:
                        break;
                    default:
                        break;
                }
                if(player != null)
                {
                    player.GetComponent<PlayerMove>().SetCurrentGameState(currentState);
                }
            }
            yield return null;
        } while(currentState != GameManagement.GameState.FINALIZE);
    }
}
