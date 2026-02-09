using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // Distancia para hacer daño
    public float attackCooldown = 1f; // Tiempo entre ataques
    private float attackTimer = 0f;

    private Transform player;
    private PlayerHealth playerHealth;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (player == null || playerHealth == null) return;

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && attackTimer <= 0f)
        {
            // Hacer daño
            playerHealth.TakeDamage(1);
            attackTimer = attackCooldown;
        }
    }
}