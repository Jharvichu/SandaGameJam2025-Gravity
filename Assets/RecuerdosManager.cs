using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecuerdosManager : MonoBehaviour
{
    [System.Serializable]
    public class SpriteRecuerdo
    {
        public Sprite imagen;
        [TextArea(2, 4)]
        public string descripcion;
    }

    [System.Serializable]
    public class DatosRecuerdo
    {
        [Header("Imágenes del Recuerdo")]
        public SpriteRecuerdo[] spritesRecuerdo;

        [Header("Audio")]
        public AudioClip sonido;

        [Header("Configuración")]
        public float tiempoEntreFotos = 0.5f; // Tiempo entre cada imagen del mismo recuerdo
    }

    [Header("Configuración de Recuerdos")]
    public DatosRecuerdo[] recuerdos;
    public float tiempoMostrarCadaImagen = 3f;
    public float tiempoFade = 1f;
    
    [Header("UI References")]
    public GameObject panelRecuerdo;
    public Image imagenRecuerdo;
    public TextMeshProUGUI textoDescripcion;
    public TextMeshProUGUI contadorImagenes; // Opcional: "1/3", "2/3", etc.
    public CanvasGroup canvasGroup;
    public AudioSource audioSource;
    
    [Header("Sonidos")]
    public AudioClip sonidoRecoleccion;
    public AudioClip sonidoCambioImagen; // Sonido al cambiar de imagen

    private int recuerdosRecogidos = 0;
    private int totalRecuerdos;
    private bool mostrandoRecuerdo = false;
    
    void Start()
    {
        totalRecuerdos = FindObjectsOfType<Recuerdo>().Length;

        // Configurar panel inicial
        if (panelRecuerdo != null)
            panelRecuerdo.SetActive(false);
            
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (contadorImagenes != null)
            contadorImagenes.gameObject.SetActive(false);
    }
    
    public void RecogerRecuerdo()
    {
        if (mostrandoRecuerdo) return;
        
        // Reproducir sonido de recolección
        if (sonidoRecoleccion != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoRecoleccion);
        }
        
        recuerdosRecogidos++;
        
        // Verificar si hay un recuerdo que desbloquear
        int indiceRecuerdo = recuerdosRecogidos - 1;
        if (indiceRecuerdo < recuerdos.Length)
        {
            StartCoroutine(MostrarRecuerdo(indiceRecuerdo));
        }
        
        // Verificar si se han recogido todos
        if (recuerdosRecogidos >= totalRecuerdos)
        {
            OnTodosLosRecuerdosRecogidos();
        }
        
        Debug.Log($"Recuerdos recogidos: {recuerdosRecogidos}/{totalRecuerdos}");
    }
    
    IEnumerator MostrarRecuerdo(int indice)
    {
        if (indice >= recuerdos.Length) yield break;
        
        mostrandoRecuerdo = true;
        DatosRecuerdo recuerdo = recuerdos[indice];

        // Validar que el recuerdo tenga imágenes
        if (recuerdo.spritesRecuerdo == null || recuerdo.spritesRecuerdo.Length == 0)
        {
            Debug.LogWarning($"El recuerdo {indice} no tiene imágenes asignadas.");
            mostrandoRecuerdo = false;
            yield break;
        }

        // Activar el panel
        if (panelRecuerdo != null)
            panelRecuerdo.SetActive(true);

        // Mostrar contador si hay múltiples imágenes
        bool tieneMultiplesImagenes = recuerdo.spritesRecuerdo.Length > 1;
        if (contadorImagenes != null)
            contadorImagenes.gameObject.SetActive(tieneMultiplesImagenes);

        // Reproducir sonido del recuerdo al inicio (solo una vez)
        if (recuerdo.sonido != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = recuerdo.sonido;
            audioSource.Play();
        }

        // Mostrar cada imagen del recuerdo
        for (int i = 0; i < recuerdo.spritesRecuerdo.Length; i++)
        {
            yield return StartCoroutine(MostrarImagenIndividual(recuerdo.spritesRecuerdo[i], i + 1, recuerdo.spritesRecuerdo.Length));

            // Si no es la última imagen, esperar un poco y reproducir sonido de cambio
            if (i < recuerdo.spritesRecuerdo.Length - 1)
            {
                yield return new WaitForSeconds(recuerdo.tiempoEntreFotos);

                // Reproducir sonido de cambio de imagen
                if (sonidoCambioImagen != null && audioSource != null)
                {
                    audioSource.PlayOneShot(sonidoCambioImagen);
                }
            }
        }

        // Desactivar el panel después de mostrar todas las imágenes
        if (panelRecuerdo != null)
            panelRecuerdo.SetActive(false);

        if (contadorImagenes != null)
            contadorImagenes.gameObject.SetActive(false);

        mostrandoRecuerdo = false;
    }

    IEnumerator MostrarImagenIndividual(SpriteRecuerdo spriteRecuerdo, int numeroImagen, int totalImagenes)
    {
        // Asegurar que el CanvasGroup empiece invisible
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        // Configurar la imagen y descripción
        if (spriteRecuerdo != null && spriteRecuerdo.imagen != null)
            imagenRecuerdo.sprite = spriteRecuerdo.imagen;

        if (textoDescripcion != null)
            textoDescripcion.text = spriteRecuerdo.descripcion;

        // Actualizar contador
        if (contadorImagenes != null && totalImagenes > 1)
            contadorImagenes.text = $"{numeroImagen}/{totalImagenes}";

        // Fade In
        yield return StartCoroutine(FadeIn());

        // Esperar tiempo configurado
        yield return new WaitForSeconds(tiempoMostrarCadaImagen);

        // Fade Out
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        if (canvasGroup == null) yield break;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < tiempoFade)
        {
            float progress = elapsedTime / tiempoFade;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        canvasGroup.alpha = 1f;
    }
    
    IEnumerator FadeOut()
    {
        if (canvasGroup == null) yield break;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < tiempoFade)
        {
            float progress = elapsedTime / tiempoFade;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
    }
    
    void OnTodosLosRecuerdosRecogidos()
    {
        Debug.Log("¡Todos los recuerdos han sido recogidos!");
        // Aquí puedes agregar lógica adicional como:
        // - Mostrar un mensaje final
        // - Desbloquear algo especial
        // - Cambiar de escena
        // - Etc.
    }
    
    // Métodos públicos para obtener información
    public int GetRecuerdosRecogidos()
    {
        return recuerdosRecogidos;
    }
    
    public int GetTotalRecuerdos()
    {
        return totalRecuerdos;
    }
    
    public float GetProgreso()
    {
        return totalRecuerdos > 0 ? (float)recuerdosRecogidos / totalRecuerdos : 0f;
    }

    // Método para obtener información sobre un recuerdo específico
    public int GetCantidadImagenesRecuerdo(int indice)
    {
        if (indice >= 0 && indice < recuerdos.Length && recuerdos[indice].spritesRecuerdo != null)
            return recuerdos[indice].spritesRecuerdo.Length;
        return 0;
    }
}