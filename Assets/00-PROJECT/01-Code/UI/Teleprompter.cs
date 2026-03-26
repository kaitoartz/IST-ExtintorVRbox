using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Teleprompter : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float delayInicial = 3f; // Tiempo de espera antes de comenzar el scroll
    [SerializeField] private float velocidadScroll = 50f; // Velocidad de desplazamiento del texto
    [SerializeField] private float stopPositionY = 1000f; // Posición Y donde el teleprompter se detendrá

    private RectTransform rectTransform;
    private bool comenzarScroll = false;
    private bool haTerminado = false; // Para evitar múltiples llamadas al evento de fin

    [Header("Eventos")]
    [SerializeField] private UnityEvent onTeleprompterEnd = new UnityEvent();
    [SerializeField] private UnityEvent onTeleprompterStart = new UnityEvent();

    void Start()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        Invoke("ComenzarScroll", delayInicial);
    }

    private void Update()
    {
        if (comenzarScroll && !haTerminado)
        {
            rectTransform.anchoredPosition += Vector2.up * velocidadScroll * Time.deltaTime;

            if (rectTransform.anchoredPosition.y >= stopPositionY)
            {
                DetenerScroll();
            }
        }
    }

    public void ComenzarScroll()
    {
        comenzarScroll = true;
        haTerminado = false;
        // Notificar inicio del teleprompter
        onTeleprompterStart.Invoke();
    }

    void DetenerScroll()
    {
        // Detener el scroll
        comenzarScroll = false;
        haTerminado = true;

        // Notificar fin del teleprompter
        onTeleprompterEnd.Invoke();
    }

    public void ReiniciarScroll()
    {
        Vector2 restarPositionY = Vector2.zero;
        rectTransform.anchoredPosition = restarPositionY;
        Invoke("ComenzarScroll", delayInicial);
    }
}