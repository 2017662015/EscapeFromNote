﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMove : MonoBehaviour
{

    public enum InputType { KBDMOUSE, TOUCH };

    private float inputX;
    private float inputY;
    private float mouseX;
    private float mouseY;
    private InputType currentInputType;
    private Character.BehaviourState currentState;

    private readonly float MAX_SCREEN_RES_WIDTH, MIN_SCREEN_RES_WIDTH;
    private readonly float MAX_SCREEN_RES_HEIGHT, MIN_SCREEN_RES_HEIGHT;

    public void SetCurrentInputType(InputType type) { this.currentInputType = type; }
    public void SetCurrentState(Character.BehaviourState state) { this.currentState = state; }

    private void Init()
    {
        currentState = Character.BehaviourState.INIT;
    }


    private void Update()
    {
        if (currentState != Character.BehaviourState.DIE)
        {
            GetInput();
        }
    }

    private void GetInput()
    {
        switch (currentInputType)
        {
            case InputType.KBDMOUSE:
                inputX = Input.GetAxis("Horizontal") * Time.deltaTime;
                inputY = Input.GetAxis("Vertical") * Time.deltaTime;
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
                MovePlayer();
                break;
            case InputType.TOUCH:
                break;
        }
    }

    private void MovePlayer()
    {
        Vector3 _dir = new Vector3(inputX, inputY);
        gameObject.transform.position += _dir;   
    }
}
