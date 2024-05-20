using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using System.Threading.Tasks;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class Authentication : MonoBehaviour
{
    [SerializeField]  string email;
    [SerializeField]  string password;
    [SerializeField]  AutenticatorSO currentUser;
    [SerializeField]  TMP_Text logText;

    private FirebaseAuth _authReference;

    public UnityEvent OnLogInSuccesful = new();
    public UnityEvent OnLogOutSuccesful = new();
    public UnityEvent OnRegisterSuccesful = new();

    void Awake()
    {
        _authReference = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
    }
    public void SetEmail(string email) 
    {
        this.email = email;
    }
    public void SetPassword(string password)
    {
        this.password = password;
    }
    public void SignUp()
    {
        StartCoroutine(RegisterUser(email, password));
    }

    public void SignIn()
    {
        StartCoroutine(SignInWithEmail(email, password));
    }

    public void SignOut()
    {
        LogOut();
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene(1);
    }

    private IEnumerator RegisterUser(string email, string password)
    {
        Debug.Log("Registering");
        var registerTask = _authReference.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if(registerTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {registerTask.Exception}");
        }
        else
        {
            OnRegisterSuccesful?.Invoke();
            email = "";
            password = "";
            Debug.Log($"Succesfully registered user {registerTask.Result.User.Email}");
        }
    }

    IEnumerator SignInWithEmail(string email, string password)
    {
        Debug.Log("Loggin In");

        var loginTask = _authReference.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning($"Login failed with {loginTask.Exception}");
        }
        else
        {
            currentUser._correoID = loginTask.Result.User.Email;
            currentUser._Name = loginTask.Result.User.UserId;
            Debug.Log($"Login succeeded with {loginTask.Result.User.Email}");
            logText.text = "User: " + currentUser._Name.ToString();
            OnLogInSuccesful?.Invoke();
        }
    }

    void LogOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        OnLogOutSuccesful?.Invoke();
    }
}
