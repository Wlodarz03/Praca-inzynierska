using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Design Patterns/Pattern Code")]
public class DesignPatternCode : ScriptableObject
{
    public string patternName;

    public List<CodeFile> files;
}