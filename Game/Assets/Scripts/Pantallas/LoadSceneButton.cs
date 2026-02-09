using UnityEngine;
using UnityEngine.SceneManagement; // Para cambiar de escena

public class LoadSceneButton : MonoBehaviour
{
    // Este método se llama al pulsar el botón
    public void LoadScene()
    {
        SceneManager.LoadScene("InicioScene");
    }
}