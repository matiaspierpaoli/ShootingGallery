using UnityEngine;
using UnityEngine.UI;

public enum ShopState
{
    Active,
    Inactive
}

public class ShopManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GameData _gameData;
    [SerializeField] private Button[] _shopButtons;

    private ShopState currentState = ShopState.Inactive;

    private void Start()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case ShopState.Active:
                ActivateShopButtons();
                break;
            case ShopState.Inactive:
                DeactivateShopButtons();
                break;
        }
    }

    public void SetState(ShopState newState)
    {
        currentState = newState;
        UpdateState();
    }

    private void ActivateShopButtons()
    {
        foreach (var button in _shopButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    private void DeactivateShopButtons()
    {
        foreach (var button in _shopButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}
