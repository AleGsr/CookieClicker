using UnityEngine;
using TMPro;
using System.Collections;

public class CookieManager : MonoBehaviour
{

    public int currentCookies = 0;
    public int cookiesPC = 1;

    float globalMultiplier = 1f;
    int clickBonus = 0;
    int passiveCookies = 0;


    public TextMeshProUGUI totalCookiesTxt;
    public TextMeshProUGUI cashCookies;


    int passiveCookiesPerSecond = 0;

    int clickCounter = 0;

    bool crunchyActive = false;
    bool comboActive = false;




    void Start()
    {
        UpdatingTextCookies();
    }

    void Update()
    {
        
    }

    public void UpdatingTextCookies()
    {
        totalCookiesTxt.text = ("$" + currentCookies);
        cashCookies.text = ("$" + currentCookies);
    }

    public void InicialIncreaseCookies()
    {

    }

    public void ClickingIncreaseCookies()
    {
        clickCounter++;

        int gain = cookiesPC + clickBonus;

        if (comboActive)
            gain *= 5;

        gain = Mathf.RoundToInt(gain * globalMultiplier);

        currentCookies += gain;

        if (crunchyActive && clickCounter % 30 == 0)
            globalMultiplier += 0.05f;

        UpdatingTextCookies();
    }



    //----------GATITOS PANADEROS----------
    public void StartPassiveProduction(int amount)
    {
        passiveCookiesPerSecond += amount;

        StopCoroutine("PassiveLoop");
        StartCoroutine("PassiveLoop");
    }

    IEnumerator PassiveLoop()
    {
        while (true)
        {
            currentCookies += passiveCookiesPerSecond;
            UpdatingTextCookies();
            yield return new WaitForSeconds(1f);
        }
    }



    //----------GATITOS HORNOS----------
    public void AddGlobalMultiplier(float percent)
    {
        globalMultiplier += percent;
    }

    //Sweet
    public void AddClickBonus(int bonus)
    {
        clickBonus += bonus;
    }


    //GAlletitas Crujientes
    public void EnableCrunchyCookies()
    {
        crunchyActive = true;
    }

    //SweetPurr
    public void EnableComboBoost()
    {
        comboActive = true;
    }




    //----------galletas doradas----------
    public void EnableGoldenCookie()
    {
        StartCoroutine(GoldenBoost());
    }

    IEnumerator GoldenBoost()
    {
        globalMultiplier *= 2f;

        yield return new WaitForSeconds(10f);

        globalMultiplier /= 2f;
    }





}
