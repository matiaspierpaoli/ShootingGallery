using UnityEngine;

public class EnemyWeaponManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GameObject pistolObject;
    [SerializeField] private GameObject akObject;
    [SerializeField] private GameObject sniperObject;
    [SerializeField] private GameData gameData;

    private void Awake()
    {
        pistolObject.SetActive(false);
        akObject.SetActive(false);
        sniperObject.SetActive(false);

        ChooseWeapon();
    }

    private void ChooseWeapon()
    {
        DifficultyProbabilities probabilities;

        switch (gameData.difficulty)
        {
            case DifficultyLevel.Easy:
                probabilities = gameData.weaponProbabilities.easyProbabilities;
                break;
            case DifficultyLevel.Medium:
                probabilities = gameData.weaponProbabilities.mediumProbabilities;
                break;
            case DifficultyLevel.Hard:
                probabilities = gameData.weaponProbabilities.hardProbabilities;
                break;
            default:
                probabilities = gameData.weaponProbabilities.easyProbabilities; 
                break;
        }

        int totalProbability = probabilities.pistolProbability + probabilities.akProbability + probabilities.sniperProbability;
        int randomValue = Random.Range(1, totalProbability + 1);

        if (randomValue <= probabilities.pistolProbability)
        {
            pistolObject.SetActive(true);
        }
        else if (randomValue <= probabilities.pistolProbability + probabilities.akProbability)
        {
            akObject.SetActive(true);
        }
        else
        {
            sniperObject.SetActive(true);
        }
    }
}
