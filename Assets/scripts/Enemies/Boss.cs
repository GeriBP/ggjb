using System;
using System.Collections.Generic;
using System.Linq;
using FluentBehaviourTree;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// The final boss of the game
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Boss : MonoBehaviour, IEnemy
{
    [SerializeField]
    private float startingHP = 1000f;

    [SerializeField]
    private GameObject minionPrefab;

	/// <summary>
	/// Prefab to spawn for the projectile
	/// </summary>
	[SerializeField]
	private Projectile projectilePrefab;

    /// <summary>
    /// Time between each bullet
    /// </summary>
    [SerializeField]
    private float timeBetweenEachShot = 0.5f;

    /// <summary>
    /// Duration of each volley of bullets when the boss is attacking
    /// </summary>
    [SerializeField]
    private float fireDuration = 2f;

    /// <summary>
    /// How long to wait before firing a round of shots
    /// </summary>
    [SerializeField]
    private float fireWindupDuration = 1.5f;

	[SerializeField]
	private float projectileSpeed = 5f;

    /// <summary>
    /// Can't take damage while the shield is active.
    /// </summary>
    private bool shieldActive = false;

    /// <summary>
    /// Our current hit point. At or below 0 will result in death.
    /// </summary>
    private float currentHP;

    private new Rigidbody2D rigidbody;

    private IBehaviourTreeNode behaviour;

    private GameObject shield;

    #region state management
    private bool alreadySpawnedMinions = false;
    private IList<GameObject> activeMinions = new List<GameObject>();
    private float timeAttackStarted;
    private int numberOfAttacksCompleted = 0;
    private int numberOfAttacksWoundUp = 0;
    private int currentAttack = -1;
    private int nextAttack = 0;
    private float windupStartTime;
    private float timeLastFired;
    private int initialNumberOfEnemies;
    #endregion

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rigidbody);

        var shieldTransform = GetComponentsInChildren<Transform>()
            .Where(t => t.name == "Shield")
            .FirstOrDefault();
        Assert.IsNotNull(shieldTransform);
        shield = shieldTransform.gameObject;

        Assert.IsNotNull(projectilePrefab);
    }

    void Start()
    {
        currentHP = startingHP;

        behaviour = new BehaviourTreeBuilder()
            .Sequence("Main behaviour")
                .Sequence("Spawn minions")
                    .Selector("Spawn")
                        .Condition("Already spawned", t => alreadySpawnedMinions)
                        .Sequence("Enable shield and spawn")
                            .Do("Enable shield", t => SetShieldEnabled(true))
                            .Do("Spawn", SpawnMinions)
                        .End()
                    .End()
                    .Do("Wait until all minions defeated", WaitUntilMinionsDefeated)
                    .Do("Disable shield", t => SetShieldEnabled(false))
                .End()
                .Sequence("Attack")
                    .Selector("Set up attack")
                        .Condition("Already set up", t => currentAttack >= nextAttack)
                        .Do("Set up", t =>
                        {
                            Debug.Log("Setting up attack");
                            currentAttack++;
                            windupStartTime = Time.time;
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                    .Do("Wait", t => Time.time >= windupStartTime + fireWindupDuration ?
                        BehaviourTreeStatus.Success : BehaviourTreeStatus.Running)
                    .Selector("Fire or wait")
                        .Condition("Timeout", t => Time.time >= windupStartTime + fireWindupDuration + fireDuration)
                        .Parallel("Fire and rotate", 2, 2)
                            .Sequence("Fire")
                                .Condition("Time", t => Time.time >= windupStartTime + fireWindupDuration + 1f)
                                .Do("Wait", t => Time.time >= timeLastFired + timeBetweenEachShot ? 
                                    BehaviourTreeStatus.Success : BehaviourTreeStatus.Running)
                                .Do("Fire", Fire)
                            .End()
                            .Do("Rotate", t => 
                            {
                                var time = Mathf.Clamp01((Time.time - (windupStartTime + fireWindupDuration)) / fireDuration);
                                time = 1f + Mathf.Sin((1.5f * Mathf.PI) + time * Mathf.PI * 0.5f);
                                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 360f, time));
                                return BehaviourTreeStatus.Success;
                            })
                        .End()
                    .End()
                .End()
                .Do("debug", t => { Debug.Log("reached end"); return BehaviourTreeStatus.Running; })
                // wait and reset 
            .End()
            .Build();
    }

    void Update()
    {
        behaviour.Tick(new TimeData(Time.deltaTime));
    }

    private BehaviourTreeStatus SpawnMinions(TimeData t)
    {
        foreach (var spawnPoint in GetComponentsInChildren<Transform>())
        {
            Debug.Log(spawnPoint.name);
            if (spawnPoint.name == "SpawnPoint")
            {
                activeMinions.Add(Instantiate(minionPrefab, spawnPoint.transform.position, Quaternion.identity));
            }
        }
        alreadySpawnedMinions = true;

        return BehaviourTreeStatus.Success;
    }

    private BehaviourTreeStatus WaitUntilMinionsDefeated(TimeData t)
    {
        return gameManager.enemiesAlive <= initialNumberOfEnemies ? 
            BehaviourTreeStatus.Success :
            BehaviourTreeStatus.Running;
    }

    private BehaviourTreeStatus SetShieldEnabled(bool enabled)
    {
        shield.SetActive(enabled);
        shieldActive = enabled;

        return BehaviourTreeStatus.Success;
    }

	private BehaviourTreeStatus Fire(TimeData t)
	{
        Debug.Log("Firing");
        
		SpawnProjectile(transform.rotation * new Vector2(0, 1));
		SpawnProjectile(transform.rotation * new Vector2(-1, 0));
		SpawnProjectile(transform.rotation * new Vector2(1, 0));
		SpawnProjectile(transform.rotation * new Vector2(0, -1));

		timeLastFired = Time.time;
		return BehaviourTreeStatus.Success;
	}

    private void SpawnProjectile(Vector2 direction)
    {   
        var projectile = Instantiate(projectilePrefab, (Vector2)transform.position + direction, Quaternion.identity);
		projectile.MovementSpeed = projectileSpeed;
        projectile.Direction = direction;
    }

    private BehaviourTreeStatus Reset(TimeData t)
    {
        alreadySpawnedMinions = false;
        activeMinions.Clear();
        numberOfAttacksCompleted = 0;
        numberOfAttacksWoundUp = 0;
        currentAttack = -1;
        nextAttack = 0;
        initialNumberOfEnemies = gameManager.enemiesAlive;

        return BehaviourTreeStatus.Success;
    }

    public void TakeHit(float damage)
    {
        currentHP -= damage;
    }
}