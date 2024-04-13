using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayfabLoginManager : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Text field to display messages to the user
    const string LAST_EMAIL_KEY = "last_email", LAST_PASSWORD_KEY = "last_password";

    #region Register
    [Header("Register")]
    [SerializeField] TMP_InputField registerEmail;
    [SerializeField] TMP_InputField registerUsername;
    [SerializeField] TMP_InputField registerPassword;

    public void OnRegisterPressed()
    {
        Register(registerEmail.text, registerPassword.text, registerUsername.text);
    }

    private void Register(string email, string password, string username)
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            DisplayName = username,
            RequireBothUsernameAndEmail = false
        }, successResult =>
        {
            Login(email, password); // If registration succeeds, automatically log in
        }, PlayfabFailure);
    }

    #endregion

    #region Login
    [Header("Login")]
    [SerializeField] TMP_InputField loginEmail;
    [SerializeField] TMP_InputField loginPassword;

    public void OnLoginPressed()
    {
        Login(loginEmail.text, loginPassword.text);
    }

    private void Login(string email, string password)
    {
        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerProfile = true
            }
        }, successResult =>
        {
            PlayerPrefs.SetString(LAST_EMAIL_KEY, email);
            PlayerPrefs.SetString(LAST_PASSWORD_KEY, password);
            PlayerPrefs.SetString("Username", successResult.InfoResultPayload.PlayerProfile.DisplayName);
            Debug.Log("Successfully Logged in User: " + PlayerPrefs.GetString("Username"));
        }, PlayfabFailure);
    }

    #endregion

    #region ResetPassword
    [Header("Reset Password")]
    [SerializeField] TMP_InputField resetEmailInput;

    public void OnSendPressed()
    {
        SendResetPasswordEmail(resetEmailInput.text);
    }

    private void SendResetPasswordEmail(string email)
    {
        PlayFabClientAPI.SendAccountRecoveryEmail(new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = PlayFabSettings.TitleId
        }, result => Debug.Log("Password reset email sent"), PlayfabFailure);
    }
    #endregion

private void PlayfabFailure(PlayFabError error)
{
    messageText.text = error.ErrorMessage; 
    Debug.LogError(error.GenerateErrorReport());
}

}
