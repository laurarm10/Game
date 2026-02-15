using UnityEngine;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 8f;

    public float maxSprintTime = 3f;    // Tiempo máximo de sprint
    public float sprintRechargeTime = 3f; // Tiempo que tarda en recargarse

    private CharacterController controller;
    public float sprintTimer;           // Cuenta el sprint actual
    private float rechargeTimer;         // Cuenta la recarga
    private bool canSprint = true;       // Si se puede correr

    public TMP_Text sprintText;

    public float crouchHeight = 0.8f;     // Altura al agacharse
    public float standHeight = 1.7f;      // Altura normal
    public float crouchSpeed = 3f;      // Velocidad mientras agachas (opcional)

    public Transform playerCamera; // La cámara que está dentro del Player

    void Start()
    {
        controller = GetComponent<CharacterController>();
        sprintTimer = maxSprintTime;
        // Posición inicial de la cámara
        playerCamera.localPosition = new Vector3(0, standHeight - 10f, 0);
    }

    void Update()
    {
        // --- Movimiento ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // --- Velocidad ---
        float currentSpeed = walkSpeed; // Por defecto camina

        // Sprint solo si se puede
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
        {
            currentSpeed = runSpeed;
        }

        // Reducir velocidad si está agachado
        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed = crouchSpeed;
        }

        // Mover al Player
        controller.Move(move * currentSpeed * Time.deltaTime);

        // --- Gestionar sprint ---
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && canSprint;

        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;

            if (sprintTimer <= 0f)
            {
                sprintTimer = 0f;
                canSprint = false;
                rechargeTimer = sprintRechargeTime;
            }
        }
        else if (!canSprint)
        {
            // Tiempo de recarga
            rechargeTimer -= Time.deltaTime;
            if (rechargeTimer <= 0f)
            {
                canSprint = true;
                sprintTimer = maxSprintTime;
            }
        }

        // --- Mostrar mensaje de sprint ---
        if (isSprinting)
        {
            sprintText.text = "Sprint ON";
        }
        else if (!canSprint)
        {
            sprintText.text = "Sprint Reloading";
        }
        else
        {
            sprintText.text = "";
        }

        // --- Agacharse ---
        if (Input.GetKey(KeyCode.Space))
        {
            controller.height = crouchHeight;
            playerCamera.localPosition = new Vector3(0, crouchHeight - 1f, 0);
        }
        else
        {
            controller.height = standHeight;
            playerCamera.localPosition = new Vector3(0, standHeight - 1f, 0);
        }
    }
}