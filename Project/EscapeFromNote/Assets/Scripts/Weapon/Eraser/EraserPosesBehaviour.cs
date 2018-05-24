using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserPosesBehaviour : MonoBehaviour {

    private Transform playerPos;

    private Vector3 direction;

    private const float OFFSET = 5f;
    
    public void SetPlayerPos(Transform position) { this.playerPos = position; }
	
	private void Start () {
		
	}
	private void LateUpdate () {
        EraserLookAtPlayer();
	}

    private void EraserLookAtPlayer()
    {
        transform.LookAt(playerPos.position, transform.up);
    }
}
