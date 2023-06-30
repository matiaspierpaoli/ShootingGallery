using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public void PlaySoundEvent(AK.Wwise.Event soundEvent, GameObject gameObject)
    {
        soundEvent.Post(gameObject);
    }
}