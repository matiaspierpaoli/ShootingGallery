using UnityEngine;

[CreateAssetMenu(fileName = "New FloorSettings", menuName = "FloorSettings")]
public class FloorSettings : ScriptableObject
{
    public string floorTag;
    public bool pistolAvailable;
    public bool akAvailable;
    public bool sniperAvailable;
    public bool challengeStarted;
    public bool currentTimeTextEnabled;
    public bool currentAmmoTextEnabled;
}
