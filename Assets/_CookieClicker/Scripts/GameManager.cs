using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    public DataSaver dataSaver;

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

    public bool TryBuy(ShopItem item)
    {
        if (dataSaver == null || dataSaver.dts == null)
        {
            Debug.LogError("[GameManager] Falta DataSaver o dts.");
            return false;
        }

        if (item == null)
        {
            Debug.LogError("[GameManager] Item null.");
            return false;
        }

        if (!int.TryParse(item.price, out int price))
        {
            Debug.LogError("[GameManager] Precio inválido: " + item.price);
            return false;
        }

        if (dataSaver.dts.totalCoins < price)
        {
            Debug.Log("No alcanza monedas.");
            return false;
        }

        // 1) Cobrar
        dataSaver.dts.totalCoins -= price;

        // 2) Registrar compra en inventario
        dataSaver.RegisterPurchase(item.id, 1);

        // 3) Guardar en Firebase
        dataSaver.SaveDataFn();

        Debug.Log($"Compra OK: {item.id} (price={price}). Coins restantes={dataSaver.dts.totalCoins}");
        return true;
    }
}
