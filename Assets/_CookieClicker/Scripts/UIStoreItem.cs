using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStoreItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameTag;
    public TMP_Text priceTag;
    public TMP_Text effectTag;
    public Image effecTypeIcon;

    [Header("Buy")]
    public Button buyButton;

    private ShopItem boundItem;
    private GameManager gameManager;

    [Header("State")]
    public GameObject purchasedMark; // check, glow, overlay, etc


    public void Init(GameManager gm)
    {
        gameManager = gm;

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClicked);
        }
    }

    public void Bind(ShopItem item, Sprite itemIcon, Sprite effectIcon)
    {
        boundItem = item;

        nameTag.text = item.name;
        priceTag.text = item.price;
        icon.sprite = itemIcon;
        effectTag.text = item.effect.ToString() + "x";
        effecTypeIcon.sprite = effectIcon;
    }

    private void OnBuyClicked()
    {
        if (gameManager == null)
        {
            Debug.LogError("[UIStoreItem] No GameManager. ¿Llamaste Init() al instanciar?");
            return;
        }
        if (boundItem == null)
        {
            Debug.LogError("[UIStoreItem] No boundItem. ¿Llamaste Bind()?");
            return;
        }

        //gameManager.TryBuy(boundItem);
        bool bought = gameManager.TryBuy(boundItem);

        if (bought)
        {
            SetPurchasedVisual(true);
        }

        Debug.Log("OnBuyClicked: intentó comprar " + boundItem.name);
    }

    void SetPurchasedVisual(bool state)
    {
        if (purchasedMark != null)
            purchasedMark.SetActive(state);

        if (buyButton != null)
            buyButton.interactable = !state;
    }


    public void ForcePurchased()
    {
        SetPurchasedVisual(true);
    }

}
