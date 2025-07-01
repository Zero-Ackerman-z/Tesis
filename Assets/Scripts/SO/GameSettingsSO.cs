using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "WeldingSim/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    public float defaultVoltage = 22f;
    public float defaultWireSpeed = 385f;
    public DifficultyLevel[] difficultyLevels;

    public class DifficultyLevel
    {
        public string name;
        public float voltageMultiplier;
        public float speedMultiplier;
    }
}
