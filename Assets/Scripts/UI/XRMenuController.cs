using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class XRMenuController : MonoBehaviour
{
    public GameObject menuCanvas; // El Canvas completo del menú
    public GameObject panelOpciones; // Panel de opciones
    public GameObject panelCreditos; // Panel de créditos
    public Light luzPrincipal; // Referencia a la luz principal de la escena

    private bool isMenuVisible = false;
    private float intensidadLuzOriginal;
    private bool transicionEnCurso = false;

    void Start()
    {
        // Guardar la intensidad original de la luz
        if (luzPrincipal != null)
        {
            intensidadLuzOriginal = luzPrincipal.intensity;
        }
    }

    void Update()
    {
        // Abrir/cerrar el menú con la tecla "M" (puedes cambiar la tecla)
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;
        menuCanvas.SetActive(isMenuVisible); // Activa o desactiva el Canvas completo
    }

    // Método para el botón "Iniciar"
    public void OnIniciarPressed()
    {
        if (transicionEnCurso) return;

        Debug.Log("Iniciar juego...");
        StartCoroutine(TransicionInicioJuego());
    }

    private System.Collections.IEnumerator TransicionInicioJuego()
    {
        transicionEnCurso = true;

        // Efecto de "cerrar la luz" - oscurecer gradualmente
        if (luzPrincipal != null)
        {
            // Reducir intensidad de la luz gradualmente hasta quedar completamente oscuro
            luzPrincipal.DOIntensity(0f, 2f).SetEase(Ease.InQuad);
        }

        // También podemos oscurecer otras luces en la escena
        Light[] todasLasLuces = FindObjectsOfType<Light>();
        foreach (Light luz in todasLasLuces)
        {
            if (luz != luzPrincipal)
            {
                luz.DOIntensity(0f, 2f).SetEase(Ease.InQuad);
            }
        }

        // Esperar a que termine la transición de oscurecimiento
        yield return new WaitForSeconds(2f);

        // Cargar la escena del juego (en completa oscuridad)
        SceneManager.LoadScene("MainVR");
    }

    // Método para el botón "Opciones"
    public void OnOpcionesPressed()
    {
        Debug.Log("Mostrando opciones...");
        panelOpciones.SetActive(true); // Muestra el panel de opciones
        panelCreditos.SetActive(false); // Oculta el panel de créditos
    }

    // Método para el botón "Créditos"
    public void OnCreditosPressed()
    {
        Debug.Log("Mostrando créditos...");
        panelCreditos.SetActive(true); // Muestra el panel de créditos
        panelOpciones.SetActive(false); // Oculta el panel de opciones
    }

    // Método para el botón "Salir"
    public void OnSalirPressed()
    {
        Debug.Log("Saliendo del juego...");

        // Efecto de cerrar luz antes de salir
        if (luzPrincipal != null)
        {
            luzPrincipal.DOIntensity(0f, 1.5f).SetEase(Ease.InQuad)
                .OnComplete(() => Application.Quit());
        }
        else
        {
            Application.Quit(); // Cierra la aplicación
        }
    }

    // Método para cerrar panels
    public void CerrarPaneles()
    {
        panelOpciones.SetActive(false);
        panelCreditos.SetActive(false);
    }
}