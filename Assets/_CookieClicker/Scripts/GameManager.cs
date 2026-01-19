using UnityEngine;

public class GameManager : MonoBehaviour
{



    void Start()
    {
        AppEventsHUB.OnFireBaseInitialized.AddListener(InitApp);
    }

    void InitApp()
    {
        AppEventsHUB.OnGameStateChange.Invoke(GameState.Auth);
    }

    private void OnDisable()
    {
        AppEventsHUB.OnFireBaseInitialized.RemoveAllListeners();
    }


    public bool TryBuy(string itemID)
    {


        return false;
    }

}
