using UnityEngine;
using UnityEngine.SceneManagement; // Para cambiar de escena

public class FirstScene : MonoBehaviour
{
    void Update()
    {
        // Si pulsas espacio → cargar escena
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}