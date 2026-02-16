using UnityEngine;

[CreateAssetMenu(menuName = "Narration Data")]
public class NarrationData : ScriptableObject
{
    public string narrationName;
    public AudioClip audioClip;
    public TextAsset subtitlesSRT;
}
