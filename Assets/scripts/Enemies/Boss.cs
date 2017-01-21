using System;
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

    /// <summary>
    /// Can't take damage while the shield is active.
    /// </summary>
    private bool shieldActive = false;

    /// <summary>
    /// Our current hit point. At or below 0 will result in death.
    /// </summary>
    private float currentHP;

    private new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rigidbody);
    }

    void Start()
    {
        currentHP = startingHP;
    }

    public void TakeHit(float damage)
    {
        currentHP -= damage;
    }
}