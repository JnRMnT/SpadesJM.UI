using UnityEngine;
using System.Collections;

public class ConnectivityManager : MonoBehaviour
{
    public static bool InternetAvailable;
    private WWW www;
    private int tryCount;

    // Use this for initialization
    void Start()
    {
        www = new WWW("http://www.microsoft.com/");
        StartCoroutine(checkConnection());

        tryCount = 0;
    }

    IEnumerator checkConnection()
    {
        LogManager.Log("Trying");
        yield return www;
        
        if (tryCount < 5)
        {
            tryCount++;
        }
        if (www.error != null)
        {
            LogManager.Log("faild to connect to internet, trying after 15 seconds.");
            InternetAvailable = false;
            yield return new WaitForSeconds(tryCount < 5 ? 2 : 15);// trying again after 15 sec
            StartCoroutine(checkConnection());
        }
        else
        {
            LogManager.Log("connected to internet");
            InternetAvailable = true;
            yield return new WaitForSeconds(60);// recheck if the internet still exists after 60 sec
            StartCoroutine(checkConnection());

        }

    }
}
