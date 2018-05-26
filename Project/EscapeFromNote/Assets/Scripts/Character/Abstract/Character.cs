using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {
    //Enums
    [HideInInspector] public enum BehaviourState { NULL = -2, INIT, IDLE, MOVE, ATTACK, SKILL, DAMAGED, DIE, GO_TO_NEXT_STAGE, FINALIZE };

    //Instances
    protected Coroutine checkState; 

    //Variables
    protected int hp;
    protected float moveSpeed;
    protected BehaviourState currentState;
    protected BehaviourState previousState;

    //Setter Methods
    protected internal void SetCurrentState(BehaviourState state) { this.currentState = state; }

    //Unity Callback Methods
    protected virtual void OnCollisionEnter2D(Collision2D coll) { }

    //Initialize Method of Class
    protected virtual void Init()
    {
        currentState = BehaviourState.INIT;
        previousState = BehaviourState.NULL;
    }

    //State Machine Callback Methods
    protected abstract void OnInit();
    protected abstract void OnIdle();
    protected abstract void OnMove();
    protected abstract void OnAttack();
    protected abstract void OnSkill();
    protected abstract void OnDamaged();
    protected abstract void OnDie();
    protected abstract void OnGoToNextStage();
    protected abstract void OnFinalize();

    //Coroutines
    protected abstract IEnumerator CheckState();
}
