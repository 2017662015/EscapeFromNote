using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInf : Character {
    //Prefabs
    private GameObject prefab_eraser;
    private GameObject prefab_white;

    //Enums
    private enum PlayerWeapon { ERASER, WHITE };

    //Instances
    private GameObject white;
    private Transform weaponAxis;
    private Coroutine checkPlayerWeapon;
    private List<GameObject> eraser;
    private List<Transform> eraserPosAxises;
    private List<Transform> eraserPoses;

    //Variables
    private int currentEraserCount = 0;
    private int eraserSpaceCount = 0;
    private float whiteElapsedTime = 0.0f;
    private float eraserSpawnElapsedTime = 0.0f;
    private PlayerWeapon currentWeaponState;
    private PlayerWeapon previousWeaponState;
    
    //Constants
    private const int PLAYER_ERASER_COUNT_INIT = 3;
    private const int PLAYER_ERASER_COUNT_MAX = 4;
    private const int PLAYER_HP_INIT = 7;
    private const int PLAYER_SKILL_COUNT_INIT = 3;
    private const float PLAYER_ERASER_SPAWN_TIME = 10.0f;
    private const float PLAYER_WEAPON_ROTATION_SPEED = 100.0f;
    private const float PLAYER_WHITE_DURATION_TIME = 60.0f;

    //Unity Callback Methods
    private void OnEnable()
    {
        Init();
    }
    
    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);
        CheckAndGetPencilCase(coll);
        CheckHitByEnemy(coll);
    }

    //Initialze Method of Class
    protected override void Init()
    {
        base.Init();
        currentWeaponState = PlayerWeapon.ERASER;
        previousWeaponState = currentWeaponState;
        checkState = StartCoroutine(CheckState());
    }

    //State Machine Callback Methods
    private void OnEraserEquipped()
    {
        currentWeaponState = PlayerWeapon.ERASER;
    }
    private void OnWhiteEquipped()
    {
        currentWeaponState = PlayerWeapon.WHITE;
    }

    protected override void OnInit()
    {
        currentState = BehaviourState.IDLE;
    }
    protected override void OnIdle()
    {
        throw new NotImplementedException();
    }
    protected override void OnMove()
    {
        throw new NotImplementedException();
    }
    protected override void OnAttack()
    {
        throw new NotImplementedException();
    }
    protected override void OnDamaged()
    {
        hp--;
    }
    protected override void OnDie()
    {
        //TODO: 죽는동안 일어나는 행동들 더 적을것
        currentState = BehaviourState.FINALIZE;
    }
    protected override void OnGoToNextStage()
    {
        throw new NotImplementedException();
    }
    protected override void OnFinalize()
    {
        gameObject.SetActive(false);
    }

    //Methods
    private void EraserInit()
    {

    }
    private void CheckAndGetPencilCase(Collision2D coll)
    {
        if (coll.collider.CompareTag("PencilCase"))
        {
            IncreaseEraserSpaceCount();
        }
    }
    private void CheckHitByEnemy(Collision2D coll)
    {
        if(coll.collider.CompareTag("Enemy") || coll.collider.CompareTag("Bullet"))
        {
            if(hp > 0)
            {
                currentState = BehaviourState.DAMAGED;
            }
            else
            {
                currentState = BehaviourState.DIE;
            }
        }
    }
    private void IncreaseEraserSpaceCount() { eraserSpaceCount++; }
    private void DecreaseEraserSpaceCount() { eraserSpaceCount--; }
    private void RotateWeaponAxis()
    {
        if (currentState != BehaviourState.DIE)
        {
            Vector3 _rot = new Vector3(0, 0, PLAYER_WEAPON_ROTATION_SPEED * Time.fixedDeltaTime);
            weaponAxis.localRotation = Quaternion.Euler(_rot);
        }
    }
    

    //Coroutines
    protected override IEnumerator CheckState()
    {
        do
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
                case BehaviourState.GO_TO_NEXT_STAGE:
                    OnGoToNextStage();
                    break;
                case BehaviourState.FINALIZE:
                    OnFinalize();
                    break;
                default:
                    break;
            }
            yield return null;
        } while (currentState != BehaviourState.FINALIZE);
    }
    private IEnumerator CheckPlayerWeapon()
    {
        do
        {
            if (currentWeaponState != previousWeaponState)
            {
                previousWeaponState = currentWeaponState;
                switch (currentWeaponState)
                {
                    case PlayerWeapon.ERASER:
                        OnEraserEquipped();
                        break;
                    case PlayerWeapon.WHITE:
                        OnWhiteEquipped();
                        break;
                    default:
                        break;
                }
            }
            yield return new WaitForEndOfFrame();
        }while (currentState != BehaviourState.DIE);
    }
}
