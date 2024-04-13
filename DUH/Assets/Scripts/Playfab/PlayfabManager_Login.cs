using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System.Collections;

public class PlayfabManager_Login : MonoBehaviour
{
    public TextMeshProUGUI messageText; 
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField; 
    public Button loginButton;
    public Button resetPasswordButton;

    private bool isProcessingRequest = false; // Flag to prevent multiple requests

    void Start()
    {
        loginButton.onClick.AddListener(OnClickLoginButton);
        resetPasswordButton.onClick.AddListener(OnClickResetPasswordButton);
    }

    private void OnClickLoginButton()
    {
        if (isProcessingRequest) return; // Prevent multiple requests
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        isProcessingRequest = true;

        if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            messageText.text = "Email and password cannot be empty!";
            isProcessingRequest = false;
            yield break;
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInputField.text,
            Password = passwordInputField.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);

        // Wait for the response before processing further requests
        while (isProcessingRequest)
        {
            yield return null;
        }
    }

    private void OnClickResetPasswordButton()
    {
        if (isProcessingRequest) return; // Prevent multiple requests
        StartCoroutine(ResetPasswordCoroutine());
    }

    private IEnumerator ResetPasswordCoroutine()
    {
        isProcessingRequest = true;

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInputField.text,
            TitleId = PlayFabSettings.TitleId
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnResetPasswordFailure);

        // Wait for the response before processing further requests
        while (isProcessingRequest)
        {
            yield return null;
        }
    }

    private void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged in!";
        isProcessingRequest = false;
    }

    private void OnLoginFailure(PlayFabError error)
    {
        // Handle login failure
        messageText.text = GetFriendlyErrorMessage(error);
        isProcessingRequest = false;
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset email sent!";
        isProcessingRequest = false;
    }

    private void OnResetPasswordFailure(PlayFabError error)
    {
        // Handle password reset failure
        messageText.text = GetFriendlyErrorMessage(error);
        isProcessingRequest = false;
    }

    private string GetFriendlyErrorMessage(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidParams:
                return "Invalid email or password.";
            case PlayFabErrorCode.AccountNotFound:
                return "Account not found.";
            case PlayFabErrorCode.AccountBanned:
                return "Your account has been banned.";
            default:
                return "Failed to perform the operation. Please try again later.";
        }
    }
}
