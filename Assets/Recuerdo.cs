using System.Collections;
using UnityEngine;
using DG.Tweening;

// Script para el objeto coleccionable (recuerdo)
public class Recuerdo : MonoBehaviour
{
    [Header("Configuración de Animación")]
    public float rotationDuration = 2f; // Duración completa de una rotación
    public float floatAmplitude = 0.5f;
    public float floatDuration = 2f; // Duración del ciclo de flotación

    [Header("Configuración de Recolección")]
    public float scaleMultiplier = 1.5f;
    public float collectionDuration = 0.4f;

    private Vector3 startPosition;
    private RecuerdosManager sistemaRecuerdos;
    private Tween rotationTween;
    private Tween floatTween;

    void Start()
    {
        startPosition = transform.position;
        sistemaRecuerdos = FindObjectOfType<RecuerdosManager>();

        IniciarAnimaciones();
    }

    void IniciarAnimaciones()
    {
        // Rotación continua con DOTween
        rotationTween = transform.DORotate(new Vector3(0, 0, 360), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental); // Loop infinito

        // Movimiento flotante vertical con DOTween
        floatTween = transform.DOMoveY(startPosition.y + floatAmplitude, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // Yoyo para subir y bajar
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RecogerRecuerdo();
        }
    }
    
    void RecogerRecuerdo()
    {
        if (sistemaRecuerdos != null)
        {
            sistemaRecuerdos.RecogerRecuerdo();
        }

        // Detener las animaciones de idle
        DetenerAnimaciones();

        // Efecto de recolección mejorado
        EfectoRecoleccionDOTween();
    }

    void DetenerAnimaciones()
    {
        // Matar las animaciones actuales
        rotationTween?.Kill();
        floatTween?.Kill();
    }

    void EfectoRecoleccionDOTween()
    {
        // Secuencia de animación de recolección
        Sequence collectSequence = DOTween.Sequence();

        // Escalar hacia arriba
        collectSequence.Append(transform.DOScale(Vector3.one * scaleMultiplier, collectionDuration * 0.6f)
            .SetEase(Ease.OutBack));

        // Escalar hacia abajo y desvanecer (si tienes SpriteRenderer)
        collectSequence.Append(transform.DOScale(Vector3.zero, collectionDuration * 0.4f)
            .SetEase(Ease.InBack));

        // Opcional: Efecto de desvanecimiento si tienes SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            collectSequence.Join(spriteRenderer.DOFade(0f, collectionDuration)
                .SetEase(Ease.InQuad));
        }

        // Destruir el objeto al completar la animación
        collectSequence.OnComplete(() => Destroy(gameObject));
    }

    void OnDestroy()
    {
        // Limpiar tweens al destruir el objeto
        rotationTween?.Kill();
        floatTween?.Kill();
    }
}