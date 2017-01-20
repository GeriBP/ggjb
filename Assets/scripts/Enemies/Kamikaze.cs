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
	private float movementSpeed = 70f;

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
		var newForce = toPlayer * movementSpeed * Time.fixedDeltaTime;
		rigidbody.AddForce(newForce);
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

    public void TakeHit()
    {
		// TODO: play animation and destroy self
        throw new NotImplementedException();
    }
}
