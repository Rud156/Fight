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
    public float rotationRate = 0.7f;
    public int maxHealth = 5000;
    public float fallThreshold = -1f;
    public float waitTimeBetweenAttacks = 0.5f;
    public int minTimeCountBetweenStateChange = 3;
    public int maxTimeCountBetweenStateChange = 7;

    [Header("Effects")]
    public GameObject deathEffect;
    public GameObject jumpingRing;
    public GameObject jumpingBalls;
    public GameObject missile;

    [Header("Effects Launch Positions")]
    public GameObject ringAndBallLaunchPosition;
    public GameObject missileLaunchPosition;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator enemyAnimator;
    private Rigidbody enemyRB;

    private JumpOnTarget jumpOnTarget;
    private BossEnemyStats enemyStats;

    private float currentHealth;
    private float randomSelectedTimeStateChange;
    private float currentTimeStateChange;
    private int timesFunctionCalled;

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

        randomSelectedTimeStateChange = Random.Range(minTimeCountBetweenStateChange,
            maxTimeCountBetweenStateChange);
        currentTimeStateChange = 0;

        timesFunctionCalled = 0;
        currentState = EnemyState.Idle;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (currentHealth <= 0 && currentState != EnemyState.Dead)
            currentState = EnemyState.Dead;

        MakeEnemyFall();

        if (currentState != EnemyState.Dead)
        {
            Vector3 lookPosition = player.transform.position - gameObject.transform.position;
            lookPosition.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
                rotation, rotationRate * Time.deltaTime);
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                DoStuffWhileEnemyIdle();
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
        if (currentTimeStateChange >= randomSelectedTimeStateChange &&
            currentState != EnemyState.Falling)
        {
            currentState = EnemyState.Idle;
            currentTimeStateChange = 0;
        }
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
        agent.enabled = true;

        currentTimeStateChange = 0;
        enemyAnimator.SetBool(EnemyControlsManager.BossFallingParam, false);
        enemyStats.isJumping = false;
    }

    void DoStuffWhileEnemyIdle()
    {
        enemyAnimator.SetFloat(EnemyControlsManager.EnemyVelocity, 0);
        agent.ResetPath();
        enemyRB.isKinematic = true;

        randomSelectedTimeStateChange = Random.Range(minTimeCountBetweenStateChange,
            maxTimeCountBetweenStateChange);
        timesFunctionCalled = 0;

        int randomNumber = Random.Range(0, 1000);
        int randomMove = randomNumber % 5;

        print("Current State: " + currentState);

        switch (randomMove)
        {
            case 0:
                currentState = EnemyState.Moving;
                break;

            case 1:
                MakeEnemyJump();
                break;

            case 2:
                currentState = EnemyState.RingAttack;
                break;

            case 3:
                currentState = EnemyState.BallAttack;
                break;

            case 4:
                currentState = EnemyState.MissileAttack;
                break;
        }

        print("State Changed To: " + currentState);
    }

    void MakeEnemyJump()
    {
        if (currentState == EnemyState.Jumping || currentState == EnemyState.Falling)
            return;

        currentState = EnemyState.Jumping;
        agent.enabled = false;

        jumpOnTarget.TriggerJump();

        enemyStats.isJumping = true;
        enemyAnimator.SetTrigger(EnemyControlsManager.BossJumpParam);
    }

    void MoveEnemyTowardsPlayer()
    {
        enemyAnimator.SetFloat(EnemyControlsManager.EnemyVelocity, agent.velocity.magnitude);
        agent.SetDestination(player.transform.position);
    }

    void AttackWithRings()
    {
        if (currentState != EnemyState.RingAttack)
            return;

        if (currentTimeStateChange / (waitTimeBetweenAttacks * timesFunctionCalled) > 1)
        {
            timesFunctionCalled += 1;
            Instantiate(jumpingRing, ringAndBallLaunchPosition.transform.position,
                jumpingRing.transform.rotation);
        }
    }

    void AttackWithBalls()
    {
        if (currentState != EnemyState.BallAttack)
            return;

        if (currentTimeStateChange / (waitTimeBetweenAttacks * timesFunctionCalled) > 1)
        {
            timesFunctionCalled += 1;
            Instantiate(jumpingBalls, ringAndBallLaunchPosition.transform.position,
                jumpingBalls.transform.rotation);
        }

    }

    void AttackWithMissiles()
    {
        if (currentState != EnemyState.MissileAttack)
            return;

        if (currentTimeStateChange / (waitTimeBetweenAttacks * timesFunctionCalled) > 1)
        {
            timesFunctionCalled += 1;
            Instantiate(missile, missile.transform.position,
                missile.transform.rotation);
        }
    }

    void MakeEnemyFall()
    {
        float yVelocity = enemyRB.velocity.y;
        if (yVelocity < fallThreshold && currentState != EnemyState.Falling)
        {
            enemyAnimator.SetBool(EnemyControlsManager.BossFallingParam, true);
            currentState = EnemyState.Falling;
        }
    }
}
