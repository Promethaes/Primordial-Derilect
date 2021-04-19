using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour
{
    NavMeshAgent agent;
    Enemy enemy;
    // change this to work for remote players as well
    PlayerManager playerManager;
    float attackCooldown;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var e = animator.gameObject;
        enemy = e.GetComponent<Enemy>();
        agent = enemy.navMeshAgent;

        playerManager = FindObjectOfType<PlayerManager>();
        attackCooldown = enemy.enemyData.attackSpeed;
        agent.isStopped = false;
        
    }


    Vector3 lastPos;
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ((playerManager.pRigidBody.position - lastPos).magnitude >= 1.0f * Time.deltaTime)
            agent.SetDestination(playerManager.pRigidBody.position);
        lastPos = playerManager.pRigidBody.position;
        if (attackCooldown > 0.0f)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        if (enemy.attackRangeBox.withinRange)
        {
            animator.SetBool("Chase", false);
            animator.SetBool("Attack", true);
            attackCooldown = enemy.enemyData.attackSpeed;
            return;
        }

        if (!enemy.sightRangeBox.withinRange)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Chase", false);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
