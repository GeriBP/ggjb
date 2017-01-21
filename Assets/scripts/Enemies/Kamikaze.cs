using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Enemy that flys towards the player and then blows up.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Kamikaze : MonoBehaviour, IEnemy
{
	[SerializeField]
    private GameObject wavePoints, pixelExplosion;
	
	[SerializeField]
	private float movementSpeed = 3f;

	private new Rigidbody2D rigidbody;

	/// <summary>
	/// Target object to move towards.
	/// </summary>
	private Transform target;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		Assert.IsNotNull(rigidbody, "Enemy must have a rigidbody attached to it");
	}

    // Use this for initialization
    void Start () 
	{
		var player = FindObjectOfType<playerMove>();
		Assert.IsNotNull(player, "Kamikaze enemy must be in the same scene as the player");
		target = player.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		var toPlayer = (target.position - transform.position).normalized;
		var newVelocity = toPlayer * movementSpeed;
		rigidbody.velocity = newVelocity;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform == target)
		{
			Explode();
		}
	}

	private void Explode()
	{
		// TODO: play explosion animation
		target.SendMessage("TakeHit");

		Destroy(gameObject);
	}

    public void TakeHit(float damage)
    {
        for (int i = 0; i < 3; ++i)
        {
            Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(transform.position.x - 0.2f, transform.position.x + 0.2f), UnityEngine.Random.Range(transform.position.y - 0.2f, transform.position.y + 0.2f), 0.0f);
            Instantiate(wavePoints, spawnPos, Quaternion.identity);
        }
        Instantiate(pixelExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
