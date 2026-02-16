using UnityEngine;

[System.Serializable]
public class CodeFile
{
    public string fileName;

    [TextArea(20, 400)]
    public string code;
}
