using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using TMPro;

public class PalyFabManager : MonoBehaviour
{
    public static PalyFabManager Instance;

    public TextMeshProUGUI txtToast;

    public TextMeshProUGUI h1, h2, txtMainButton, t1, t2;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public GameObject btnForgotPassword;

    public bool isRegistering;
    public Color32 cRed, cBrightLime;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        GlobalData.userID = "";
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

    public void MainButton()
    {
        if (!isRegistering) {
            var request = new LoginWithEmailAddressRequest
            {
                Email = emailInput.text,
                Password = passwordInput.text
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFail);
            txtToast.text = "Logging in...";
            txtToast.color = cBrightLime;
        }
        else {
            var request = new RegisterPlayFabUserRequest
            {
                Email = emailInput.text,
                Password = passwordInput.text,
                RequireBothUsernameAndEmail = false
            };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFail);
            txtToast.text = "Registering...";
            txtToast.color = cBrightLime;
        }
    }

    public void ResetPassword() {
        var request = new SendAccountRecoveryEmailRequest {
            Email = emailInput.text,
            TitleId = "75F36"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordResetSuccess, OnPasswordResetFail);
    }

    void OnPasswordResetSuccess(SendAccountRecoveryEmailResult result) {
        txtToast.text = "Password reset mail sent!";
        txtToast.color = cBrightLime;
    }
    void OnPasswordResetFail(PlayFabError error) {
        txtToast.text = "Password reset error.";
        txtToast.color = cRed;
        if (error.ErrorDetails != null) {
            foreach(KeyValuePair<string,List<string>> errorDetail in error.ErrorDetails)
            {
                txtToast.text = errorDetail.Value[0];
                break;
            }
        }
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        txtToast.text = "Registration successful!";
        txtToast.color = cBrightLime;
    }
    void OnRegisterFail(PlayFabError error)
    {
        txtToast.text = "Registration error.";
        txtToast.color = cRed;
        if (error.ErrorDetails != null) {
            foreach(KeyValuePair<string,List<string>> errorDetail in error.ErrorDetails)
            {
                txtToast.text = errorDetail.Value[0];
                break;
            }
        }
        // Debug.Log(error.GenerateErrorReport());
    }

    void OnLoginSuccess(LoginResult result)
    {
        txtToast.text = "Log in successful!";
        txtToast.color = cBrightLime;
        SceneManager.LoadScene("MainMenu");
        GlobalData.userID = emailInput.text;
        GetUserAdminStatus();
    }
    void OnLoginFail(PlayFabError error)
    {
        txtToast.text = "Error logging in.";
        txtToast.color = cRed;
        if (error.ErrorDetails != null) {
            foreach(KeyValuePair<string,List<string>> errorDetail in error.ErrorDetails)
            {
                txtToast.text = errorDetail.Value[0];
                break;
            }
        }
        // Debug.Log(error.GenerateErrorReport());
    }

    private void GetUserAdminStatus() {
        PlayFab.ClientModels.GetUserDataRequest request = new PlayFab.ClientModels.GetUserDataRequest();
        request.Keys = new List<string>();
        request.Keys.Add("isAdmin");
        PlayFabClientAPI.GetUserReadOnlyData(request, OnGUASSuccess, OnGUASFail);
    }

    private void OnGUASSuccess(PlayFab.ClientModels.GetUserDataResult result) {
        PlayFab.ClientModels.UserDataRecord dataRecord;
        result.Data.TryGetValue("isAdmin", out dataRecord);
        if (dataRecord != null) {
            GlobalData.isAdmin = dataRecord.Value == "true";
        }
    }
    private void OnGUASFail(PlayFabError error) {
        // Debug.Log(error.GenerateErrorReport());
    }
}
