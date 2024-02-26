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
    public float currentTime;
    public float currentEnemiesDefeated;
    public float maxTime;
    public float maxEnemiesToDefeat;
    public bool victory;
    public bool defeat;
    public bool challengeStarted;
    public DifficultyLevel difficulty;
    public WeaponProbabilities weaponProbabilities;
    public bool isNextLevelCheatAvailiable;

    public float easyLevelAimingProbability;
    public float mediumLevelAimingProbability;
    public float hardLevelAimingProbability;

    public float easyLevelInaccuracyAngle;
    public float mediumLevelInaccuracyAngle;
    public float hardLevelInaccuracyAngle;

}
