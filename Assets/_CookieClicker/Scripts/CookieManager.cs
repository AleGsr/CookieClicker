using UnityEngine;
using TMPro;

public class CookieManager : MonoBehaviour
{

    public int currentCookies = 0;
    public int cookiesPC = 1;


    public TextMeshProUGUI totalCookiesTxt;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatingTextCookies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatingTextCookies()
    {
        totalCookiesTxt.text = currentCookies.ToString();
    }

    public void InicialIncreaseCookies()
    {

    }

    public void ClickingIncreaseCookies()
    {
        currentCookies += cookiesPC;
        UpdatingTextCookies();
    }



}
