using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour
{
    [SerializeField] private APIManager apiManager;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button loginButton;
    [SerializeField] private GameObject authObject;
    
    private void Start()
    {
        loginButton.onClick.AddListener(SubmitLogin);
        authObject.SetActive(true);
    }

    void SubmitLogin()
    {
        string email = inputField.text.Trim();

        if (!IsValidEmail(email)) return;

        apiManager.Login(email);
        authObject.SetActive(false);
    }

    bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        return Regex.IsMatch(
            email.Trim(),
            @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$"
        );
    }
}
