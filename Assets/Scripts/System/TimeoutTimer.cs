using UnityEngine;
using System.Collections;

public class TimeoutTimer : MonoBehaviour {
    private float seconds;
    private GameObject callbackObject;
    private bool IsActive;

	// Use this for initialization
	void Start () {
	
	}
	
    public void StartTimer(float totalSeconds,GameObject callbackObject){
        this.seconds = totalSeconds;
        this.callbackObject = callbackObject;
        this.IsActive = true;
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (IsActive)
        {
            seconds -= Time.fixedDeltaTime;
            if (seconds <= 0)
            {
                IsActive = false;
                callbackObject.SendMessage("Timeout");
                Destroy(this.gameObject);
            }
        }
	}
}
