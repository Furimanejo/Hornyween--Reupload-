using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, ISpriteAnimation
{
    [SerializeField] Rigidbody2D rb = null;
    public static List<EnemyController> spawnedEnemies { get; set; } = new();
    public static List<EnemyController> despawnedEnemies { get; set; } = new();
    public EnemyType EnemyType { get; private set; }
    public Vector2 direction = Vector2.zero;
    float currentHP = -1f;

    public void Spawn(EnemyType enemyType)
    {
        if (spawnedEnemies.Contains(this))
            throw new System.Exception("Enemy already spawned");
        despawnedEnemies.Remove(this);
        spawnedEnemies.Add(this);

        gameObject.SetActive(true);
        transform.position = SpawnPositionAroundPlayer();
        EnemyType = enemyType;
        currentHP = enemyType.Health;
    }
    public Vector2 SpawnPositionAroundPlayer()
    {
        Vector2 position;
        var spawnAroundRadius = 11f;
        Vector2 playerPosition = PlayerController.Instance.transform.position;
        Collider2D collision;
        do
        {
            position = Random.onUnitSphere;
            position.Normalize();
            position *= spawnAroundRadius;
            position += playerPosition;
            collision = Physics2D.OverlapCircle(position, 0.5f);
        } while (collision != null);
        return position;
    }

    public void Despawn()
    {
        if (despawnedEnemies.Contains(this))
            throw new System.Exception("Enemy already despawned");
        despawnedEnemies.Add(this);
        spawnedEnemies.Remove(this);

        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        FollowPlayer();
    }
    void FollowPlayer()
    {
        direction = (Vector2)PlayerController.Instance.transform.position - rb.position;
        rb.velocity = direction.normalized * EnemyType.MoveSpeed;
    }
    
    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        ToyManager.vibrationScore += amount;
        if (currentHP <= 0f)
            Die();
    }

    void Die()
    {
        var drop = EnemyType.Drop;
        if (drop)
            Instantiate(drop, transform.position, Quaternion.identity);
        Despawn();
    }

    // Sprite Animation
    public List<Sprite> WalkAnimation()
    {
        return EnemyType.WalkAnimSprites;
    }
    public bool IsWalking()
    {
        return true;
    }
    public bool Flip()
    {
        return direction.x < 0;
    }
}
