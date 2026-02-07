using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UI_LoadScreen;
    public GameObject UI_AuthScreen;
    public GameObject UI_Game;
    public GameObject UI_StoreScreen;

    public GameObject currentScreen;

    private void Start()
    {
        AppEventsHUB.OnGameStateChange.AddListener(OnGameStateChanged);

    }

    private void OnDisable()
    {
        AppEventsHUB.OnGameStateChange.RemoveAllListeners();
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        currentScreen = newGameState switch
        {
            GameState.Loading => UI_LoadScreen,
            GameState.Auth => UI_AuthScreen,
            GameState.Game => UI_Game,
            GameState.Store => UI_StoreScreen,
            _ => null
        };

        foreach(GameObject screen in new GameObject[] { UI_LoadScreen, UI_AuthScreen, UI_StoreScreen})
        {
            screen.SetActive(screen == currentScreen);
        }

    }


}
