using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : Manager<PlayerManagement> {
    //Prefab Instances
    private GameObject prefab_player;

    //Instances
    private GameObject player;
    private Transform uiRoot;

    //Variables
    private GameManagement.GameState currentState;
    private GameManagement.GameState previousState;

    //Setter Methods
    public void SetCurrentState(GameManagement.GameState state) { this.currentState = state; }

    //Unity Callback Method
    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    //Initialize Method of Class
    private void Init()
    {
        prefab_player = Resources.Load("Prefabs/Player") as GameObject;
        uiRoot = GameObject.Find("UI Root").transform;
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
                        break;
                    case GameManagement.GameState.BACK_TO_TITLE:
                        DestroyPlayer();
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
