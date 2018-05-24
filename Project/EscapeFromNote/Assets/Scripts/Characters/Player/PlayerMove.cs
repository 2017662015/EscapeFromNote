using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMove : MonoBehaviour
{

    public enum InputType { KBDMOUSE, TOUCH };

    private PlayerInf playerInf;

    private float inputX;
    private float inputY;
    private float posX;
    private float posY;
    private float mouseX;
    private float mouseY;


    private InputType currentInputType;
    private Character.BehaviourState currentState;

    private readonly float MAX_SCREEN_RES_WIDTH =  360, MIN_SCREEN_RES_WIDTH = -360;
    private readonly float MAX_SCREEN_RES_HEIGHT = 640, MIN_SCREEN_RES_HEIGHT = -640;

    public void SetCurrentInputType(InputType type) { this.currentInputType = type; }
    public void SetCurrentState(Character.BehaviourState state) { this.currentState = state; }

    private Animator anim;

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        currentState = Character.BehaviourState.INIT;
        playerInf = gameObject.GetComponent<PlayerInf>();
        anim = gameObject.GetComponent<Animator>();
    }


    private void Update()
    {
        if (currentState != Character.BehaviourState.DIE)
        {
            GetInput();
        }
    }
    private void FixedUpdate()
    {
        if (currentState != Character.BehaviourState.DIE && currentState != Character.BehaviourState.DAMAGED)
        {
            if (inputX != 0 || inputY != 0)
            {
                playerInf.SetCurrentState(Character.BehaviourState.MOVE);
            }
            else
            {
                playerInf.SetCurrentState(Character.BehaviourState.IDLE);
            }
        }
    }

    private void GetInput()
    {
        switch (currentInputType)
        {
            case InputType.KBDMOUSE:
                inputX = Input.GetAxis("Horizontal") * Time.deltaTime;
                anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
                inputY = Input.GetAxis("Vertical") * Time.deltaTime;
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
                posX += inputX;
                posY += inputY;
                float posXLimit = Camera.main.ScreenToWorldPoint(new Vector3(720, 0, 0)).x;
                float posYLimit = Camera.main.ScreenToWorldPoint(new Vector3(0, 1280, 0)).y;
                posX = Mathf.Clamp(posX, -posXLimit, posXLimit);
                posY = Mathf.Clamp(posY, -posYLimit, posYLimit);
                MovePlayer();
                break;
            case InputType.TOUCH:
                break;
        }
    }

    private void MovePlayer()
    {
        Vector2 _dir = new Vector2(posX, posY);
        
        gameObject.transform.position = _dir;
        Debug.Log(gameObject.transform.position.x);
    }
}
