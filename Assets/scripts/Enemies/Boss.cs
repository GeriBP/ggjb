using System;
using System.Collections.Generic;
using System.Linq;
using FluentBehaviourTree;
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

    [SerializeField]
    private GameObject minionPrefab;

    /// <summary>
    /// Can't take damage while the shield is active.
    /// </summary>
    private bool shieldActive = false;

    /// <summary>
    /// Our current hit point. At or below 0 will result in death.
    /// </summary>
    private float currentHP;

    private new Rigidbody2D rigidbody;

    private IBehaviourTreeNode behaviour;

    private GameObject shield;

    private IList<GameObject> activeMinions = new List<GameObject>();

    #region state management
    private bool alreadySpawnedMinions = false;


    #endregion

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rigidbody);

        var shieldTransform = GetComponentsInChildren<Transform>()
            .Where(t => t.name == "Shield")
            .FirstOrDefault();
        Assert.IsNotNull(shieldTransform);
        shield = shieldTransform.gameObject;
    }

    void Start()
    {
        currentHP = startingHP;

        behaviour = new BehaviourTreeBuilder()
            .Sequence("Main behaviour")
                .Sequence("Spawn minions")
                    .Selector("Spawn")
                        .Condition("Already spawned", t => alreadySpawnedMinions)
                        .Sequence("Enable shield and spawn")
                            .Do("Enable shield", t => SetShieldEnabled(true))
                            .Do("Spawn", SpawnMinions)
                        .End()
                    .End()
                    .Do("Wait until all minions defeated", WaitUntilMinionsDefeated)
                    .Do("Disable shield", t => SetShieldEnabled(false))
                .End()
            .End()
            .Build();
    }

    void Update()
    {
        behaviour.Tick(new TimeData(Time.deltaTime));
    }

    BehaviourTreeStatus SpawnMinions(TimeData t)
    {
        foreach (var spawnPoint in GetComponentsInChildren<Transform>())
        {
            Debug.Log(spawnPoint.name);
            if (spawnPoint.name == "SpawnPoint")
            {
                activeMinions.Add(Instantiate(minionPrefab, spawnPoint.transform.position, Quaternion.identity));
            }
        }
        alreadySpawnedMinions = true;

        return BehaviourTreeStatus.Success;
    }

    BehaviourTreeStatus WaitUntilMinionsDefeated(TimeData t)
    {
        return activeMinions.All(e => e == null) ? 
            BehaviourTreeStatus.Success :
            BehaviourTreeStatus.Running;
    }

    BehaviourTreeStatus SetShieldEnabled(bool enabled)
    {
        shield.SetActive(enabled);
        shieldActive = enabled;

        return BehaviourTreeStatus.Success;
    }

    public void TakeHit(float damage)
    {
        currentHP -= damage;
    }
}