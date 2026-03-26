using UnityEngine;

public class ExtinguisherDischarge : MonoBehaviour
{
    [Header("--- Visuales y Física ---")]
    public ParticleSystem[] sprayVFX;
    public AudioSource sprayAudio;
    public AimFeedback aimFeedback; // Si lo usas

    private bool isSpraying = false;

    // --- MÉTODOS PÚBLICOS (El Jefe llama a esto) ---
    public void StartSpraying()
    {
        if (isSpraying) return;
        isSpraying = true;

        if (sprayAudio && !sprayAudio.isPlaying) sprayAudio.Play();

        foreach (var vfx in sprayVFX)
        {
            if (vfx != null)
            {
                if (!vfx.isPlaying) vfx.Play();
                var col = vfx.collision;
                col.enabled = true; // Activar colisión física
            }
        }
    }

    public void StopSpraying()
    {
        if (!isSpraying) return;
        isSpraying = false;

        if (sprayAudio && sprayAudio.isPlaying) sprayAudio.Stop();

        foreach (var vfx in sprayVFX)
        {
            if (vfx != null && vfx.isPlaying) vfx.Stop();
        }
    }

    // --- PUENTE DE COLISIÓN (Lo llama el ParticleRelay) ---
    public void HandleParticleCollision(GameObject other)
    {
        // Aquí conectas con tu FireHealth como lo tenías antes
        FireHealth fire = other.GetComponent<FireHealth>();
        Debug.Log("ExtinguisherDischarge: Particle collided with " + other.name);
        if (fire == null) fire = other.GetComponentInParent<FireHealth>();

        if (fire != null)
        {
            // Calcula daño o llama a tu lógica de reducción
            fire.TakeDamage(1f, "ABC"); // Ajusta el daño según necesites
        }
    }
}