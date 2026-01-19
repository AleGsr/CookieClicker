using UnityEngine.Events;


public enum GameState
{
    Loading,
    Auth,
    Store
}


public static class AppEventsHUB 
{
    public static UnityEvent OnFireBaseInitialized = new();
    public static UnityEvent<GameState> OnGameStateChange = new();

}
