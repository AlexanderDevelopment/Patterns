using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyScript : MonoBehaviour
{
    private StateMachine movementSM;
    private AttackPlayer attackPlayer;
    private Patrol patrol;
    private Death death;
    

    void Start()
    {
        movementSM = new StateMachine();
        attackPlayer = new AttackPlayer(this, movementSM);
        patrol = new Patrol(this, movementSM);
        death = new Death(this, movementSM);
        movementSM.Initialize(patrol);
    }


    void Update()
    {
        movementSM.CurrentState.LogicUpdate();
    }
}
