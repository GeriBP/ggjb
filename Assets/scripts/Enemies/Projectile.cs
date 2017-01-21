using UnityEngine;

/// <summary>
/// Bullet shot by the shooter enemy type.
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Unit vector of the direction we're moving in.
    /// </summary>
    public Vector2 Direction { get; set; }

    /// <summary>
    /// How fast to move at.
    /// </summary>
    public float MovementSpeed { get; set; }

    /// <summary>
    /// How long to wait before automatically destroying self
    /// </summary>
    private static readonly float lifespan = 5f;
    public float speed;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(Direction.x*speed, Direction.y*speed, 0.0f);
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        //transform.position = (Vector2)transform.position + Direction * MovementSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Damage player, if we hit them.
        var player = collider.gameObject.GetComponent<playerMove>();
        if (player != null)
        {
            player.TakeHit();
        }

        // Destroy self on impact
        Destroy(gameObject);
    }
}