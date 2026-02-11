using UnityEngine;
using UnityEngine.AI;

public class SkeletonMov : MonoBehaviour
{
    public float detectionRange = 8f;
    public float walkSpeed = 2f;
    public float runSpeed = 3f;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    public float patrolRadius = 10f;
    public float patrolWaitTime = 2f;

    private float waitTimer;
    private Vector3 patrolTarget;
    private bool hasPatrolPoint = false;

    private bool isStunned = false;
    private float stunTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Buscar al jugador
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        agent.isStopped = true;
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
    }

    void Update()
    {

        // Manejo de aturdimiento
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                agent.isStopped = false;
            }
            return; // si está aturdido no hace nada más
        }

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 🔥 PERSEGUIR JUGADOR
        if (distance <= detectionRange)
        {
            agent.speed = runSpeed;
            agent.SetDestination(player.position);

            animator.SetBool("Run", true);
            animator.SetBool("Walk", false);

            hasPatrolPoint = false;
        }
        // 😴 PATRULLA ALEATORIA
        else
        {
            agent.speed = walkSpeed;

            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);

            // Si no tiene punto, crear uno
            if (!hasPatrolPoint)
            {
                patrolTarget = GetRandomPatrolPoint();
                agent.SetDestination(patrolTarget);
                hasPatrolPoint = true;
            }

            // Si llegó al destino
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                waitTimer += Time.deltaTime;

                if (waitTimer >= patrolWaitTime)
                {
                    hasPatrolPoint = false;
                    waitTimer = 0f;
                }
            }
        }
    }

    Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }
}