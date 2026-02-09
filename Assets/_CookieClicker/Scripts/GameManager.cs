using UnityEngine;using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    public DataSaver dataSaver;
    public CookieManager cookieManager;

    void Start()
    {

        AppEventsHUB.OnFireBaseInitialized.AddListener(InitApp);
        dataSaver.LoadDataFn();
        StartCoroutine(WaitAndApply());
    }

    IEnumerator WaitAndApply()
    {
        yield return new WaitForSeconds(1f);
        ApplySavedPurchases();
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
        //if (dataSaver == null || !dataSaver.IsFirebaseReady)
        //{
        //    Debug.LogWarning("Firebase not ready. Purchase blocked.");
        //    return false;
        //}
        //if (dataSaver == null || dataSaver.dts == null)
        //{
        //    Debug.LogError("[GameManager] Falta DataSaver o dts.");
        //    return false;
        //}

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
        //dataSaver.dts.totalCoins -= price;
        if (!cookieManager.TrySpendCookies(price))
        {
            Debug.Log("No alcanza monedas.");
            return false;
        }

        // 2) Registrar compra en inventario
        dataSaver.RegisterPurchase(item.id, 1);
        cookieManager.UpdatingTextCookies();

        ApplyItemEffectById(item.id);


        // 3) Guardar en Firebase
        dataSaver.SaveDataFn();

        Debug.Log($"Compra OK: {item.id} (price={price}). Coins restantes={dataSaver.dts.totalCoins}");
        return true;
    }

    void ApplyItemEffectById(string id)
    {
        switch (id)
        {
            case "Baker Kittens":
                cookieManager.BakerKittens(1);
                cookieManager.TurnOnPowerUp(0);
                break;

            case "Oven Cat":
                cookieManager.OvenCat(0.30f);
                cookieManager.TurnOnPowerUp(1);
                break;

            case "Delivery Cat":
                cookieManager.OvenCat(0.10f);
                cookieManager.TurnOnPowerUp(2);
                break;

            case "Sweet Secret Technique":
                cookieManager.AddClickBonus(5);
                cookieManager.TurnOnPowerUp(3);
                break;

            case "Crispy Cookies":
                cookieManager.EnableCrunchyCookies();
                cookieManager.TurnOnPowerUp(4);
                break;

            case "Sweet Purr":
                cookieManager.EnableComboBoost();
                cookieManager.TurnOnPowerUp(5);
                break;

            case "Golden Cookie":
                cookieManager.EnableGoldenCookie();
                cookieManager.TurnOnPowerUp(6);
                break;
        }
    }

    public void ApplySavedPurchases()
    {
        if (dataSaver.dts.purchasedItems == null) return;

        foreach (var entry in dataSaver.dts.purchasedItems)
        {
            for (int i = 0; i < entry.count; i++)
            {
                ApplyItemEffectById(entry.id);
            }
        }


        UIStoreItem[] all = FindObjectsOfType<UIStoreItem>();

        foreach (var ui in all)
        {
            foreach (var entry in dataSaver.dts.purchasedItems)
            {
                if (ui.nameTag.text == entry.id)
                    ui.ForcePurchased();
            }
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
