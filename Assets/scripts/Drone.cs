using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Simple enemy type that bounces off objects and does not follow the player.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Drone : MonoBehaviour, IEnemy 
{
	[SerializeField]
	private float movementSpeed = 1f;

	private Vector2 currentMovementDirection;

	private new Rigidbody2D rigidbody;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		Assert.IsNotNull(rigidbody, "Drone must have a Rigidbody2D attached to it.");
	}

	void Start()
	{
		currentMovementDirection = new Vector2(1, 1).normalized;
	}

	void FixedUpdate()
	{
		rigidbody.velocity = currentMovementDirection * movementSpeed * Time.fixedDeltaTime;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		// Bounce and change direction away from the object we collided with.
		currentMovementDirection = Vector2.Reflect(currentMovementDirection, collision.contacts[0].normal);
	}

    public void DealDamage()
    {
        throw new NotImplementedException();
    }
}
