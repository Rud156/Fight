using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(JumpOnTarget))]
public class EnemyController : MonoBehaviour
{
    [Header("General Stats")]
    public float rotationSpeed = 0.7f;
    public int maxHealth = 500;

    [Header("Effects")]
    public GameObject deathEffect;
    public GameObject jumpingRing;
    public GameObject jumpingBalls;
    public GameObject missile;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator enemyAnimator;
    private JumpOnTarget jumpOnTarget;

    private float currentHealth;
    private bool isFalling;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsManager.Player);
        agent = gameObject.GetComponent<NavMeshAgent>();
        enemyAnimator = gameObject.GetComponent<Animator>();
        jumpOnTarget = gameObject.GetComponent<JumpOnTarget>();

        currentHealth = maxHealth;
        isFalling = false;
        EnemyStats.isJumping = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (!EnemyStats.isJumping)
            return;

        if (!other.gameObject.CompareTag(TagsManager.Player) &&
            !other.gameObject.CompareTag(TagsManager.Ground))
            return;

        isFalling = false;
        enemyAnimator.SetBool(EnemyControlsManager.BossFallingParam, false);
    }

}
