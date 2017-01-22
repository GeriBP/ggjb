using System;
using System.Collections;
using System.Collections.Generic;
using FluentBehaviourTree;
using FMODUnity;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Enemy that moves towards the player and shoots at them
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(StudioEventEmitter))]
public class Shooter : MonoBehaviour, IEnemy
{
	[SerializeField]
    private GameObject wavePoints, pixelExplosion, shooterPs, ex2, shootPoint;

	[SerializeField]
	private float movementSpeed = 70f;

	/// <summary>
	/// Maximum distance from the player before we will stop and start shooting.
	/// </summary>
	[SerializeField]
	private float maxShootDistance = 10f;

	/// <summary>
	/// Time to wait after firing a shot before we can fire again.
	/// </summary>
	[SerializeField]
	private float timeBetweenShots = 0.2f;

	[Header("Projectile properties")]

	/// <summary>
	/// Prefab to spawn for the projectile
	/// </summary>
	[SerializeField]
	private Projectile projectilePrefab;

	[SerializeField]
	private float projectileSpeed = 5f;

	private new Rigidbody2D rigidbody;
	private StudioEventEmitter gunSoundEmitter;
	private StudioEventEmitter deathSoundEmitter;

	private IBehaviourTreeNode behaviour;

	/// <summary>
	/// Target object to move towards.
	/// </summary>
	private Transform target;
    private SpriteRenderer sr;

    private float timeLastShotFired;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		Assert.IsNotNull(rigidbody, "Enemy must have a rigidbody attached to it");

		var soundEmitters = GetComponents<StudioEventEmitter>();
		Assert.IsTrue(soundEmitters.Length == 2, "Shoot requires two sound emitters");
		gunSoundEmitter = soundEmitters[0];
		deathSoundEmitter = soundEmitters[1];
	}

    // Use this for initialization
    void Start () 
	{
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine("orbit");
		var player = FindObjectOfType<playerMove>();
		Assert.IsNotNull(player, "Shooter enemy must be in the same scene as the player");
		target = player.transform;

		behaviour = new BehaviourTreeBuilder()
			.Selector("logic")
				.Sequence("Shoot at player")
					.Condition("is within range", t => Vector3.Distance(transform.position, target.position) <= maxShootDistance)
					.Do("Stop moving", StopMoving)
					.Do("Wait for cooldown", WaitForFireCooldown)
					.Do("Fire", Fire)
				.End()
				.Do("Move towards target", MoveTowardsTarget)
			.End()
			.Build();
	}
	
	// Update is called once per frame
	void Update () 
	{
		behaviour.Tick(new TimeData(Time.deltaTime));
	}

	private BehaviourTreeStatus Fire(TimeData t)
	{
		var projectile = Instantiate(projectilePrefab, shootPoint.transform.position, Quaternion.identity);
		projectile.MovementSpeed = projectileSpeed;
		projectile.Direction = (target.position - transform.position).normalized;

		gunSoundEmitter.Play();

		timeLastShotFired = Time.time;
		return BehaviourTreeStatus.Success;
	}

	private BehaviourTreeStatus MoveTowardsTarget(TimeData t)
	{
		var toPlayer = (target.position - transform.position).normalized;
		var newVelocity = toPlayer * movementSpeed;
		rigidbody.velocity = newVelocity;

		return BehaviourTreeStatus.Success;
	}

	private BehaviourTreeStatus StopMoving(TimeData t)
	{
		rigidbody.velocity = Vector2.zero;
		return BehaviourTreeStatus.Success;
	}

	private BehaviourTreeStatus WaitForFireCooldown(TimeData t)
	{
		return Time.time >= timeLastShotFired + timeBetweenShots ?
			BehaviourTreeStatus.Success :
			BehaviourTreeStatus.Running;
	}

    public void TakeHit(float damage)
    {
        for (int i = 0; i < 3; ++i)
        {
            Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(transform.position.x - 0.2f, transform.position.x + 0.2f), UnityEngine.Random.Range(transform.position.y - 0.2f, transform.position.y + 0.2f), 0.0f);
            Instantiate(wavePoints, spawnPos, Quaternion.identity);
        }
        Instantiate(pixelExplosion, transform.position, Quaternion.identity);
        Instantiate(ex2, transform.position, Quaternion.identity);
        Instantiate(shooterPs, transform.position, Quaternion.identity);

		deathSoundEmitter.Play();

        gameManager.enemiesAlive--;
        Destroy(gameObject);
    }

    IEnumerator orbit()
    {
        if (sr.sortingOrder == 1) sr.sortingOrder = 3;
        else sr.sortingOrder = 1;
        yield return new WaitForSeconds(4.8f);
        StartCoroutine("orbit");
    }
}
