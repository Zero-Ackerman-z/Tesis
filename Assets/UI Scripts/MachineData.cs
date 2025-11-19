using UnityEngine;

[CreateAssetMenu(menuName = "Machines/MachineData")]
public class MachineData : ScriptableObject
{
    public string machineName;
    [TextArea] public string description;
    public MachineType machineType; // enum AC/DC (create below)
    public GameObject modelPrefab; // prefab of 3D model
    public Sprite thumbnail; // optional UI image
    [Header("Technical Specs")]
    public float defaultVoltage;
    public float defaultCurrent;
    public string[] components; // list of components
}
public enum MachineType { AC, DC }


