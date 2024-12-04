using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag;
    public event System.Action VictoryTriggerEvent;

    private bool triggered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == playerTag && !triggered)
        {
            triggered = true;
            VictoryTriggerEvent?.Invoke();
        }
    }
}
