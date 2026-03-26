using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelManager : MonoBehaviour, ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [Tooltip("Arrastra aquí la escena desde el proyecto")]
    public SceneAsset sceneAsset; // Solo visible en el Editor
#endif

    [HideInInspector] public string sceneName; // Se usa internamente en el build

    // Se ejecuta antes de compilar o guardar para extraer el nombre
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
#endif
    }

    public void OnAfterDeserialize() { }

    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();

        // CORRECCIÓN 1: Envolver esto para que no rompa el build
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    // CORRECCIÓN 2: Eliminamos el parámetro SceneAsset.
    // Ahora carga la escena que guardaste en la variable 'sceneAsset' del inspector.
    public void IniciarEscenaGuardada()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("No hay ninguna escena asignada en el Inspector del LevelManager.");
        }
    }

    public void IniciarEscenaPorNombre(string nombreDeEscena)
    {
        SceneManager.LoadScene(nombreDeEscena);
    }
}