using UnityEngine;

public class SimulationTimer : MonoBehaviour
{
    public static SimulationTimer Instance;

    [Header("Configuración")]
    public float tiempoLimite = 60f; // Los 60 segundos que dijiste

    [Header("Referencias Visuales")]
    public Renderer cajaRenderer;
    private Material cajaMaterial;
    private Color colorOriginal;

    [Header("Estado")]
    public float tiempoTranscurrido = 0f;
    public bool estaCorriendo = false;
    public bool incendioDesatado = false;
    public bool simulacionTerminada = false;

    void Awake()
    {
        // Configuración básica del Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (cajaRenderer != null)
        {
            // Usamos el material directo para modificar el color
            cajaMaterial = cajaRenderer.material;
            colorOriginal = cajaMaterial.color;
        }
    }

    void Update()
    {
        if (estaCorriendo && !simulacionTerminada)
        {
            tiempoTranscurrido += Time.deltaTime;

            if (cajaMaterial != null)
            {
                // Interpolamos del color original a NEGRO
                cajaMaterial.color = Color.Lerp(colorOriginal, Color.black, ObtenerFactorQuemadura());
            }

            if (tiempoTranscurrido > tiempoLimite && !incendioDesatado)
            {
                incendioDesatado = true;
                Debug.Log("🔥 ¡CAGASTE TE MANDASTE UN INCENDIO!");
            }
        }
    }

    public void IniciarTemporizador()
    {
        if (!estaCorriendo)
        {
            estaCorriendo = true;
            Debug.Log("⏰ ¡Corre tiempo ctm!");
        }
    }

    public void TerminarSimulacion()
    {
        estaCorriendo = false;
        simulacionTerminada = true;
        Debug.Log("🛑 Simulación terminada. Tiempo final: " + tiempoTranscurrido);
    }

    public float ObtenerFactorQuemadura()
    {
        return Mathf.Clamp01(tiempoTranscurrido / tiempoLimite);
    }
}