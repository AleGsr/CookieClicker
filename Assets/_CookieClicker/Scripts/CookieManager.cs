using UnityEngine;
using TMPro;
using System.Collections;

public class CookieManager : MonoBehaviour
{

    public int currentCookies;
    public int cookiesPC = 1;

    public DataSaver dataSaver;

    float globalMultiplier = 1f;
    int clickBonus = 0;
    int passiveCookies = 0;


    public TextMeshProUGUI totalCookiesTxt;
    public TextMeshProUGUI cashCookies;


    int bakerCookiesPerSec;

    int clickCounter = 0;

    bool crunchyActive = false;
    bool comboActive = false;


    void Awake()
    {
        dataSaver.SetCookieManager(this);
    }


    void Start()
    {
        InvokeRepeating(nameof(AutoSave), 10f, 20f);

        void AutoSave()
        {
            dataSaver.SaveDataFn();
        }

        //dataSaver.SetCookieManager(this);
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

        AddCookies(gain);


        if (crunchyActive && clickCounter % 30 == 0)
            globalMultiplier += 0.05f;

        UpdatingTextCookies();
    }

    public void AddCookies(int amount)
    {
        currentCookies += amount;
        UpdatingTextCookies();

        if (dataSaver != null)
            dataSaver.SyncCoins(currentCookies);
    }



    //----------GATITOS PANADEROS----------
    public void BakerKittens(int amount)
    {
        bakerCookiesPerSec += amount;

        StopCoroutine("BakerKittensProduction");
        StartCoroutine("BakerKittensProduction");
    }

    IEnumerator BakerKittensProduction()
    {
        while (true)
        {
            AddCookies(bakerCookiesPerSec);
            UpdatingTextCookies();
            yield return new WaitForSeconds(1f);
        }
    }



    //----------GATITOS HORNOS----------
    public void OvenCat(float percent)
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


    public bool TrySpendCookies(int amount)
    {
        if (currentCookies < amount)
            return false;

        currentCookies -= amount;
        UpdatingTextCookies();

        if (dataSaver != null)
            dataSaver.SyncCoins(currentCookies);

        return true;
    }



}
