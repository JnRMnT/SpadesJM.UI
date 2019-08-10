using UnityEngine;
using System.Collections;

public class LogManager : MonoBehaviour {
    public static bool IsActive = false;

    public static void Log(string message){
        if (IsActive)
        {
            Debug.Log(message);
        }
    }
}
