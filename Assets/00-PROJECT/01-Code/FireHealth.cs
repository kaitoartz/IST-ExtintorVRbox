using UnityEngine;

public class FireHealth : MonoBehaviour
{
    [Header("--- Configuración del Fuego ---")]
    [Tooltip("Vida total del fuego (ej: 100)")]
    public float maxHealth = 100f;
    [Tooltip("Tipo de fuego (A, B, C, K, etc.)")]
    public string fireType = "A";

    private float currentHealth;
    private ParticleSystem[] fireParticles;

    void Start()
    {
        currentHealth = maxHealth;
        // Agarra todas las partículas hijas para achicarlas después
        fireParticles = GetComponentsInChildren<ParticleSystem>();
    }

    // Esta es la función pública que el extintor va a llamar
    public void TakeDamage(float amount, string extinguisherType)
    {
        // 1. Verificamos si el tipo de extintor sirve pa este fuego
        // (Aquí puedes poner tu lógica compleja de tipos)
        if (!extinguisherType.Contains(fireType) && !extinguisherType.Contains("Universal"))
        {
            // Debug.Log("❌ Este extintor no sirve pa este fuego wn!");
            return;
        }

        // 2. Bajamos la vida
        currentHealth -= amount;

        // 3. Feedback visual (Achicarse)
        UpdateVisuals();

        // 4. Muerte
        if (currentHealth <= 0)
        {
            Extinguish();
        }
    }

    void UpdateVisuals()
    {
        float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);

        foreach (var ps in fireParticles)
        {
            // Ignoramos hitboxes o humo si quieres
            if (ps.name.Contains("Hitbox")) continue;

            // Achicamos basado en el % de vida restante
            // Guardamos la escala inicial en el Start si quieres ser más preciso, 
            // pero esto sirve de ejemplo rápido.
            var main = ps.main;
            // Un truco sucio para escalar partículas es bajar el StartSize o escalar el transform
            ps.transform.localScale = Vector3.one * healthPercent;
        }
    }

    void Extinguish()
    {
        Debug.Log($"💀 Fuego {name} apagado ctm!");
        gameObject.SetActive(false);
        SimulationTimer.Instance.TerminarSimulacion();
        // Aquí podrías llamar a un GameManger para sumar puntos
    }
}