using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FadeEffect : MonoBehaviour
{
    [Header("Configuración de Fade")]
    [SerializeField] private float duracionFade = 2f;
    [SerializeField] private float delayInicial = 0f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [FormerlySerializedAs("isStartFade")] [SerializeField] private bool isStartFadeOut = false;
    
    [Header("Referencias")]
    [SerializeField] private List<TextMeshProUGUI> textosTMP;
    [SerializeField] private List<Image> imagenes;
    [SerializeField] private List<RawImage> rawImagenes;
    [SerializeField] private List<GameObject> poseGestos;
    [SerializeField] private GameObject movementPlayer;

    // Lista de objetos para destruir después del fade
    [SerializeField] private List<GameObject> objetosADestruir = new List<GameObject>();
    
    [Header("Eventos")]
    [SerializeField] private UnityEvent onFadeComplete = new UnityEvent();
    [SerializeField] private UnityEvent onFadeStart = new UnityEvent();

    public float GetDefaultDuration() => duracionFade;

    // Interfaces para manejar objetos fadeables
    private interface IFadeable
    {
        void SetAlpha(float alpha);
        float GetAlpha();
    }

    // Clase genérica para manejar cualquier componente gráfico (Image, RawImage, TextMeshProUGUI, etc.)
    private class GraphicFadeable : IFadeable
    {
        private Graphic graphic;

        public GraphicFadeable(Graphic graphic)
        {
            this.graphic = graphic;
        }

        public float GetAlpha() => graphic.color.a;

        public void SetAlpha(float alpha)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }

    private List<IFadeable> fadeables = new List<IFadeable>();
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Inicializar la lista de fadeables una sola vez
        foreach (var texto in textosTMP)
        {
            if (texto != null)
                fadeables.Add(new GraphicFadeable(texto));
        }

        foreach (var imagen in imagenes)
        {
            if (imagen != null)
                fadeables.Add(new GraphicFadeable(imagen));
        }

        foreach (var rawImage in rawImagenes)
        {
            if (rawImage != null)
                fadeables.Add(new GraphicFadeable(rawImage));
        }
    }

    private void Start()
    {
        if (movementPlayer != null)
            movementPlayer.SetActive(false);
            
        if (poseGestos.Count > 0)
        {
            foreach (var gesto in poseGestos)
            {
                if (gesto != null)
                    gesto.SetActive(false);
            }
        }

        if (isStartFadeOut)
        {
            IniciarFadeOut();
        }
    }

    // Método para agregar objetos a la lista de destrucción
    public void AgregarObjetoADestruir(GameObject objeto)
    {
        if (objeto != null && !objetosADestruir.Contains(objeto))
        {
            objetosADestruir.Add(objeto);
        }
    }

    // Método para agregar imagen para hacer fade
    public void AgregarImagenParaFade(Image imagen)
    {
        if (imagen != null && !imagenes.Contains(imagen))
        {
            imagenes.Add(imagen);
            fadeables.Add(new GraphicFadeable(imagen));
        }
    }

    // Método para agregar RawImage para hacer fade
    public void AgregarRawImageParaFade(RawImage rawImage)
    {
        if (rawImage != null && !rawImagenes.Contains(rawImage))
        {
            rawImagenes.Add(rawImage);
            fadeables.Add(new GraphicFadeable(rawImage));
        }
    }

    // Método para agregar texto TMP para hacer fade
    public void AgregarTextoParaFade(TextMeshProUGUI texto)
    {
        if (texto != null && !textosTMP.Contains(texto))
        {
            textosTMP.Add(texto);
            fadeables.Add(new GraphicFadeable(texto));
        }
    }
    
    public void IniciarFadeIn()
    {
        if (movementPlayer != null)
            movementPlayer.SetActive(false);
            
        // Detener cualquier fade en progreso
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
            
        fadeCoroutine = StartCoroutine(EjecutarFade(0f, 1f));
    }
    
    public void IniciarFadeOut()
    {
        if (movementPlayer != null)
            movementPlayer.SetActive(true);
            
        // Detener cualquier fade en progreso
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
            
        fadeCoroutine = StartCoroutine(EjecutarFade(1f, 0f));
    }

    // Método general para iniciar fade con valores personalizados
    public void IniciarFadePersonalizado(float alphaInicial, float alphaFinal, float duracion = -1f, float delay = -1f)
    {
        if (duracion < 0) duracion = duracionFade;
        if (delay < 0) delay = delayInicial;
        
        // Detener cualquier fade en progreso
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
            
        fadeCoroutine = StartCoroutine(EjecutarFade(alphaInicial, alphaFinal, duracion, delay));
    }

    // Agregar un listener al evento de finalización del fade
    public void AgregarListenerAlCompletar(UnityAction action)
    {
        onFadeComplete.AddListener(action);
    }
    
    // Quitar un listener del evento de finalización del fade
    public void QuitarListenerAlCompletar(UnityAction action)
    {
        onFadeComplete.RemoveListener(action);
    }

    private IEnumerator EjecutarFade(float alphaInicial, float alphaFinal, float duracion = -1f, float delay = -1f)
    {
        if (duracion < 0) duracion = duracionFade;
        if (delay < 0) delay = delayInicial;
        
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        // Notificar inicio del fade
        onFadeStart.Invoke();

        // Establecer valor inicial para todos los elementos
        foreach (var fadeable in fadeables)
        {
            fadeable.SetAlpha(alphaInicial);
        }

        float tiempoTranscurrido = 0f;
        
        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float progreso = Mathf.Clamp01(tiempoTranscurrido / duracion);
            float valorCurva = fadeCurve.Evaluate(progreso);
            float alphaCurrent = Mathf.Lerp(alphaInicial, alphaFinal, valorCurva);

            // Aplicar alpha a todos los elementos
            foreach (var fadeable in fadeables)
            {
                fadeable.SetAlpha(alphaCurrent);
            }

            yield return null;
        }

        // Asegurar valor final exacto
        foreach (var fadeable in fadeables)
        {
            fadeable.SetAlpha(alphaFinal);
        }
        
        // Destruir los objetos de la lista si hay alguno
        DestruirObjetosPendientes();

        // Invocar los eventos de finalización
        onFadeComplete.Invoke();

        fadeCoroutine = null;
    }

    // Método para destruir los objetos pendientes
    private void DestruirObjetosPendientes()
    {
        if (objetosADestruir.Count > 0)
        {
            List<GameObject> objetosARemover = new List<GameObject>();
            
            foreach (var obj in objetosADestruir)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
                objetosARemover.Add(obj);
            }
            
            // Limpiar los objetos ya destruidos de la lista
            foreach (var obj in objetosARemover)
            {
                objetosADestruir.Remove(obj);
            }
        }
    }
    
    // Método público para comprobar si hay un fade en progreso
    public bool EstaFadeando()
    {
        return fadeCoroutine != null;
    }
}