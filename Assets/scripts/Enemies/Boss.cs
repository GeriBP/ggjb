using System;
using UnityEngine;

/// <summary>
/// The final boss of the game
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Boss : MonoBehaviour, IEnemy
{
    public void TakeHit(float damage)
    {
        throw new NotImplementedException();
    }
}