using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Enemy that moves towards the player and shoots at them
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Shooter : MonoBehaviour, IEnemy
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
		Assert.IsNotNull(player, "Shooter enemy must be in the same scene as the player");
		target = player.transform;		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

	}

    public void TakeHit()
    {
        throw new NotImplementedException();
    }
}
