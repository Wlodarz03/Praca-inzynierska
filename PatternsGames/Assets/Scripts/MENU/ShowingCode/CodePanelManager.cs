using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

public class CodePanelManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void DownloadCodeFile(string fileName, string content);
#endif

    public ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform codeTextRect;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private Transform tabsContainer;
    [SerializeField] private TabButton tabPrefab;
    [SerializeField] private TMP_Text info;
    [SerializeField] private GameObject infoObject;

    private DesignPatternCode currentPattern;
    private int currentFileIndex;
    private float infoDisplayTime = 1.5f;
    private float infoTimer = 0f;

    private void Update()
    {
        if (infoObject.activeSelf)
        {
            infoTimer += Time.unscaledDeltaTime;
            if (infoTimer >= infoDisplayTime)
            {
                infoObject.SetActive(false);
                infoTimer = 0f;
            }
        }
    }
    public void ShowCode(DesignPatternCode pattern)
    {
        panel.SetActive(true);
        Time.timeScale = 0f;

        currentPattern = pattern;
        titleText.text = pattern.patternName;

        CreateTabs();
        currentFileIndex = 0;
        ShowFile(0);
    }

    void CreateTabs()
    {
        foreach (Transform child in tabsContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < currentPattern.files.Count; i++)
        {
            var tab = Instantiate(tabPrefab, tabsContainer);
            tab.Init(currentPattern.files[i].fileName, i, this);
            if (i == 0)
            {
                Image img = tab.GetComponent<Image>();
                Color c = img.color;
                c.a = 1f;
                img.color = c;
            }
        }
    }

    public void ShowFile(int index)
    {
        codeText.text = HighlightSyntax(currentPattern.files[index].code);

        var tab = tabsContainer.GetChild(currentFileIndex).GetComponent<TabButton>();
        tab.GetComponent<Image>().color = new Color(
            tab.GetComponent<Image>().color.r,
            tab.GetComponent<Image>().color.g,
            tab.GetComponent<Image>().color.b,
            tab.GetAlfa()
        );

        currentFileIndex = index;

        tab = tabsContainer.GetChild(currentFileIndex).GetComponent<TabButton>();
        tab.GetComponent<Image>().color = new Color(
            tab.GetComponent<Image>().color.r,
            tab.GetComponent<Image>().color.g,
            tab.GetComponent<Image>().color.b,
            1f
        );
        
        Canvas.ForceUpdateCanvases();

        float height = codeText.preferredHeight;
        codeTextRect.sizeDelta = new Vector2(
            codeTextRect.sizeDelta.x,
            height
        );

        content.sizeDelta = new Vector2(
            content.sizeDelta.x,
            height
        );

        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        if (currentPattern.patternName == "Factory")
        {
            UnityEngine.Cursor.visible = false;
        }
    }

    public void CopyCodeToClipboard()
    {
        GUIUtility.systemCopyBuffer = currentPattern.files[currentFileIndex].code;
        info.text = "Code copied to clipboard!";
        infoObject.SetActive(true);
    }

    public void DownloadCode()
    {
        string fileName = currentPattern.files[currentFileIndex].fileName + ".cs";
        string code = currentPattern.files[currentFileIndex].code;
        info.text = "File downloading";
        infoObject.SetActive(true);

        #if UNITY_WEBGL && !UNITY_EDITOR
            DownloadCodeFile(fileName, code);
        #else
            Debug.Log("Download not supported in this platform.");
        #endif
    }

    private string HighlightSyntax(string code)
    {
        // Kolory z VS Code Dark+ theme
        const string commentColor       = "#6A9955";  // zielony
        const string keywordColor       = "#569CD6";  // niebieski (public, void, if, this)
        const string controlKeyword     = "#C586C0";  // fioletowy (var, get, set, yield)
        const string typeColor          = "#4EC9B0";  // turkusowy (int, string, PlayerMover)
        const string stringColor        = "#CE9178";  // pomarańczowy
        const string numberColor        = "#B5CEA8";  // zielonkawo-żółty
        const string memberColor        = "#9CDCFE";  // jasnoniebieski (pola, metody, parametry, wywołania)
        const string punctuationColor   = "#D4D4D4";  // szary – nawiasy, kropki itp. (opcjonalnie)

        // 1. Komentarze wielolinijkowe /* ... */
        code = Regex.Replace(code, @"/\*[\s\S]*?\*/", m => $"<color={commentColor}>{m.Value}</color>");

        // 2. Komentarze jednolinijkowe //
        code = Regex.Replace(code, @"//.*$", m => $"<color={commentColor}>{m.Value}</color>", RegexOptions.Multiline);

        // 3. Preprocesor (#if, #region itp.)
        code = Regex.Replace(code, @"^\s*(#.*)$", m => $"<color={controlKeyword}>{m.Value}</color>", RegexOptions.Multiline);

        // 4. Stringi – wszystkie rodzaje (najpierw bardziej specyficzne)
        code = Regex.Replace(code, @"\$@""[^""]*(?:""""[^""]*)*""", m => $"<color={stringColor}>{m.Value}</color>");
        code = Regex.Replace(code, @"\$""([^""\\]|\\.)*""", m => $"<color={stringColor}>{m.Value}</color>");
        code = Regex.Replace(code, @"@""[^""]*(?:""""[^""]*)*""", m => $"<color={stringColor}>{m.Value}</color>");
        code = Regex.Replace(code, @"""([^""\\]|\\.)*""", m => $"<color={stringColor}>{m.Value}</color>");

        // 5. Liczby
        code = Regex.Replace(code, @"\b\d+\.?\d*(f|F|d|D|m|M)?\b|0x[0-9a-fA-F]+", m => $"<color={numberColor}>{m.Value}</color>");

        // 6. Atrybuty [SerializeField], [Header] itp.
        code = Regex.Replace(code, @"\[\s*([A-Za-z_]\w*(?:\.[A-Za-z_]\w*)*)\s*(\([^)]*\))?\s*\]",
            m =>
            {
                string name = m.Groups[1].Value;
                string paramsPart = m.Groups[2].Success ? m.Groups[2].Value : "";
                return $"<color=#808080>[</color><color={typeColor}>{name}</color>{paramsPart}<color=#808080>]</color>";
            }, RegexOptions.Multiline);

        // 7. Słowa kluczowe kontekstowe (var, get, set, yield, where itp.)
        code = Regex.Replace(code, @"\b(var|get|set|value|yield|partial|where|select|from|add|remove|async|await|global)\b",
            m => $"<color={controlKeyword}>{m.Value}</color>");

        // 8. Stałe true / false / null
        code = Regex.Replace(code, @"\b(true|false|null)\b", m => $"<color={keywordColor}>{m.Value}</color>");

        // 9. Wbudowane typy (int, string, bool, Vector3 itp.)
        code = Regex.Replace(code, @"\b(void|bool|byte|sbyte|char|decimal|double|float|int|uint|long|ulong|short|ushort|object|string|dynamic)\b",
            m => $"<color={typeColor}>{m.Value}</color>");

        // 10. Główne słowa kluczowe (public, class, if, this itp.)
        code = Regex.Replace(code, @"\b(abstract|as|base|break|case|catch|checked|class|const|continue|default|delegate|do|else|enum|event|explicit|extern|finally|fixed|for|foreach|goto|if|implicit|in|interface|internal|is|lock|namespace|new|operator|out|override|params|private|protected|public|readonly|ref|return|sealed|sizeof|stackalloc|static|struct|switch|this|throw|try|typeof|unchecked|unsafe|using|virtual|volatile|while)\b",
            m => $"<color={keywordColor}>{m.Value}</color>");

        // 11. Nazwy typów po słowach kluczowych (class MyClass, interface ICommand)
        code = Regex.Replace(code, @"\b(class|struct|interface|enum|delegate)\b\s+([A-Za-z_]\w*)",
            m => $"<color={keywordColor}>{m.Groups[1].Value}</color> <color={typeColor}>{m.Groups[2].Value}</color>");

        // 12. Kolorowanie nazw metod i właściwości w deklaracjach i wywołaniach
        // Łapie nazwę przed nawiasem otwierającym (np. void Execute(), player.Move())
        code = Regex.Replace(code, @"(\.)?([A-Za-z_]\w*)\s*\(", m =>
        {
            string name = m.Groups[2].Value;

            // Pomijamy typy i słowa kluczowe, żeby nie pokolorować np. String.Format
            if (Regex.IsMatch(name, @"^(void|bool|int|string|object|true|false|null|this|new|public|private)$"))
                return m.Value;

            string prefix = m.Groups[1].Success ? m.Groups[1].Value : "";
            return $"{prefix}<color={memberColor}>{name}</color>(";
        });

        // 13. this. – koloruje 'this' i pole po kropce
        code = Regex.Replace(code, @"\b(this)\.([A-Za-z_]\w*)", 
            m => $"<color={keywordColor}>this</color>.<color={memberColor}>{m.Groups[2].Value}</color>");

        // 14. Opcjonalnie: jaśniejsze znaki interpunkcyjne (kropki, nawiasy, średniki)
        code = Regex.Replace(code, @"([{}();,])", m => $"<color={punctuationColor}>{m.Value}</color>");
        code = Regex.Replace(code, @"\.", m => $"<color={punctuationColor}>.</color>");
        
        return code;
    }
}
