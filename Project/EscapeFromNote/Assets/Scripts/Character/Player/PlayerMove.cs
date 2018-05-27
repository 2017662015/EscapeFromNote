using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{ 
    //Instances
    private PlayerInf playerInf;

    //Variables
    private bool isFingerPressed;
    [SerializeField][Range(30.0f, 100.0f)]private float moveSpeed = 30.0f;
    private Vector3 initFingerPos;
    private Vector3 currentFingerPos;
    private Vector3 moveDir;
    private Character.BehaviourState currentState;
    private GameManagement.GameState currentGameState;

    //Setter Methods
    public void SetCurrentState(Character.BehaviourState state) { this.currentState = state; }
    public void SetCurrentGameState(GameManagement.GameState state) { this.currentGameState = state; }
    
    //Unity Callback Methods
    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        currentState = Character.BehaviourState.INIT;
        playerInf = gameObject.GetComponent<PlayerInf>();
    }
    private void Update()
    {
        GetInput();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        if(currentGameState == GameManagement.GameState.PLAY)
        {
            #if UNITY_ANDROID || UNITY_IPHONE
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    initFingerPos = Input.GetTouch(0).position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
                {
                    initFingerPos = Vector3.zero;
                }
            }
            #endif
            #if UNITY_EDITOR
            if(Input.GetMouseButtonDown(0))
            {
                isFingerPressed = true;
                initFingerPos = Input.mousePosition;
            }
            if(Input.GetMouseButton(0))
            {
                currentFingerPos = Input.mousePosition;
                moveDir = currentFingerPos - initFingerPos;
            }
            if(Input.GetMouseButtonUp(0))
            {
                isFingerPressed = false;
                initFingerPos = Vector3.zero;
                currentFingerPos = Vector3.zero;
                moveDir = Vector3.zero;
            }
            #endif
        }
    }
    private void MovePlayer()
    {
        if(currentGameState == GameManagement.GameState.PLAY)
        {
            if(isFingerPressed)
            {
                transform.localPosition += moveDir * Time.deltaTime * moveSpeed;
            }
        }
    }
}
