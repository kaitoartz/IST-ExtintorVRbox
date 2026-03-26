using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit; // Importante

public class FireExtinguisherController : MonoBehaviour
{
    [Header("Referencias")]
    //[SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private AudioSource sprayAudio;

    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;
    private string extinguisherTag; 
    private float extinguishRate = 0.8f;
    private float emissionRate = 0.6f;

    void Awake() // Usamos Awake para inicializar referencias
    {
        part = GetComponent<ParticleSystem>();
        extinguisherTag = this.transform.tag;
        collisionEvents = new List<ParticleCollisionEvent>();

        //if (grabInteractable == null) grabInteractable = GetComponent<XRGrabInteractable>();

        // Asegurar que parta apagado
        var emission = part.emission;
        emission.enabled = false;
    }

    // Nos suscribimos a los eventos cuando el objeto se habilita
    private void OnEnable()
    {
        //if (grabInteractable != null)
        {
            // "activated" ocurre cuando aprietas el gatillo/botón mientras agarras el objeto
            //grabInteractable.activated.AddListener(OnSprayActivated);

            // "deactivated" ocurre cuando sueltas el gatillo/botón
            //grabInteractable.deactivated.AddListener(OnSprayDeactivated);
        }
    }

    // Nos desuscribimos para evitar errores de memoria
    private void OnDisable()
    {
        //if (grabInteractable != null)
        {
            //grabInteractable.activated.RemoveListener(OnSprayActivated);
            //grabInteractable.deactivated.RemoveListener(OnSprayDeactivated);
        }
    }

    // La firma del método debe recibir ActivateEventArgs aunque no lo usemos
    /*private void OnSprayActivated(ActivateEventArgs args)
    {
        Debug.Log("¡Disparando extintor!");
        StartSpray();
    }*/

    /*private void OnSprayDeactivated(DeactivateEventArgs args)
    {
        Debug.Log("Dejando de disparar.");
        StopSpray();
    }*/

    private void StartSpray()
    {
        var emission = part.emission;
        emission.enabled = true;

        if (sprayAudio != null && !sprayAudio.isPlaying)
        {
            sprayAudio.Play();
        }
    }

    private void StopSpray()
    {
        var emission = part.emission;
        emission.enabled = false;

        if (sprayAudio != null)
        {
            sprayAudio.Stop();
        }
    }

    // --- TU LÓGICA DE COLISIÓN (IGUAL QUE ANTES) ---
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            if (other.CompareTag("Fire A") || other.CompareTag("Fire ABC") || other.CompareTag("Fire BC"))
            {
                if (CanExtinguish(other.tag))
                {
                    ReduceFireSizeAndEmission(other.transform, numCollisionEvents);
                }
            }
        }
    }

    private bool CanExtinguish(string fireTag)
    {
        return fireTag.Substring(fireTag.Length - 1) == extinguisherTag.Substring(extinguisherTag.Length - 1);
    }

    private void ReduceFireSizeAndEmission(Transform fireTransform, int numCollisions)
    {
        // Tu lógica matemática exacta para reducir el fuego
        // (La omito para no ocupar espacio, pero pégala aquí tal cual la tenías)
        foreach (ParticleSystem ps in fireTransform.GetComponentsInChildren<ParticleSystem>())
        {
            Vector3 newScale = ps.transform.localScale - Vector3.one * extinguishRate * Time.deltaTime / numCollisions;
            ps.transform.localScale = newScale;

            var emission = ps.emission;
            var emissionRateOverTime = emission.rateOverTime.constant;
            emissionRateOverTime = Mathf.Max(0, emissionRateOverTime - emissionRate * Time.deltaTime / numCollisions);
            var emissionRateOverTimeCurve = new ParticleSystem.MinMaxCurve(emissionRateOverTime);
            emission.rateOverTime = emissionRateOverTimeCurve;

            if (newScale.x <= 0)
            {
                fireTransform.gameObject.SetActive(false);
                return;
            }
        }
    }
}