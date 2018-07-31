using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayer : MonoBehaviour
{

    private GameObject player;

    private NavMeshAgent agent;
    private Animator enemyAnimator;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsManager.Player);
        agent = gameObject.GetComponent<NavMeshAgent>();
        enemyAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyAnimator.SetFloat(EnemyControlsManager.EnemyVelocity, agent.velocity.magnitude);
        agent.SetDestination(player.transform.position);
    }
}
