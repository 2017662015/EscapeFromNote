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
    [SerializeField][Range(0.0f, 300.0f)]private float moveSpeed = 100.0f;
    private float inputDirMag;
    private Vector3 initFingerPos;
    private Vector3 currentFingerPos;
    private Vector3 inputDir;
    private Character.BehaviourState currentState;
    private GameManagement.GameState currentGameState;

    //Constants
    [SerializeField]private const float INPUT_AREA_RADIUS = 50.0f;
    private readonly float INPUT_AREA_RECT_LENGTH = INPUT_AREA_RADIUS * Mathf.Cos(45.0f * Mathf.Deg2Rad);

    //Setter Methods
    public void SetCurrentState(Character.BehaviourState state) { this.currentState = state; }
    public void SetCurrentGameState(GameManagement.GameState state) { this.currentGameState = state; }
    
    //Unity Callback Methods
    private void OnEnable()
    {
        Init();
    }
    private void Update()
    {
        GetInput();
    }
    private void LateUpdate()
    {
        MovePlayer();
    }

    //Initialize Methods of Class
    private void Init()
    {
        currentState = Character.BehaviourState.INIT;
        playerInf = gameObject.GetComponent<PlayerInf>();
    }

    //Methods
    private void GetInput()
    {
        //TODO: This code was hardcorded. Need to be refactored.
        #region //For Unity Editor
            #if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0))
                {
                    initFingerPos = Input.mousePosition;
                    isFingerPressed = true;
                }
                if (Input.GetMouseButton(0) && isFingerPressed)
                {
                    currentFingerPos = Input.mousePosition;
                    inputDir = currentFingerPos - initFingerPos;
                    inputDir.x = Mathf.Clamp(inputDir.x, -INPUT_AREA_RECT_LENGTH, INPUT_AREA_RECT_LENGTH);
                    inputDir.y = Mathf.Clamp(inputDir.y, -INPUT_AREA_RECT_LENGTH, INPUT_AREA_RECT_LENGTH);
                    inputDirMag = inputDir.magnitude;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    initFingerPos = Vector3.zero;
                    currentFingerPos = Vector3.zero;
                    inputDir = Vector3.zero;
                    inputDirMag = 0.0f;
                    isFingerPressed = false;
                }
            #endif
        #endregion
        #region //For Android and iOS
        #if UNITY_ANDROID || UNITY_IPHONE || UNITY_IPAD 
                if(Input.touchCount > 0)
                {
                    switch (Input.GetTouch(0).phase)
                    {
                        case TouchPhase.Began:
                            initFingerPos = Input.GetTouch(0).position;
                            break;
                        case TouchPhase.Stationary: 
                        case TouchPhase.Moved:
                            currentFingerPos = Input.GetTouch(0).position;
                            inputDir = currentFingerPos - initFingerPos;
                            inputDir.x = Mathf.Clamp(inputDir.x, -INPUT_AREA_RECT_LENGTH, INPUT_AREA_RECT_LENGTH);
                            inputDir.y = Mathf.Clamp(inputDir.y, -INPUT_AREA_RECT_LENGTH, INPUT_AREA_RECT_LENGTH);
                            inputDirMag = inputDir.magnitude;
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                        default:
                            initFingerPos = Vector3.zero;
                            currentFingerPos = Vector3.zero;
                            inputDir = Vector3.zero;
                            inputDirMag = 0.0f;
                            isFingerPressed = false;
                            break;
                    }
                }
        #endif
        #endregion
    }
    private void MovePlayer()
    {
        if(currentGameState == GameManagement.GameState.PLAY && 
            currentState != Character.BehaviourState.INIT && currentState != Character.BehaviourState.DIE)
        {
            if(isFingerPressed)
            {
                transform.localPosition += ((inputDir / INPUT_AREA_RADIUS) * moveSpeed) * Time.deltaTime;
                if(currentState != Character.BehaviourState.DAMAGED)
                {
                    currentState = Character.BehaviourState.MOVE;
                    playerInf.SetCurrentState(currentState);
                }
            }
            else
            {
                if (currentState != Character.BehaviourState.DAMAGED)
                {
                    currentState = Character.BehaviourState.IDLE;
                    playerInf.SetCurrentState(currentState);
                }
            }
        }
    }
}
