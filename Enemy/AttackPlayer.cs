using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : State
{
    public AttackPlayer(EnemyScript enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
       
    }

    public override void Exit()
    {
        base.Exit();
        enemy.AimToPlayer(false);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.attackCorutine == null)
        {
            enemy.attackCorutine = enemy.StartCoroutine(enemy.Attack());
        }
        if (enemy.dead)
        {
            stateMachine.ChangeState(enemy.death);
        }
        if (!enemy.CanAttackPlayer())
        {
            enemy.navMeshAgent.destination = enemy.target.position;
        }

    }

}
