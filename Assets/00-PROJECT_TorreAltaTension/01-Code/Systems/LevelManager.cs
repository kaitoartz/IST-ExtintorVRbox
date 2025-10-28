using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void ReiniciarEscena()
    { SceneManager.LoadScene("Inmergency");
    }
    public void QuitGame()
    { Application.Quit();
    }
}
