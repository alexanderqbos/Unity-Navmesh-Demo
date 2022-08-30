using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;

    private enum EnemyState { IDLE, CHASE, ATTACK };
    private EnemyState state;

    private float distanceToTarget = float.MaxValue;
    private float chaseRange = 10.0f;
    private float attackRange = 3.0f;

    void SetState( EnemyState newState)
    {
        state = newState;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(EnemyState.IDLE);
    }

    void Update_Attacking()
    {
        agent.isStopped = true;
        Debug.Log("Attacking");
        if(distanceToTarget > attackRange)
        {
            SetState(EnemyState.CHASE);
        }
    }

    void Update_Idle()
    {
        agent.isStopped = true;
        if(distanceToTarget <= chaseRange)
        {
            SetState(EnemyState.CHASE);
        }
    }

    void Update_Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
        if(distanceToTarget > chaseRange)
        {
            SetState(EnemyState.IDLE);
        } else if (distanceToTarget <= attackRange)
        {
            SetState(EnemyState.ATTACK);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        switch(state)
        {
            case EnemyState.IDLE: Update_Idle(); break;
            case EnemyState.CHASE: Update_Chase(); break;
            case EnemyState.ATTACK: Update_Attacking(); break;
        }
    }
}
