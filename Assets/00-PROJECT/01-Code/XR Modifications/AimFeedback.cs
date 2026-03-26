using UnityEngine;
using UnityEngine.UI; // Necesario para manipular la Imagen

[RequireComponent(typeof(AudioSource))] // Asegura que haya un AudioSource
public class AimFeedback : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Arrastra aquí la Imagen del punto central del Canvas.")]
    public Image imagenPunto;

    [Header("Configuración de Colores")]
    public Color colorNormal = Color.white;
    public Color colorAcierto = Color.green;

    [Header("Configuración de Audio")]
    public AudioClip sfxAcierto, sfxFallo;

    [Header("Configuración de Detección")]
    [Tooltip("Tiempo que permanece el feedback verde después de impactar (segundos).")]
    public float duracionFeedbackAcierto = 0.5f;

    private AudioSource audioSource;
    private bool estaAcertando = false;
    private float tiempoUltimoAcierto = -999f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (imagenPunto == null)
        {
            // Intenta encontrar la imagen en los hijos si no se asignó manual
            imagenPunto = GetComponentInChildren<Image>();
        }

        // Configurar el estado inicial
        SetFeedbackState(false);
    }

    void Update()
    {
        // Verificar si el tiempo de feedback ha expirado
        if (estaAcertando && Time.time - tiempoUltimoAcierto > duracionFeedbackAcierto)
        {
            SetFeedbackState(false);
        }
    }

    // Método PÚBLICO para ser llamado desde PASSController cuando las partículas colisionan
    public void NotificarImpacto()
    {
        tiempoUltimoAcierto = Time.time;
        if (!estaAcertando)
        {
            SetFeedbackState(true);
        }
    }

    // Función auxiliar para manejar el cambio de estado
    void SetFeedbackState(bool esAcierto)
    {
        estaAcertando = esAcierto;

        if (estaAcertando)
        {
            // --- FEEDBACK POSITIVO ---
            if (imagenPunto) imagenPunto.color = colorAcierto;

            if (sfxAcierto != null)
            {
                audioSource.PlayOneShot(sfxAcierto);
            }
        }
        else
        {
            // --- FEEDBACK NORMAL ---
            if (imagenPunto) imagenPunto.color = colorNormal;

            if (sfxFallo != null)
            {
                audioSource.PlayOneShot(sfxFallo);
            }
        }
    }

}