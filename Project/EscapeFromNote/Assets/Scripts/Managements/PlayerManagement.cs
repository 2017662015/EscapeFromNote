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

    public void SpawnPlayer(Transform spawnPos)
    {
        if (!player)
        {
            player = Instantiate<GameObject>(prefab_player, spawnPos.position, spawnPos.rotation);
        }
        else
        {
            Debug.Log("Player is already spawned in the world!");
        }
    }
}
