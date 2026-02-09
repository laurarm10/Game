using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Para cambiar de escena

public class PlayerInventory : MonoBehaviour
{
    // --- DARDOS ---
    public int dartsCollected = 0;
    public TMP_Text dartsText;
    private GameObject dartInRange;

    public int totalKeys = 3;
    public int keysCollected = 0;
    public TMP_Text keysText;

    public TMP_Text messageText; // Texto para mensajes temporales
    public string victorySceneName = "VictoryScene"; // Nombre de la escena de victoria

    private GameObject keyInRange;
    private bool nearDoor = false; // Si estamos cerca de la puerta

    public GameObject dartPrefab;      // Prefab del dardo
    public Transform dartSpawnPoint;   // Donde sale el dardo (ej: frente a la cámara)

    void Start()
    {
        UpdateKeysUI();
        UpdateDartsUI();
        messageText.text = "";
    }

    void Update()
    {
        // Lanzar dardo con click izquierdo
        if (Input.GetMouseButtonDown(0)) // click izquierdo
        {
            if (dartsCollected <= 0)
            {
                StartCoroutine(ShowMessage("No tienes dardos", 2f));
            }
            else
            {
                ShootDart();
                dartsCollected--;
                UpdateDartsUI();
            }
        }

        // --- Recoger llaves ---
        if (keyInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            keysCollected++;
            UpdateKeysUI();
            Destroy(keyInRange);
            keyInRange = null;
        }

        // --- Interactuar con puerta ---
        if (nearDoor && Input.GetKeyDown(KeyCode.E))
        {
            if (keysCollected >= totalKeys)
            {
                // Cargar escena de victoria
                SceneManager.LoadScene(victorySceneName);
            }
            else
            {
                // Mostrar mensaje temporal
                StartCoroutine(ShowMessage("Te faltan llaves", 2f));
            }
        }

        // --- Recoger dardos ---
        if (dartInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            dartsCollected++;
            UpdateDartsUI();
            Destroy(dartInRange);
            dartInRange = null;
        }
    }
    void UpdateDartsUI()
    {
        dartsText.text = "Dardos: " + dartsCollected;
    }

    void UpdateKeysUI()
    {
        keysText.text = "Keys: " + keysCollected + "/" + totalKeys;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            keyInRange = other.gameObject;
        }
        else if (other.CompareTag("Dart"))
        {
            dartInRange = other.gameObject;
        }
        else if (other.CompareTag("ExitDoor"))
        {
            nearDoor = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            keyInRange = null;
        }
        else if (other.CompareTag("Dart"))
        {
            dartInRange = null;
        }
        else if (other.CompareTag("ExitDoor"))
        {
            nearDoor = false;
        }
    }

    private System.Collections.IEnumerator ShowMessage(string message, float duration)
    {
        messageText.text = message;
        yield return new WaitForSeconds(duration);
        messageText.text = "";
    }

    void ShootDart()
    {
        float maxDistance = 10f; // distancia máxima del dardo
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                SkeletonMov enemy = hit.collider.GetComponent<SkeletonMov>();
                if (enemy != null)
                {
                    enemy.Stun(5f); // aturde 5 segundos
                }
            }
        }
    }
}