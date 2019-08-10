using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    //private static AndroidJavaClass admob;
    private static float renew;
    private static bool renewActive = false;
    private static float renewCount = 10f;
    private static InterstitialAd iosInters;
    private static string iosIntersAdUnitId = "ca-app-pub-4677568272713037/3263089113";
    private static bool tryAgain, tryInstantiateAgain;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            //admob = new AndroidJavaClass("com.jnrmnt.spadesjm.AdMobUnityActivity");
        }

        renew = renewCount;

        StartCoroutine(InstantiateInters());
    }

    void Update()
    {
        if (renewActive)
        {
            renew -= Time.deltaTime;

            if (renew <= 0)
            {
                renewActive = false;
                RenewIntersAd();
            }
        }
        if (tryAgain)
        {
            tryAgain = false;
            StartCoroutine(TryShowAgain());
        }
        if (tryInstantiateAgain)
        {
            tryInstantiateAgain = false;
            StartCoroutine(TryInstantiateAgain());
        }
    }

    private IEnumerator TryInstantiateAgain()
    {
        if (!ConnectivityManager.InternetAvailable)
        {
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(TryInstantiateAgain());
        }
        else
        {
            yield return null;
            InstantiateIntersAd();
        }
    }

    private IEnumerator TryShowAgain()
    {
        if (!ConnectivityManager.InternetAvailable)
        {
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(TryShowAgain());
        }
        else
        {
            yield return null;
            DisplayIntersAd();
        }
    }

    private IEnumerator InstantiateInters()
    {
        if (!ConnectivityManager.InternetAvailable)
        {
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(InstantiateInters());
        }
        else
        {
            yield return null;
            InstantiateIntersAd();
        }
    }

    public static void DisplayIntersAd()
    {
        if (ConnectivityManager.InternetAvailable)
        {
            if (Application.platform == RuntimePlatform.WP8Player)
            {
                AdMobUnityPlugin.AdMobAds.displayInterstitial();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                //admob.CallStatic("displayInterstitial");
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (iosInters!=null && iosInters.IsLoaded())
                {
                    iosInters.Show();
                }
                else if (iosInters != null)
                {
                    RenewIntersAd();
                    tryAgain = true;
                }
                else
                {
                    InstantiateIntersAd();
                }
            }

            renewActive = true;
            renew = renewCount;
        }
        else
        {
            RenewIntersAd();
            tryAgain = true;
        }
    }

    private static void RenewIntersAd()
    {
        if (ConnectivityManager.InternetAvailable)
        {
            if (Application.platform == RuntimePlatform.WP8Player)
            {
                AdMobUnityPlugin.AdMobAds.gecisYenile();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                //admob.CallStatic("renewInters");
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                iosInters.Destroy();
                InstantiateIntersAd();
            }
        }
    }

    private static void InstantiateIntersAd()
    {
        if (ConnectivityManager.InternetAvailable)
        {
            if (Application.platform == RuntimePlatform.WP8Player)
            {
                AdMobUnityPlugin.AdMobAds.gecisIlkle();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                //admob.CallStatic("setupInters");
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                iosInters = new InterstitialAd(iosIntersAdUnitId);
                iosInters.LoadAd(new GoogleMobileAds.Api.AdRequest.Builder().Build());
            }
        }
        else
        {
            tryInstantiateAgain = true;
        }
    }
}
