using UnityEngine;

public class WwisePlayEvent : MonoBehaviour
{
    public void PlaySoundEvent(AK.Wwise.Event soundEvent, GameObject gameObject)
    {
        soundEvent.Post(gameObject);
    }
}
