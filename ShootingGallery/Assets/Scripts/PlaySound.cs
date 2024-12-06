using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private AK.Wwise.Event eventSFX;

    public void PlaySFX()
    {
        eventSFX.Post(gameObject);
    } 
}
