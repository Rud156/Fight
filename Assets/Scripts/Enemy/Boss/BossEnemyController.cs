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
    public float launchFromHeightAboveGround = 1f;
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
        UpdateState(EnemyState.Idle);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (currentHealth <= 0 && currentState != EnemyState.Dead)
            UpdateState(EnemyState.Dead);

        MakeEnemyFall();

        if (currentState != EnemyState.Dead)
        {
            Vector3 lookPosition = player.transform.position - gameObject.transform.position;
            lookPosition.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
                rotation, rotationRate * Time.deltaTime);
        }

        // Vector3 position = gameObject.transform.position;
        // Vector3 clampedPosition = new Vector3(position.x, Mathf.Clamp(position.y, 0.2f, 1000), position.z);
        // if (agent.enabled)
        //     agent.Warp(clampedPosition);
        // else
        //     gameObject.transform.position = clampedPosition;

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
            UpdateState(EnemyState.Idle);
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

        jumpOnTarget.LandOnGround();

        currentTimeStateChange = 0;

        enemyAnimator.SetBool(EnemyControlsManager.BossFallingParam, false);
        enemyStats.isJumping = false;
        enemyRB.velocity = Vector3.zero;

        agent.enabled = true;
        UpdateState(EnemyState.Idle);
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
        // int randomMove = 1;
        // int randomMove = randomNumber % 2;

        print("Current State: " + currentState);

        switch (randomMove)
        {
            case 0:
                UpdateState(EnemyState.Moving);
                break;

            case 1:
                MakeEnemyJump();
                break;

            case 2:
                UpdateState(EnemyState.RingAttack);
                break;

            case 3:
                UpdateState(EnemyState.BallAttack);
                break;

            case 4:
                UpdateState(EnemyState.MissileAttack);
                break;
        }

        print("State Changed To: " + currentState);
    }

    void MakeEnemyJump()
    {
        if (currentState == EnemyState.Jumping || currentState == EnemyState.Falling)
            return;

        UpdateState(EnemyState.Jumping);
        agent.enabled = false;

        Vector3 currentPosition = gameObject.transform.position;
        Vector3 modifiedPosition = new Vector3(currentPosition.x, launchFromHeightAboveGround,
            currentPosition.z);
        gameObject.transform.position = modifiedPosition;

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
            UpdateState(EnemyState.Falling);
        }
    }

    void UpdateState(EnemyState state)
    {
        currentState = state;
    }
}
