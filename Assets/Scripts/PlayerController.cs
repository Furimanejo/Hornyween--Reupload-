using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISpriteAnimation
{
    public static PlayerController Instance { get; private set; }
    public Vector2 direction { get; private set; } = Vector3.right;
    public float Health { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }

    public int Level { get; private set; } = 0;
    public int XP { get; private set; } = 0;
    public int XPToNextLevel
    {
        get
        {
            return (Level + 1) * 10;
        }
    }

    [SerializeField] UpgradeWindow upgradeWindow;
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] List<Sprite> walkAnimation;
    [SerializeField] float moveSpeed = 1;
    bool isWalking = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Health = MaxHealth;
        }
        else
        {
            Debug.LogError("Duplicate PlayerController found, destroying it");
            Destroy(this);
        }
    }

    private void Update()
    {
        if(Health <= 0f)
        {
            cam.transform.parent = null;
            gameObject.SetActive(false);
        }
        else
        {
            if (XP >= XPToNextLevel)
            {
                XP -= XPToNextLevel;
                Level++;
                upgradeWindow.Trigger();
            }
        }
    }

    private void FixedUpdate()
    {
        var inputDirection = Vector2.zero;
        if (Input.GetMouseButton(0))
        {
            var targetPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            inputDirection = (Vector2)targetPosition - rb.position;
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
                inputDirection += Vector2.up;
            if (Input.GetKey(KeyCode.A))
                inputDirection += Vector2.left;
            if (Input.GetKey(KeyCode.S))
                inputDirection += Vector2.down;
            if (Input.GetKey(KeyCode.D))
                inputDirection += Vector2.right;
        }

        if (inputDirection != Vector2.zero)
        {
            isWalking = true;
            direction = inputDirection;
            rb.velocity = direction.normalized * moveSpeed;
        }
        else
        {
            isWalking = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
            CheckTouchEnemy(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckTouchEnemy(collision);
    }

    void CheckTouchEnemy(Collision2D collision)
    {
        var enemy = collision.collider.GetComponentInParent<EnemyController>();
        if (enemy)
        {
            Health -= enemy.EnemyType.Damage * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var candy = collision.gameObject.GetComponentInParent<Candy>();
        if (candy)
        {
            XP += 1;
            if (Health < MaxHealth)
                Health += 1;
            Destroy(candy.gameObject);
        }
    }

    // animation
    public List<Sprite> WalkAnimation()
    {
        return walkAnimation;
    }

    public bool Flip()
    {
        return direction.x < 0;
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}