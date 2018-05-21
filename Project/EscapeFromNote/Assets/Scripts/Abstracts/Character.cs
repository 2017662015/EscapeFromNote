using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

public abstract class Character : MonoBehaviour
{
    [HideInInspector] public enum BehaviourState { INIT = -1, IDLE, MOVE, ATTACK, DAMAGED, DIE };

    //Variables
    protected int hp;
    protected BehaviourState currentState;
    protected BehaviourState previousState;

    //Methods
    protected abstract void OnInit();
    protected abstract void OnIdle();
    protected abstract void OnMove();
    protected abstract void OnAttack();
    protected abstract void OnDamaged();
    protected abstract void OnDie();

    protected virtual void OnCollisionEnter(Collision coll) { }

    //Coroutines
    protected abstract IEnumerator CheckState();
}