using UnityEngine;

[System.Serializable]
public class WeaponProbabilities
{
    public DifficultyProbabilities easyProbabilities;
    public DifficultyProbabilities mediumProbabilities;
    public DifficultyProbabilities hardProbabilities;
}

[System.Serializable]
public class DifficultyProbabilities
{
    public int pistolProbability;
    public int akProbability;
    public int sniperProbability;
}

[CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
    [Header("Time")]
    public float currentTime;
    public float maxTime;

    [Header("Enemies")]
    public float currentEnemiesDefeated;
    public float maxEnemiesToDefeat;

    [Header("Core loop")]
    public bool victory;
    public bool defeat;
    public bool challengeStarted;
    public DifficultyLevel difficulty;

    [Header("Cheats")]
    public bool isNextLevelCheatAvailiable;

    [Header("Probabilities")]
    public WeaponProbabilities weaponProbabilities;

    public float easyLevelAimingProbability;
    public float mediumLevelAimingProbability;
    public float hardLevelAimingProbability;

    public float easyLevelInaccuracyAngle;
    public float mediumLevelInaccuracyAngle;
    public float hardLevelInaccuracyAngle;

}
