using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using TMPro;
using Firebase.Extensions;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameInput;

    private FirebaseAuth auth;
    private DatabaseReference dbRef;

    public DataSaver dataSaver;

    void Start()
    {
        if(FirebaseInit.IsFirebaseReady)
        {
            InitializeAppFireBase();
        }
        else
        {
            //NEW
            AppEventsHUB.OnFireBaseInitialized.AddListener(InitializeAppFireBase);

            //OLD
            //FirebaseInit.OnFirebaseReady += InitializeAppFireBase;
        }
    }

    private void InitializeAppFireBase()
    {
        FirebaseApp app = FirebaseInit.AppInstance;

        Debug.Log("Database URL in AuthManager: " + app.Options.DatabaseUrl);

        auth = FirebaseAuth.GetAuth(app);
        dbRef = FirebaseDatabase.GetInstance(app).RootReference;

        Debug.Log("Firebase is ready in AuthManager. ");
    }

    public void RegisterUser()
    {
        if (auth == null)
        {
            Debug.LogWarning("Firebase not ready yet. ");
            return;
        }

        string email = emailInput.text;
        string password = passwordInput.text;
        string username = usernameInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Signup Failed: " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result.User;
            Debug.Log("User created: " + user.Email);
            SaveUserData(user.UserId, username);
        });
    }

    public void LoginUser()
    {
        if (auth == null)
        {
            Debug.LogWarning("Firebase not ready yet. ");
            return;
        }
        if (auth.CurrentUser != null)
        {
            Debug.Log("User already logged in: " + auth.CurrentUser.Email);
            return;
        }
        if (auth.CurrentUser == null)
        {
            Debug.LogWarning("No authenticated user. Waiting for login...");
            return;
        }

        string email = emailInput.text;
        string password = passwordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Login Failed: " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result.User;
            Debug.Log("User logged in: " + user.Email);

            dataSaver.LoadDataFn();
        });
    }

    void SaveUserData(string uid, string username)
    {
        dbRef.Child("users").Child(uid).Child("username").SetValueAsync(username).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Username saved to database. ");
            }
            else
            {
                Debug.LogError("Failed to save username: " + task.Exception);
            }
        });
    }

    public void Logout()
    {
        if (auth != null)
        {
            auth.SignOut();
            dataSaver.SaveDataFn();

            Debug.Log("User signed out.");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
