using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ZoneFeedback : MonoBehaviour
{
    [Header("Configuración de la Alerta")]
    public Color colorPeligro = Color.red;
    public SpriteRenderer spriteRenderer;
    public AudioClip sonidoAlerta;
    public string tagDelJugador = "Player";
    public GameObject uiAlerta; // UI de alerta

    [Header("Configuración de Vignette")]
    public Volume globalVolume;
    public float intensidadPulsacion = 0.5f;
    public float velocidadPulsacion = 2.0f;

    [Header("Nombre de la Propiedad en ShaderGraph")]
    public string nombrePropiedadColor = "_Color";

    private Renderer miRenderer;
    private AudioSource miAudio;
    private Color colorOriginal;

    private Vignette vignette;
    private Color colorVignetteOriginal;
    private float intensidadVignetteOriginal;
    private bool alertaActiva = false;

    void Start()
    {
        miRenderer = GetComponent<Renderer>();
        miAudio = GetComponent<AudioSource>();

        if (miAudio == null)
        {
            Debug.LogWarning("Oye weon, te falta el AudioSource en el objeto " + gameObject.name);
        }

        // Guardamos el color original por si quisieras volver a él después (opcional)
        if (miRenderer != null)
        {
            // Intenta obtener el color actual
            if (miRenderer.material.HasProperty(nombrePropiedadColor))
                colorOriginal = miRenderer.material.GetColor(nombrePropiedadColor);
        }

        // Inicializar Vignette
        if (globalVolume != null)
        {
            if (globalVolume.profile.TryGet(out vignette))
            {
                colorVignetteOriginal = vignette.color.value;
                intensidadVignetteOriginal = vignette.intensity.value;
            }
        }

        if (uiAlerta != null)
        {
            uiAlerta.SetActive(false);
        }
    }

    void Update()
    {
        if (alertaActiva && vignette != null)
        {
            // Pulsar la intensidad
            float pulsacion = Mathf.PingPong(Time.time * velocidadPulsacion, intensidadPulsacion);
            vignette.intensity.value = intensidadVignetteOriginal + pulsacion;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificamos si lo que tocó el trigger es el Jugador
        if (other.CompareTag(tagDelJugador))
        {
            activarAlerta();
        }
    }

    void activarAlerta()
    {
        // 1. Cambiar el Color a Rojo
        if (miRenderer != null && spriteRenderer != null)
        {
            spriteRenderer.color = colorPeligro;
            miRenderer.material.SetColor(nombrePropiedadColor, colorPeligro);
        }

        // 2. Tocar el Sonido
        if (miAudio != null && sonidoAlerta != null)
        {
            miAudio.PlayOneShot(sonidoAlerta);
        }

        // 3. Activar UI
        if (uiAlerta != null)
        {
            uiAlerta.SetActive(true);
        }

        // 4. Activar Vignette Rojo y Pulsación
        if (vignette != null)
        {
            vignette.color.value = Color.red;
            alertaActiva = true;
        }

        Debug.Log("¡CUIDADO CONCHETUMARE! 🚨 Zona activada.");
    }

    // Opcional: Si quieres que vuelva a ser azul cuando sales
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagDelJugador))
        {
            if (miRenderer != null && spriteRenderer != null)
            {
                spriteRenderer.color = colorOriginal;
                // Descomenta la línea de abajo si quieres que vuelva al azul
                miRenderer.material.SetColor(nombrePropiedadColor, colorOriginal);
            }

            // Desactivar UI
            if (uiAlerta != null)
            {
                uiAlerta.SetActive(false);
            }

            // Restaurar Vignette
            if (vignette != null)
            {
                alertaActiva = false;
                vignette.color.value = colorVignetteOriginal;
                vignette.intensity.value = intensidadVignetteOriginal;
            }
        }
    }
}