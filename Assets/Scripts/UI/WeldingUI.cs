using UnityEngine;
using UnityEngine.UI;

public class WeldingUI : MonoBehaviour
{
    public PistolaSphereCastMerge weldingGun; // Referencia a la pistola de soldadura
    public Text voltageText; // Texto para mostrar el voltaje
    public Text wireSpeedText; // Texto para mostrar la velocidad del cable
    public Text totalTimeText; // Texto para mostrar el tiempo total
    public Text resultText; // Texto para mostrar el resultado final

    public void UpdateUI(float voltage, float wireSpeed, float totalTime, string result)
    {
        // Actualizar los textos de la UI con los valores correspondientes
        voltageText.text = "Voltaje: " + voltage.ToString("F1") + " volts";
        wireSpeedText.text = "Velocidad de cable: " + wireSpeed.ToString("F0") + " ipm";
        totalTimeText.text = "Tiempo total: " + totalTime.ToString("F0") + " segundos";
        resultText.text = "Resultado final: " + result;
    }
}