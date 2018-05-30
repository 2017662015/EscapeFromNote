﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPenMove : EnemyBehaviour {
    //Method Callbacks
    protected override void Init()
    {
        base.Init();
        prefab_bullet = Resources.Load("Prefabs/BulletBallPen") as GameObject;
        bulletShotDelay = 2.0f;
    }
    protected override void OnInit()
    {
        base.OnInit();
        currentState = BehaviourState.IDLE;
    }
    protected override void OnIdle()
    {
        base.OnIdle();
    }
    protected override void OnAttack()
    {
        base.OnAttack();
    }
}
