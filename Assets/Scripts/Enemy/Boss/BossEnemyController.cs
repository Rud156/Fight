﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(JumpOnTarget))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BossEnemyDamageAndDeathControls))]
public class BossEnemyController : MonoBehaviour
{
    [Header("General Stats")]
    public float rotationRate = 0.7f;
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

    [Header("Health Low Effect")]
    public AddScreenOverlay screenOverlay;
    public MinionSpawner minionSpawner;
    public GameObject soundRipple;
    public GameObject soundRippleSpawnParent;
    public float playSoundTime = 3f;
    public float spawnEnemiesTime = 60f;
    public float floatHeightAboveGround = 50f;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator enemyAnimator;
    private Rigidbody enemyRB;
    private BossEnemyDamageAndDeathControls bossHealthAndDamage;

    private JumpOnTarget jumpOnTarget;
    private float randomSelectedTimeStateChange;
    private float currentTimeStateChange;
    private int timesFunctionCalled;
    private bool isJumping;

    private int lowHeathAnimationCount;
    private bool disableUpdate;

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

        LowHealthAnimation
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

        bossHealthAndDamage = gameObject.GetComponent<BossEnemyDamageAndDeathControls>();
        jumpOnTarget = gameObject.GetComponent<JumpOnTarget>();

        isJumping = false;

        randomSelectedTimeStateChange = Random.Range(minTimeCountBetweenStateChange,
            maxTimeCountBetweenStateChange);
        currentTimeStateChange = 0;

        timesFunctionCalled = 0;

        lowHeathAnimationCount = -1;
        disableUpdate = false;

        UpdateState(EnemyState.Idle);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector3 lookPosition = player.transform.position - gameObject.transform.position;
        lookPosition.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
            rotation, rotationRate * Time.deltaTime);

        if (disableUpdate)
            return;

        if (currentState != EnemyState.Dead)
        {
            MakeEnemyFall();

            currentTimeStateChange += Time.deltaTime;
            if (currentTimeStateChange >= randomSelectedTimeStateChange &&
                currentState != EnemyState.Falling)
            {
                UpdateState(EnemyState.Idle);
                currentTimeStateChange = 0;
            }
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
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (!isJumping)
            return;

        if (!other.gameObject.CompareTag(TagsManager.Player) &&
            !other.gameObject.CompareTag(TagsManager.Ground))
            return;

        jumpOnTarget.LandOnGround();

        currentTimeStateChange = 0;

        enemyAnimator.SetBool(EnemyControlsManager.BossFallingParam, false);
        isJumping = false;
        enemyRB.velocity = Vector3.zero;

        agent.enabled = true;
        UpdateState(EnemyState.Idle);
    }

    void DoStuffWhileEnemyIdle()
    {
        float halfHealth = bossHealthAndDamage.maxBossHealth / 2;
        float quarterHealth = bossHealthAndDamage.maxBossHealth / 4;

        if (bossHealthAndDamage.currentBossHealth <= halfHealth && lowHeathAnimationCount == -1)
        {
            StartCoroutine(FloatAndSpawnEnemies());

            // Play First Time
            lowHeathAnimationCount += 1;
            disableUpdate = true;
            return;
        }
        else if (bossHealthAndDamage.currentBossHealth <= quarterHealth && lowHeathAnimationCount < 1)
        {
            StartCoroutine(FloatAndSpawnEnemies());

            // Play Second Time
            lowHeathAnimationCount = lowHeathAnimationCount == -1 ?
                lowHeathAnimationCount + 2 : lowHeathAnimationCount + 1;
            disableUpdate = true;
            return;
        }

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

        isJumping = true;
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

    IEnumerator FloatAndSpawnEnemies()
    {
        enemyAnimator.SetFloat(EnemyControlsManager.EnemyVelocity, 0);
        agent.ResetPath();
        agent.enabled = false;
        enemyRB.isKinematic = true;

        GameObject soundRippleInstance = Instantiate(soundRipple, gameObject.transform.position,
            gameObject.transform.rotation);
        soundRippleInstance.transform.SetParent(soundRippleSpawnParent.transform);
        screenOverlay.TurnOnChromaticAberration();

        yield return new WaitForSeconds(playSoundTime);

        Destroy(soundRippleInstance);
        screenOverlay.TurnOffChromaticAberration();

        Vector3 currentPosition = gameObject.transform.position;
        gameObject.transform.position = new Vector3(currentPosition.x, floatHeightAboveGround,
            currentPosition.z);

        minionSpawner.StartSpawn();

        yield return new WaitForSeconds(spawnEnemiesTime);

        minionSpawner.StopSpawn();
    }
}
