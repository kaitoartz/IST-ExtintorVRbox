using UnityEngine;

public class ParticleRelay : MonoBehaviour
{
    // Arrastra aquí al padre (Extinguisher_Red) que tiene el ExtinguisherDischarge
    public ExtinguisherDischarge mainController;

    // Unity llama a esto en el HIJO cuando las partículas chocan
    void OnParticleCollision(GameObject other)
    {
        // Le avisamos al padre: "Oye, mis partículas chocaron con 'other'"
        if (mainController != null)
        {
            Debug.Log($"[ParticleRelay] Partículas chocaron contra '{other.name}' (tag: {other.tag})", this);
            mainController.HandleParticleCollision(other);
        }
    }
}