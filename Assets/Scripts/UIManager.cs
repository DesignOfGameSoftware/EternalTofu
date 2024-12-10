using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject settingsPanel;  // ���� �г� (Canvas �Ʒ��� Panel)
    public Button easyButton;
    public Button hardButton;
    public Button backButton;
    public Button restartButton;

    private bool isSettingsActive = false;

    private string difficulty = "Easy";

    // TextMeshProUGUI�� �ؽ�Ʈ�� �ٷ� ��� �ش� ������ �ؽ�Ʈ�� ������ �� �ֽ��ϴ�.
    public TMP_Text easyButtonText;
    public TMP_Text hardButtonText;
    public TMP_Text difficultyText;  // ���̵� �ؽ�Ʈ UI

    private Character character;  // Character ��ũ��Ʈ ����

    // ��ư�� ������ ������ ���� �߰�
    public Color defaultButtonColor;
    public Color selectedButtonColor;

    void Start()
    {
        settingsPanel.SetActive(false);  // ���� ���� �� ���� â�� ������ �ʰ� ����

        // Character ��ũ��Ʈ ���� ��������
        character = FindObjectOfType<Character>();

        // ��ư �̺�Ʈ ����
        easyButton.onClick.AddListener(SetEasy);
        hardButton.onClick.AddListener(SetHard);
        backButton.onClick.AddListener(BackToGame);
        restartButton.onClick.AddListener(RestartGame);

        // �⺻ ��ư �� ����
        defaultButtonColor = easyButton.GetComponent<Image>().color;
        selectedButtonColor = Color.green;  // ���õ� ��ư ������ �ʷϻ����� ����
    }

    void Update()
    {
        // ESC Ű �Է� �� ���� â ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    void ToggleSettings()
    {
        isSettingsActive = !isSettingsActive;
        settingsPanel.SetActive(isSettingsActive);

        // ���� â�� ���� �� ���� �Ͻ� ����, â�� ���� �� ���� ����
        if (isSettingsActive)
        {
            Time.timeScale = 0;  // ���� â�� ������ ���� �Ͻ� ����
        }
        else
        {
            Time.timeScale = 1;  // ���� â�� ������ ���� ����
        }
    }


    void SetEasy()
    {
        difficulty = "Easy";
        if (character != null)
        {
            character.SetEasyDifficulty(); // �Ķ���� ���� ȣ��
        }

        // ���̵� �ؽ�Ʈ ������Ʈ
        UpdateDifficultyText();

        // ��ư �� ����
        easyButton.GetComponent<Image>().color = selectedButtonColor;
        hardButton.GetComponent<Image>().color = defaultButtonColor;
    }

    void SetHard()
    {
        difficulty = "Hard";
        if (character != null)
        {
            character.SetHardDifficulty(); // �Ķ���� ���� ȣ��
        }

        // ���̵� �ؽ�Ʈ ������Ʈ
        UpdateDifficultyText();

        // ��ư �� ����
        hardButton.GetComponent<Image>().color = selectedButtonColor;
        easyButton.GetComponent<Image>().color = defaultButtonColor;
    }

    void BackToGame()
    {
        ToggleSettings();  // ���� ȭ�� ���
    }

    void RestartGame()
{
    Time.timeScale = 1;  // ������ �����ִ� ���¶��, �ð� �帧�� �ٽ� �������� ����
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // ���� ���� �ٽ� �ε��Ͽ� �����
}


    // ���̵� �ؽ�Ʈ�� ������Ʈ�ϴ� �Լ�
    void UpdateDifficultyText()
    {
        if (difficultyText != null)
        {
            difficultyText.text = "Difficulty: " + difficulty;
        }
    }
}
