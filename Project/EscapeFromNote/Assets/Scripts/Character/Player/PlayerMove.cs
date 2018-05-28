using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //Prefab Instances
    //TODO: Need to move to UI Manager when UI is completed
    private GameObject prefab_inputArea;

    //Instances
    private GameObject inputArea;
    private GameObject inputBtn;
    private Transform uiRoot;
    private Rigidbody2D rb2D;
    private PlayerInf playerInf;

    //Variables
    private bool isFingerPressed;
    [SerializeField][Range(0.0f, 200.0f)]private float moveSpeed = 100.0f;
    private float inputDirMag;
    private Vector2 initFingerPos;
    private Vector2 currentFingerPos;
    private Vector2 inputDir;
    private Character.BehaviourState currentState;
    private GameManagement.GameState currentGameState;

    //Constants
    private const float INPUT_AREA_RADIUS = 75.0f;
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
    private void FixedUpdate()
    {
        MovePlayer();
    }

    //Initialize Methods of Class
    private void Init()
    {
        prefab_inputArea = Resources.Load("Prefabs/InputArea") as GameObject;
        uiRoot = GameObject.Find("UI Root").transform;
        inputArea = Instantiate(prefab_inputArea, uiRoot);
        inputBtn = inputArea.transform.GetChild(0).gameObject;
        inputArea.SetActive(false);
        currentState = Character.BehaviourState.INIT;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
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
                    inputArea.SetActive(true);
                    inputArea.transform.position = Camera.main.ScreenToWorldPoint(initFingerPos); 
                    isFingerPressed = true;
                }
                if (Input.GetMouseButton(0) && isFingerPressed)
                {
                    currentFingerPos = Input.mousePosition;
                    inputDir = currentFingerPos - initFingerPos;
                    inputDir.x = Mathf.Clamp(inputDir.x, -INPUT_AREA_RECT_LENGTH, INPUT_AREA_RECT_LENGTH);
                    inputDir.y = Mathf.Clamp(inputDir.y, -INPUT_AREA_RECT_LENGTH, INPUT_AREA_RECT_LENGTH);
                    inputBtn.transform.localPosition = inputDir;
                    inputDirMag = inputDir.magnitude;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    initFingerPos = Vector3.zero;
                    currentFingerPos = Vector3.zero;
                    inputDir = Vector3.zero;
                    inputDirMag = 0.0f;
                    inputArea.SetActive(false);
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
                            inputArea.SetActive(true);
                            inputArea.transform.position = Camera.main.ScreenToWorldPoint(initFingerPos);
                            isFingerPressed = true;
                            break;
                        case TouchPhase.Stationary: 
                        case TouchPhase.Moved:
                            currentFingerPos = Input.GetTouch(0).position;
                            inputDir = currentFingerPos - initFingerPos;
                            inputDir.x = Mathf.Clamp(inputDir.x, -INPUT_AREA_RECT_LENGTH, INPUT_AREA_RECT_LENGTH);
                            inputDir.y = Mathf.Clamp(inputDir.y, -INPUT_AREA_RECT_LENGTH, INPUT_AREA_RECT_LENGTH);
                            inputBtn.transform.localPosition = inputDir;
                            inputDirMag = inputDir.magnitude;
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                        default:
                            initFingerPos = Vector3.zero;
                            currentFingerPos = Vector3.zero;
                            inputDir = Vector3.zero;
                            inputDirMag = 0.0f;
                            inputArea.SetActive(false);
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
                rb2D.position += ((inputDir / INPUT_AREA_RADIUS) * Time.deltaTime * moveSpeed) * Time.fixedDeltaTime;
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
