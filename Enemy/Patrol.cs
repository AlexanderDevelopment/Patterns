using UnityEngine;

public class Patrol : State
{
    public Patrol(EnemyScript enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        enemy.SetNavMeshPoint();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.WayPointComplete())
        {
            enemy.SetNavMeshPoint();
        }
        if (enemy.dead)
        {
            stateMachine.ChangeState(enemy.death);
        }
        if (enemy.CanAttackPlayer())
        {
            stateMachine.ChangeState(enemy.attackPlayer);
        }

    }
    public override void Exit()
    {
        base.Exit();
    }
}
