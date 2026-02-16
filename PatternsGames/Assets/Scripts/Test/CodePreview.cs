using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class CodePreview : MonoBehaviour
{
    public TextMeshProUGUI codeText;  // Assign your TMP_Text component

    void Start()
    {
        //DisplayCode(codeText.text);
    }
    public void DisplayCode(string sourceCode)
    {
        string highlighted = HighlightSyntax(sourceCode);  // Your parsing function
        codeText.text = highlighted;
    }

    private string HighlightSyntax(string code)
    {
        // Order matters! Apply more specific patterns first.

        // Multi-line comments /* ... */
        code = Regex.Replace(code, @"/\*[\s\S]*?\*/", match => $"<color=#6A9955>{match.Value}</color>");

        // Single-line comments //
        code = Regex.Replace(code, @"//.*$", match => $"<color=#6A9955>{match.Value}</color>", RegexOptions.Multiline);

        // Preprocessor directives (#if, #region, etc.)
        code = Regex.Replace(code, @"^\s*(#.*)$", "<color=#C586C0>$1</color>", RegexOptions.Multiline);

        // Strings (both " and @")
        code = Regex.Replace(code, @"@(""[^""]*("")+[^""]*""|""[^""]*"")", "<color=#CE9178>$1</color>");
        code = Regex.Replace(code, @"""([^""\\]|\\.)*""", "<color=#CE9178>$1</color>");

        // Keywords (control flow: if, else, return, etc.)
        code = Regex.Replace(code, @"\b(abstract|as|base|break|case|catch|checked|class|const|continue|default|do|else|enum|event|explicit|extern|finally|fixed|for|foreach|goto|if|implicit|in|internal|is|lock|namespace|new|operator|out|override|params|private|protected|public|readonly|ref|return|sealed|sizeof|stackalloc|static|struct|switch|this|throw|try|typeof|unchecked|unsafe|using|virtual|volatile|while)\b",
            "<color=#569CD6>$1</color>");

        // Type keywords (int, string, bool, void, etc.)
        code = Regex.Replace(code, @"\b(bool|byte|char|decimal|double|float|int|long|object|sbyte|short|string|uint|ulong|ushort|void)\b",
            "<color=#4EC9B0>$1</color>");

        // Constants: true, false, null
        code = Regex.Replace(code, @"\b(true|false|null)\b", "<color=#569CD6>$1</color>");

        // Numbers (integers, floats, hex)
        code = Regex.Replace(code, @"\b(0x[\da-fA-F]+|\d+(\.\d+)?([eE][+-]?\d+)?f?)\b", "<color=#B5CEA8>$1</color>");

        // Class/field/property names after common keywords (e.g., class MyClass, MyField =)
        // This is approximate but works well visually
        code = Regex.Replace(code, @"\b(class|struct|interface|enum)\s+([\w]+)", "$1 <color=#4EC9B0>$2</color>");

        // LINQ and common keywords (var, get, set, yield, etc.)
        code = Regex.Replace(code, @"\b(var|get|set|value|yield|partial|where|select|from)\b", "<color=#C586C0>$1</color>");

        return code;
    }
}