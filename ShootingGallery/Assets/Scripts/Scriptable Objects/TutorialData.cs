using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "TutorialData")]
public class TutorialData : ScriptableObject
{
    public bool isMovingPlayerAvailable;
    public bool isShootingAvailable;
    public bool isReloadingAvailable;
    public bool isPausingAvailable;
    public bool isChanginWeaponsAvailable;

    public bool finishedMovingPlayer;
    public bool finishedShooting;
    public bool finishedReloading;
    public bool finishedPausing;
}
