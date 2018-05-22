using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInf : Character
{

    //Prefab Instances
    private GameObject prefab_eraser;

    //Instance
    private Coroutine checkState;

    private List<GameObject> erasers = new List<GameObject>();
    private List<Transform> eraserPoses = new List<Transform>();

    //Variables
    private int skillCount;
    private int eraserCount;

    //Constants 
    private readonly int PLAYER_HP = 7;
    private readonly int PLAYER_SKILLCNT = 3;
    private readonly int PLAYER_ERASERINIT = 3;
    private readonly int PLAYER_ERASERMAX = 4;
    private readonly float MOVE_SPEED = 1.5f;
    private readonly float ITEM_REMAIN_LENGTH = 60.0f;

    //Unity Callback Methods
    private void OnEnable()
    {
        Init();
    }
    protected override void OnCollisionEnter(Collision coll)
    {
        base.OnCollisionEnter(coll);
        //TODO: 추후 충돌처리 구현 예정
        if (hp <= 0)
        {
            currentState = BehaviourState.DIE;
        }
        else
        {
            currentState = BehaviourState.DAMAGED;
        }
    }

    //Initialize Method of this script
    private void Init()
    {
        prefab_eraser = Resources.Load("Prefabs/Eraser") as GameObject;
        currentState = BehaviourState.INIT;
        previousState = BehaviourState.DIE;
        checkState = StartCoroutine(CheckState());
    }

    //State Machine Callbacks
    protected override void OnInit()
    {
        //TODO: Player 생성시 동작할 Function을 구현해 주세요.
        hp = PLAYER_HP;
        EraserInit();
    }
    protected override void OnIdle()
    {
        //TODO: Player가 Idle 상태일때 동작할 Function을 구현해주세요.
    }
    protected override void OnMove()
    {
        //TODO: Player가 Move 상태일때 동작할 Function을 구현해주세요.
    }
    protected override void OnAttack()
    {
        //TODO: Player가 Attack상태일때 동작할 Function을 구현해주세요.
    }
    protected override void OnDamaged()
    {
        //TODO: Player가 Damage를 받은 상태일때 동작할 Function을 구현해주세요.
    }
    protected override void OnDie()
    {
        //TODO: Player가 Die상태일때 동작할 Function을 구현해주세요.   
    }

    //Methods
    private void EraserInit()
    {
        do
        {
            erasers.Add(AddEraser());
            eraserCount++;
        } while (erasers.Count != PLAYER_ERASERMAX);
        erasers[PLAYER_ERASERINIT].SetActive(false);
    }
    private GameObject AddEraser()
    {
        return Instantiate<GameObject>(prefab_eraser);
    }
    
    //Coroutines
    protected override IEnumerator CheckState()
    {
        do
        {
            if (previousState != currentState)
            {
                previousState = currentState;
                
                switch (currentState)
                {
                    case BehaviourState.INIT:
                        OnInit();
                        break;
                    case BehaviourState.IDLE:
                        OnIdle();
                        break;
                    case BehaviourState.MOVE:
                        OnMove();
                        break;
                    case BehaviourState.ATTACK:
                        OnAttack();
                        break;
                    case BehaviourState.DAMAGED:
                        OnDamaged();
                        break;
                    case BehaviourState.DIE:
                        OnDie();
                        break;
                }
            }
            yield return new WaitForEndOfFrame();
        } while (true);
    }
}
