using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class AdvancedMonologue : MonoBehaviour
{
    // ========== НАСТРОЙКИ В ИНСПЕКТОРЕ ==========
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image backgroundPanel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Color backgroundColor = new Color(0, 0, 0, 0.7f);

    [Header("Content")]
    [TextArea(3, 10)]
    [SerializeField] private string[] monologueLines;

    private int currentLine = 0;
    private bool isTyping = false;

    void Start()
    {
        InitializeUIComponents();
        InitializeDialogue();
    }

    void InitializeUIComponents()
    {
        // Создаем CanvasGroup если отсутствует
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Автоматическое создание фоновой панели
        if (backgroundPanel == null)
        {
            CreateBackgroundPanel();
        }

        backgroundPanel.color = backgroundColor;
        backgroundPanel.gameObject.SetActive(false);
        canvasGroup.alpha = 0f;
    }

    void CreateBackgroundPanel()
    {
        GameObject panelObj = new GameObject("DialogueBackground");
        panelObj.transform.SetParent(dialogueText.transform.parent);
        backgroundPanel = panelObj.AddComponent<Image>();
        backgroundPanel.color = backgroundColor;

        RectTransform rt = panelObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.SetAsFirstSibling();
    }

    void InitializeDialogue()
    {
        currentLine = 0;
        StartCoroutine(FadeInDialogue());
    }

    IEnumerator FadeInDialogue()
    {
        backgroundPanel.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        StartCoroutine(RunDialogue());
    }

    IEnumerator RunDialogue()
    {
        while (currentLine < monologueLines.Length)
        {
            yield return StartCoroutine(TypeText(monologueLines[currentLine]));
            yield return new WaitForSeconds(1f);
            currentLine++;
        }

        yield return StartCoroutine(FadeOutDialogue());
    }

    // Добавленный метод TypeText
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    IEnumerator FadeOutDialogue()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        backgroundPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = monologueLines[currentLine];
                isTyping = false;
            }
            else if (currentLine < monologueLines.Length - 1)
            {
                currentLine++;
                StartCoroutine(TypeText(monologueLines[currentLine]));
            }
            else
            {
                StartCoroutine(FadeOutDialogue());
            }
        }
    }

    public void StartDialogueManually()
    {
        InitializeDialogue();
    }
}