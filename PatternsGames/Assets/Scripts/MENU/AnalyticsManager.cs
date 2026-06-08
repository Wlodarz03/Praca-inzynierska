using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UnityConsent; // <--- NOWA BIBLIOTEKA OD UNITY DO ZGÓD

public class AnalyticsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject privacyPanel;
    public Button acceptButton;
    public Button declineButton;

    // Klucz, pod którym zapiszemy decyzję gracza
    private const string ConsentKey = "AnalyticsConsent";

    async void Start()
    {
        try
        {
            // 1. Inicjalizacja bazowych usług (zawsze wymagana)
            await UnityServices.InitializeAsync();

            // Przypisanie funkcji do przycisków
            acceptButton.onClick.AddListener(OnAcceptClicked);
            declineButton.onClick.AddListener(OnDeclineClicked);

            // Sprawdzenie, czy gracz już wcześniej podjął decyzję
            if (PlayerPrefs.HasKey(ConsentKey))
            {
                privacyPanel.SetActive(false); // Ukrywamy panel
                
                int consent = PlayerPrefs.GetInt(ConsentKey);
                if (consent == 1)
                {
                    // Gracz wcześniej się zgodził - nadajemy uprawnienia w nowym systemie
                    GrantAnalyticsConsent();
                    Debug.Log("Analityka działa w tle (zgoda już była).");
                }
                else
                {
                    // Gracz wcześniej odmówił
                    DenyAnalyticsConsent();
                }
            }
            else
            {
                // Brak decyzji - pokazujemy panel i czekamy na kliknięcie
                privacyPanel.SetActive(true);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Błąd inicjalizacji: {e.Message}");
        }
    }

    private void OnAcceptClicked()
    {
        // Zapisujemy zgodę i ukrywamy panel
        PlayerPrefs.SetInt(ConsentKey, 1);
        PlayerPrefs.Save();
        privacyPanel.SetActive(false);
        
        // Przekazujemy zgodę do nowego systemu Unity
        GrantAnalyticsConsent();
        Debug.Log("Zgoda udzielona. Analityka wystartowała.");
    }

    private void OnDeclineClicked()
    {
        // Zapisujemy odmowę i ukrywamy panel
        PlayerPrefs.SetInt(ConsentKey, 0);
        PlayerPrefs.Save();
        privacyPanel.SetActive(false);
        
        // Blokujemy analitykę w nowym systemie Unity
        DenyAnalyticsConsent();
        Debug.Log("Zgoda odrzucona. Gramy bez analityki.");
    }

    // --- NOWE METODY ZARZĄDZANIA ZGODAMI ---

    private void GrantAnalyticsConsent()
    {
        // Nowy sposób na uruchomienie analityki (zastępuje StartDataCollection)
        EndUserConsent.SetConsentState(new ConsentState {
            AnalyticsIntent = ConsentStatus.Granted, // Zgoda udzielona
            AdsIntent = ConsentStatus.Denied         // Jeśli nie masz reklam, od razu odrzucasz
        });
    }

    private void DenyAnalyticsConsent()
    {
        EndUserConsent.SetConsentState(new ConsentState {
            AnalyticsIntent = ConsentStatus.Denied, // Zgoda odrzucona
            AdsIntent = ConsentStatus.Denied
        });
    }
}