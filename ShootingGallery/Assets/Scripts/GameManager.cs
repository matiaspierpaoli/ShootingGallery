using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;

    public void SetTutorialData()
    {
        _gameData.tutorial = true;
    }

    public void SetLevelData()
    {
        _gameData.tutorial = false;
    }
}
