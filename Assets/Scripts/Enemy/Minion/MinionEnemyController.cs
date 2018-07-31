using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionEnemyController : MonoBehaviour
{
    public float animationWaitTime = 1.6f;
    public float rotationRate = 0.7f;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator enemyAnimator;

    private bool coroutinePlaying;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsManager.Player);
        agent = gameObject.GetComponent<NavMeshAgent>();
        enemyAnimator = gameObject.GetComponent<Animator>();

        coroutinePlaying = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!agent.pathPending && !coroutinePlaying)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
        }

        bool enemyDead = enemyAnimator.GetBool(EnemyControlsManager.EnemyDead);

        if (!enemyDead)
        {
            Vector3 lookPosition = player.transform.position - gameObject.transform.position;
            lookPosition.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
                rotation, rotationRate * Time.deltaTime);

            enemyAnimator.SetFloat(EnemyControlsManager.EnemyVelocity, agent.velocity.magnitude);
            agent.SetDestination(player.transform.position);
        }
    }

    IEnumerator AttackPlayer()
    {
        coroutinePlaying = true;
        enemyAnimator.SetTrigger(EnemyControlsManager.MinionAttack);
        yield return new WaitForSeconds(animationWaitTime);
        coroutinePlaying = false;
    }



    void FootL()
    {
        // For now Ignore Sounds
    }

    void FootR()
    {
        // For now Ignore Sounds
    }
}
