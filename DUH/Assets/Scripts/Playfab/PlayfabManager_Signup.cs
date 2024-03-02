using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using PlayFab;
using PlayFab.ClientModels;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class PlayfabManager_Signup : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI messageText; 
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField; 
    public TMP_InputField usernameInputField; 
    public Button registerButton;

    void Start()
    {
        // Add click listeners to the buttons
        registerButton.onClick.AddListener(RegisterButton);
    }

    public void RegisterButton()
    {
        string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        string passwordPattern = @"^.{8,}$";


        // Validate email and password fields
        if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            messageText.text = "Email and password cannot be empty!";
            Debug.LogError("Email and password cannot be empty!");
        }

        if (!Regex.IsMatch(emailInputField.text, emailPattern))
        {
            messageText.text = "Invalid email format!";
            Debug.LogError("Invalid email format!");
        }

        if (!Regex.IsMatch(passwordInputField.text, passwordPattern))
        {
            messageText.text = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, and one digit!";
        }

        // Register the user
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInputField.text,
            Password = passwordInputField.text,
            Username = usernameInputField.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }



    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and logged in!";
        Debug.Log("Congratulations, you are registered and logged in!");
    }

    void OnRegisterFailure(PlayFabError error)
    {
        messageText.text = error.GenerateErrorReport();
        Debug.LogError(error.GenerateErrorReport());
    }

}
