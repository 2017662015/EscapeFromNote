using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>PlayerManagement는 Player Object를 관리하는 Class입니다.</summary>
public class PlayerManagement : MonoBehaviour
{

    //Instance of this class by Singleton Pattern
    private static PlayerManagement instance;

    //Getter Method of instance of this class
    public static PlayerManagement GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.Find("PlayerManagement").GetComponent<PlayerManagement>();
        }
        return instance;
    }

    private GameObject prefab_player;

    private static GameObject player;
    private static Transform PlayerSpawnPos;
    

    private static GameManagement.GameState mGameState;
    private static GameManagement.GameState previousGameState;

    public static void SetMGameState(GameManagement.GameState state) { mGameState = state; }

    public void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        prefab_player = Resources.Load("Prefabs/Player") as GameObject;
        PlayerSpawnPos = GameObject.Find("PlayerSpawnPos").transform;
        previousGameState = GameManagement.GameState.NULL;
        mGameState = GameManagement.GameState.TITLE;
        StartCoroutine(CheckState());
    }

    public void SpawnPlayer(Transform spawnPos)
    {
        if (!player)
        {
            player = Instantiate<GameObject>(prefab_player, spawnPos.position, spawnPos.rotation, spawnPos);
            player.name = "Player";
        }
        else
        {
            Debug.Log("Player is already spawned in the world!");
        }
    }

    private IEnumerator CheckState()
    {
        do
        {
            if(previousGameState != mGameState)
            {
                previousGameState = mGameState;
                switch (mGameState)
                {
                    case GameManagement.GameState.NULL:
                        break;
                    case GameManagement.GameState.INIT:
                        break;
                    case GameManagement.GameState.TITLE:
                        break;
                    case GameManagement.GameState.OPTION:
                        break;
                    case GameManagement.GameState.PLAY:
                        SpawnPlayer(PlayerSpawnPos);
                        break;
                    case GameManagement.GameState.PAUSE:
                        break;
                    case GameManagement.GameState.RESUME:
                        break;
                    case GameManagement.GameState.INGAMEOPTION:
                        break;
                    case GameManagement.GameState.GAMEOVER:
                        break;
                    case GameManagement.GameState.FINALIZE:
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        } while (true);
    }
}
