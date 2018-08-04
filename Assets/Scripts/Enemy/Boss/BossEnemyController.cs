using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(JumpOnTarget))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BossEnemyStats))]
public class BossEnemyController : MonoBehaviour
{
    [Header("General Stats")]
    public float rotationSpeed = 0.7f;
    public int maxHealth = 500;
    public float fallThreshold = -1f;
    public int minTimeCountBetweenStateChange = 3;
    public int maxTimeCountBetweenStateChange = 7;

    [Header("Effects")]
    public GameObject deathEffect;
    public GameObject jumpingRing;
    public GameObject jumpingBalls;
    public GameObject missile;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator enemyAnimator;
    private Rigidbody enemyRB;

    private JumpOnTarget jumpOnTarget;
    private BossEnemyStats enemyStats;

    private float currentHealth;
    private float currentTimeStateChange;

    private enum EnemyState
    {
        Idle,
        Moving,
        Jumping,
        Falling,
        Dead,
        RingAttack,
        BallAttack,
        MissileAttack,
    }
    private EnemyState currentState;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsManager.Player);

        agent = gameObject.GetComponent<NavMeshAgent>();
        enemyAnimator = gameObject.GetComponent<Animator>();
        enemyRB = gameObject.GetComponent<Rigidbody>();

        enemyStats = gameObject.GetComponent<BossEnemyStats>();
        jumpOnTarget = gameObject.GetComponent<JumpOnTarget>();

        currentHealth = maxHealth;
        enemyStats.isJumping = false;
        currentTimeStateChange = 0;

        currentState = EnemyState.Idle;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (currentHealth <= 0 && currentState != EnemyState.Dead)
            currentState = EnemyState.Dead;

        switch (currentState)
        {
            case EnemyState.Idle:
                // Change to a Different State
                break;

            case EnemyState.Moving:
                MoveEnemyTowardsPlayer();
                break;

            case EnemyState.Jumping:
                MakeEnemyJump();
                break;

            case EnemyState.Falling:
                // Don't do anything. Player Falling
                break;

            case EnemyState.Dead:
                // Don't do anything. Player Dead
                break;

            case EnemyState.RingAttack:
                AttackWithRings();
                break;

            case EnemyState.BallAttack:
                AttackWithBalls();
                break;

            case EnemyState.MissileAttack:
                AttackWithMissiles();
                break;
        }

        currentTimeStateChange += Time.deltaTime;
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (!enemyStats.isJumping)
            return;

        if (!other.gameObject.CompareTag(TagsManager.Player) &&
            !other.gameObject.CompareTag(TagsManager.Ground))
            return;

        currentState = EnemyState.Idle;
        enemyAnimator.SetBool(EnemyControlsManager.BossFallingParam, false);
    }

    void DoStuffWhilePlayerIdle()
    {

    }

    void MakeEnemyJump()
    {
        if (currentState == EnemyState.Jumping || currentState == EnemyState.Jumping)
            return;

        currentState = EnemyState.Jumping;
        jumpOnTarget.TriggerJump();
    }

    void MoveEnemyTowardsPlayer()
    {
        enemyAnimator.SetFloat(EnemyControlsManager.EnemyVelocity, agent.velocity.magnitude);
        agent.SetDestination(player.transform.position);
    }

    void AttackWithRings()
    {

    }

    void AttackWithBalls()
    {

    }

    void AttackWithMissiles()
    {

    }

    void MakePlayerFall()
    {
        float yVelocity = enemyRB.velocity.y;
        if (yVelocity < fallThreshold && currentState != EnemyState.Falling)
        {
            enemyAnimator.SetBool(EnemyControlsManager.BossFallingParam, true);
            currentState = EnemyState.Falling;
        }
    }

}
