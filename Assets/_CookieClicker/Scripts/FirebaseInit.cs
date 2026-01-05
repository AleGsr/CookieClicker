using UnityEngine;
using Firebase;
using Firebase.Extensions;
using System;

public class FirebaseInit : MonoBehaviour
{
    public static bool IsFirebaseReady { get; private set; } = false;
    public static FirebaseApp AppInstance { get; private set; }
    public static event Action OnFirebaseReady;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                AppInstance = FirebaseApp.DefaultInstance;
                if (Application.isEditor && AppInstance.Options.DatabaseUrl == null)
                {
                    AppInstance.Options.DatabaseUrl = new System.Uri("https://protogames-2cfa8-default-rtdb.firebaseio.com/");
                    Debug.Log("Database URL set in FirebaseInit: " + AppInstance.Options.DatabaseUrl);
                }
                IsFirebaseReady = true;
                Debug.Log("Firebase initialized!");
                OnFirebaseReady?.Invoke();
            }
            else
            {
                Debug.LogError("Firebase init failed: " + task.Result);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
