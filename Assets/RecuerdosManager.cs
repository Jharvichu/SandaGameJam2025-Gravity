using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecuerdosManager : MonoBehaviour
{
    [System.Serializable]
    public class DatosRecuerdo
    {
        public Sprite imagen;
        public AudioClip sonido;
        [TextArea(2, 4)]
        public string descripcion;
    }
    
    [Header("Configuración de Recuerdos")]
    public DatosRecuerdo[] recuerdos;
    public float tiempoMostrarRecuerdo = 3f;
    public float tiempoFade = 1f;
    
    [Header("UI References")]
    public GameObject panelRecuerdo;
    public Image imagenRecuerdo;
    public TextMeshProUGUI textoDescripcion; // Opcional
    public CanvasGroup canvasGroup;
    public AudioSource audioSource;
    
    [Header("Sonidos")]
    public AudioClip sonidoRecoleccion;
    
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
        
        // Asegurar que el CanvasGroup empiece invisible
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
        
        // Activar el panel
        if (panelRecuerdo != null)
            panelRecuerdo.SetActive(true);
        
        // Configurar la imagen del recuerdo (cambiar el sprite)
        if (imagenRecuerdo != null)
            imagenRecuerdo.sprite = recuerdo.imagen;
            
        if (textoDescripcion != null)
            textoDescripcion.text = recuerdo.descripcion;
        
        // Fade In
        yield return StartCoroutine(FadeIn());
        
        // Reproducir sonido del recuerdo
        if (recuerdo.sonido != null && audioSource != null)
        {
            audioSource.PlayOneShot(recuerdo.sonido);
        }
        
        // Esperar tiempo configurado
        yield return new WaitForSeconds(tiempoMostrarRecuerdo);
        
        // Fade Out
        yield return StartCoroutine(FadeOut());
        
        // Desactivar el panel después del fade
        if (panelRecuerdo != null)
            panelRecuerdo.SetActive(false);
            
        mostrandoRecuerdo = false;
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
}