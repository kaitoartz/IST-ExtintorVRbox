using UnityEngine;
using DG.Tweening;

public class MoveAndBackTweenUI : MonoBehaviour
{
    [Header("Configuración de la Animación")]
    [SerializeField] private float distanciaMovimiento = 200f; // Distancia hacia la izquierda
    [SerializeField] private float duracionMovimiento = .2f; // Duración de cada movimiento
    [SerializeField] private float delayEntreMovimientos = 0.5f; // Tiempo de espera antes de volver

    private RectTransform rectTransform;
    private Vector2 posicionInicial;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        posicionInicial = rectTransform.anchoredPosition;

        IniciarAnimacion();
    }

    void IniciarAnimacion()
    {
        Sequence secuencia = DOTween.Sequence();

        secuencia.Append(rectTransform.DOAnchorPosX(posicionInicial.x - distanciaMovimiento, duracionMovimiento))
            .SetEase(Ease.OutQuad);

        secuencia.AppendInterval(delayEntreMovimientos);

        secuencia.Append(rectTransform.DOAnchorPosX(posicionInicial.x, duracionMovimiento))
            .SetEase(Ease.InQuad);

        secuencia.SetLoops(-1, LoopType.Restart);
    }
}