using Unity.Services.Analytics;
using UnityEngine;

public class TelemetrySender : MonoBehaviour
{
    private float currentSessionStartTime;
    private string currentTrackedGame;

    public void RecordPatternClick(string patternName)
    {
        CustomEvent patternEvent = new CustomEvent("pattern_clicked")
        {
            { "pattern_name", patternName }
        };
        
        AnalyticsService.Instance.RecordEvent(patternEvent);
        Debug.Log($"[Telemetria] Zarejestrowano kliknięcie w: {patternName}");
    }

   public void RecordNarratorClick()
    {
        CustomEvent narratorEvent = new CustomEvent("narrator_clicked");
        
        AnalyticsService.Instance.RecordEvent(narratorEvent);
        Debug.Log("[Telemetria] Kliknięto w opcję Narratora.");
    }

    public void RecordShowCodeClick(string currentPattern)
    {
        CustomEvent codeEvent = new CustomEvent("show_code_clicked")
        {
            { "pattern_name", currentPattern }
        };
        
        AnalyticsService.Instance.RecordEvent(codeEvent);
        Debug.Log($"[Telemetria] Podejrzano kod dla: {currentPattern}");
    }

    public void StartTrackingGameTime(string gameName)
    {
        currentTrackedGame = gameName;
        currentSessionStartTime = Time.time;
        Debug.Log($"[Telemetria] Rozpoczęto mierzenie czasu dla: {gameName}");
    }

    public void StopTrackingGameTime()
    {
        if (string.IsNullOrEmpty(currentTrackedGame)) 
        {
            return;
        }

        float timeSpentSeconds = Time.time - currentSessionStartTime;

        timeSpentSeconds = Mathf.Round(timeSpentSeconds * 100f) / 100f;

        CustomEvent timeEvent = new CustomEvent("time_spent_in_game")
        {
            { "pattern_name", currentTrackedGame },
            { "time_spent_seconds", timeSpentSeconds }
        };
        
        AnalyticsService.Instance.RecordEvent(timeEvent);
        Debug.Log($"[Telemetria] Zakończono. Czas w {currentTrackedGame}: {timeSpentSeconds} sekund.");

        currentTrackedGame = null; 
    }
}
