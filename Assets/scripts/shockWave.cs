using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shockWave : MonoBehaviour 
{
    /// <summary>
    /// How much damage to deal to enemies
    /// </summary>
	public float Damage { get; set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        IEnemy ie = other.gameObject.GetComponent<IEnemy>();
        if (ie != null)
        {
            ie.TakeHit(Damage);
        }
    }
}
