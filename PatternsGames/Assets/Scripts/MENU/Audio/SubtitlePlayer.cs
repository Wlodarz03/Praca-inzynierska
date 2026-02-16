using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SubtitlePlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text subtitleText;

    private List<SubtitleLine> subtitles;
    private AudioSource narratorSource;

    public void Load(TextAsset srt)
    {
        subtitles = SRTParser.Parse(srt);
        narratorSource = AudioManager.Instance.GetNarratorSource();
    }

    private void Update()
    {
        if (narratorSource == null || !narratorSource.isPlaying)
            return;

        float time = narratorSource.time;

        foreach (var sub in subtitles)
        {
            if (time >= sub.start && time <= sub.end)
            {
                subtitleText.text = sub.text;
                return;
            }
        }

        subtitleText.text = "";
    }

    public void Seek(float time)
    {
        subtitleText.text = "";
    }
}
