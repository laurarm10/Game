using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    void Update()
    {
        // Si pulsas espacio → cargar escena
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("InicioScene");
        }
    }
}