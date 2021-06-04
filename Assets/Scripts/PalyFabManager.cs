using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PalyFabManager : MonoBehaviour
{
    public static PalyFabManager Instance;

    public Text messageText;

    public TextMeshProUGUI h1, h2, txtMainButton, t1, t2;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public GameObject btnForgotPassword;

    public bool isRegistering;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void Register() {
        h1.text = "Hello there,";
        h2.text = "Welcome to TaraCar!";
        txtMainButton.text = "REGISTER";
        t1.text = "Already have an account?";
        t2.text = "Login";
        btnForgotPassword.SetActive(false);
    }

    public void BackToLogin() {
        h1.text = "Welcome back!";
        h2.text = "Log in and start exploring.";
        txtMainButton.text = "LOG IN";
        t1.text = "Don't have an account yet?";
        t2.text = "Register";
        btnForgotPassword.SetActive(true);
    }

    public void SceneChanger() {
        if (isRegistering) {
            BackToLogin();
        } else {
            Register();
        }
        isRegistering = !isRegistering;
    }

    public void ToggleShowPassword() {
        if (passwordInput.inputType == TMP_InputField.InputType.Password) {
            passwordInput.inputType = TMP_InputField.InputType.Standard;
        } else {
            passwordInput.inputType = TMP_InputField.InputType.Password;
        }
        passwordInput.ActivateInputField();
    }

    public void RegisterButton()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Account Registered!";
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged in";
        SceneManager.LoadScene("MainMenu");
        //Debug.Log("Successful login/account create!");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/ creating account");
        Debug.Log(error.GenerateErrorReport());
    }
   
}
