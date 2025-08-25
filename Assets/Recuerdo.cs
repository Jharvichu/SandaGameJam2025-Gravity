using System.Collections;
using UnityEngine;

// Script para el objeto coleccionable (recuerdo)
public class Recuerdo : MonoBehaviour
{
    [Header("Configuración")]
    public float rotationSpeed = 50f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    
    private Vector3 startPosition;
    private RecuerdosManager sistemaRecuerdos;
    
    void Start()
    {
        startPosition = transform.position;
        sistemaRecuerdos = FindObjectOfType<RecuerdosManager>();
    }
    
    void Update()
    {
        // Rotación continua
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        
        // Movimiento flotante
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
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
        
        // Efecto de recolección
        StartCoroutine(EfectoRecoleccion());
    }
    
    IEnumerator EfectoRecoleccion()
    {
        // Animación de recolección
        float duration = 0.3f;
        float elapsedTime = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;
        
        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}