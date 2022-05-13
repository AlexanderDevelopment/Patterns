using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : State
{
    public Death(EnemyScript enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        if (enemy.deathCorutine == null)
        {
            enemy.deathCorutine = enemy.StartCoroutine(enemy.DeathEnemy());
        }
    }

    public override void Exit()
    {
    }
    public override void LogicUpdate()
    {
    }

}
