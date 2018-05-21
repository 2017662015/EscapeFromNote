using System.Collections;
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
                inputX = Input.GetAxis("Horizontal");
                inputY = Input.GetAxis("Vertical");
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
                break;
            case InputType.TOUCH:
                break;
        }
    }

    private void MovePlayer()
    {

    }

    private float[ , ] GetResolution()
    {
        float _MAX_SCREEN_RES_HEIGHT;
        float _MIN_SCREEN_RES_HEIGHT;
        float _MAX_SCREEN_RES_WIDTH;
        float _MIN_SCREEN_RES_WIDTH;
        Resolution[] _resolution = Screen.resolutions;
        if(_resolution.Length == 1)
        {
            _MAX_SCREEN_RES_HEIGHT = _resolution[0].height / 2;
            _MIN_SCREEN_RES_HEIGHT = -_resolution[0].height / 2;
            _MAX_SCREEN_RES_WIDTH = _resolution[0].width / 2;
            _MIN_SCREEN_RES_WIDTH = -_resolution[0].height / 2;
            float[, ] output = new float[2, 2] {
            { _MAX_SCREEN_RES_HEIGHT,
            _MIN_SCREEN_RES_HEIGHT, },
            { _MAX_SCREEN_RES_WIDTH,
            _MIN_SCREEN_RES_WIDTH }
            };
            return output;
        }
        else
        {
            throw new System.ArgumentOutOfRangeException();
        }
    }
}
