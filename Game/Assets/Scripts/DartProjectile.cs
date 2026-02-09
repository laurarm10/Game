using UnityEngine;

public class DartProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f; // se destruye tras 3s
    public float stunTime = 5f; // segundos de aturdimiento

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Aturdir enemigo
            SkeletonMov enemy = other.GetComponent<SkeletonMov>();
            if (enemy != null)
            {
                enemy.Stun(stunTime);
            }

            Destroy(gameObject); // destruir dardo al impactar
        }
        else if (!other.CompareTag("Player"))
        {
            // destruir dardo si choca con cualquier otra cosa
            Destroy(gameObject);
        }
    }
}