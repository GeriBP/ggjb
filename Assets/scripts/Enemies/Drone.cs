using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

/// <summary>
/// Simple enemy type that bounces off objects and does not follow the player.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(StudioEventEmitter))]
public class Drone : MonoBehaviour, IEnemy 
{
	[SerializeField]
    private GameObject wavePoints, pixelExplosion, dronePs, ex2;

	[SerializeField]
	private float movementSpeed = 3f;

	private Vector2 currentMovementDirection;

	private new Rigidbody2D rigidbody;
	private StudioEventEmitter soundEmitter;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		soundEmitter = GetComponent<StudioEventEmitter>();
		Assert.IsNotNull(rigidbody, "Drone must have a Rigidbody2D attached to it.");
		Assert.IsNotNull(soundEmitter, "Drone must have a StudioEventEmitter attached to it.");
	}

	void Start()
	{
		currentMovementDirection = Random.insideUnitCircle.normalized;
        float angleZ = Random.Range(0.0f, 360.0f);
        transform.Rotate(transform.rotation.x, transform.rotation.y, angleZ);
	}

	void FixedUpdate()
	{
		rigidbody.velocity = currentMovementDirection * movementSpeed;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		var player = collision.gameObject.GetComponent<playerMove>();
		if (player != null)
		{
			player.TakeHit();
        }

		// Bounce and change direction away from the object we collided with.
		currentMovementDirection = Vector2.Reflect(currentMovementDirection, collision.contacts[0].normal);
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
        Instantiate(dronePs, transform.position, Quaternion.identity);

		soundEmitter.Play();

        gameManager.enemiesAlive--;
		
        Destroy(gameObject);
    }
}
