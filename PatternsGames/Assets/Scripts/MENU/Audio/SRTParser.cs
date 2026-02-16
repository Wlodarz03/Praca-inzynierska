using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class SRTParser
{
    public static List<SubtitleLine> Parse(TextAsset srtFile)
    {
        var result = new List<SubtitleLine>();
        var lines = srtFile.text.Split('\n');

        int i = 0;
        while (i < lines.Length)
        {
            if (string.IsNullOrWhiteSpace(lines[i]) || int.TryParse(lines[i], out _))
            {
                i++;
                continue;
            }

            if (lines[i].Contains("-->"))
            {
                var time = lines[i].Split(new[] { "-->" }, StringSplitOptions.None);

                float start = ParseTime(time[0]);
                float end = ParseTime(time[1]);

                i++;
                string text = "";

                while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
                {
                    text += lines[i].Trim() + "\n";
                    i++;
                }

                result.Add(new SubtitleLine
                {
                    start = start,
                    end = end,
                    text = text.Trim()
                });
            }

            i++;
        }

        return result;
    }

    private static float ParseTime(string time)
    {
        time = time.Trim();
        var parts = time.Split(':', ',');

        float h = float.Parse(parts[0]);
        float m = float.Parse(parts[1]);
        float s = float.Parse(parts[2]);
        float ms = float.Parse(parts[3]);

        return h * 3600f + m * 60f + s + ms / 1000f;
    }
}
