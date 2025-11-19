using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIMachineInfoController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Transform modelRotationRoot; // model will be parented here (optional)
    [SerializeField] private Text specsText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button closeButton;

    private MachineData currentData;
    private MachineCardController currentCard;

    public event Action<MachineData> OnAccepted;

    private void Awake()
    {
        acceptButton.onClick.AddListener(OnAccept);
        closeButton.onClick.AddListener(OnClose);
        gameObject.SetActive(false);
    }

    public void ShowInfo(MachineData data, MachineCardController card)
    {
        currentData = data;
        currentCard = card;
        nameText.text = data.machineName;
        descriptionText.text = data.description;
        specsText.text = FormatSpecs(data);
        // move the model instance to modelRotationRoot if card holds it
        if (card != null && card.transform != null)
        {
            // try to find the model in card's container and reparent it
            var model = card.transform.GetComponentsInChildren<Transform>(true)
                      .FirstOrDefault(t => t != card.transform && t.parent == card.transform);
            // Simpler: rely on factory instance created inside card: modelContainer's child
            // To avoid overcomplicating, assume the model is already in card and active; for rotation we reference card's model
        }

        gameObject.SetActive(true);
    }

    private string FormatSpecs(MachineData d)
    {
        var s = $"Voltage: {d.defaultVoltage} V\nCurrent: {d.defaultCurrent} A\nComponents:\n";
        foreach (var c in d.components) s += $"- {c}\n";
        return s;
    }

    private void OnAccept()
    {
        MachineSelectionManager.Instance.SetMachine(currentData);
        OnAccepted?.Invoke(currentData);
        // load the scene according to mode saved
        var loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            string sceneName = "WeldingPracticeScene"; // change if needed
            loader.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("SceneLoader not found in scene. Please add one.");
        }
    }

    private void OnClose()
    {
        gameObject.SetActive(false);
        currentCard?.Collapse();
    }

    private void OnDestroy()
    {
        acceptButton.onClick.RemoveListener(OnAccept);
        closeButton.onClick.RemoveListener(OnClose);
    }
}

