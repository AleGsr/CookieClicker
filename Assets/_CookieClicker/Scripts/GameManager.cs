using UnityEngine;
using System.Collections;

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
            case "gatitos_panaderos":
                cookieManager.StartPassiveProduction(1);
                break;

            case "hornos_felinos":
                cookieManager.AddGlobalMultiplier(0.30f);
                break;

            case "gato_repartidor":
                cookieManager.AddGlobalMultiplier(0.10f);
                break;

            case "dulce_tecnica":
                cookieManager.AddClickBonus(5);
                break;

            case "galletas_crujientes":
                cookieManager.EnableCrunchyCookies();
                break;

            case "ronroneo_dulce":
                cookieManager.EnableComboBoost();
                break;

            case "galleta_dorada":
                cookieManager.EnableGoldenCookie();
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
    }



}
