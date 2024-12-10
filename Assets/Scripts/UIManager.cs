using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject settingsPanel;  // 설정 패널 (Canvas 아래의 Panel)
    public Button easyButton;
    public Button hardButton;
    public Button backButton;
    public Button restartButton;

    private bool isSettingsActive = false;

    private string difficulty = "Easy";

    // TextMeshProUGUI로 텍스트를 다룰 경우 해당 변수로 텍스트를 변경할 수 있습니다.
    public TMP_Text easyButtonText;
    public TMP_Text hardButtonText;
    public TMP_Text difficultyText;  // 난이도 텍스트 UI

    private Character character;  // Character 스크립트 참조

    // 버튼의 색상을 저장할 변수 추가
    public Color defaultButtonColor;
    public Color selectedButtonColor;

    void Start()
    {
        settingsPanel.SetActive(false);  // 게임 시작 시 설정 창은 보이지 않게 설정

        // Character 스크립트 참조 가져오기
        character = FindObjectOfType<Character>();

        // 버튼 이벤트 연결
        easyButton.onClick.AddListener(SetEasy);
        hardButton.onClick.AddListener(SetHard);
        backButton.onClick.AddListener(BackToGame);
        restartButton.onClick.AddListener(RestartGame);

        // 기본 버튼 색 설정
        defaultButtonColor = easyButton.GetComponent<Image>().color;
        selectedButtonColor = Color.green;  // 선택된 버튼 색상을 초록색으로 설정
    }

    void Update()
    {
        // ESC 키 입력 시 설정 창 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    void ToggleSettings()
    {
        isSettingsActive = !isSettingsActive;
        settingsPanel.SetActive(isSettingsActive);

        // 설정 창이 열릴 때 게임 일시 정지, 창을 닫을 때 게임 진행
        if (isSettingsActive)
        {
            Time.timeScale = 0;  // 설정 창이 열리면 게임 일시 정지
        }
        else
        {
            Time.timeScale = 1;  // 설정 창이 닫히면 게임 진행
        }
    }


    void SetEasy()
    {
        difficulty = "Easy";
        if (character != null)
        {
            character.SetEasyDifficulty(); // 파라미터 없이 호출
        }

        // 난이도 텍스트 업데이트
        UpdateDifficultyText();

        // 버튼 색 변경
        easyButton.GetComponent<Image>().color = selectedButtonColor;
        hardButton.GetComponent<Image>().color = defaultButtonColor;
    }

    void SetHard()
    {
        difficulty = "Hard";
        if (character != null)
        {
            character.SetHardDifficulty(); // 파라미터 없이 호출
        }

        // 난이도 텍스트 업데이트
        UpdateDifficultyText();

        // 버튼 색 변경
        hardButton.GetComponent<Image>().color = selectedButtonColor;
        easyButton.GetComponent<Image>().color = defaultButtonColor;
    }

    void BackToGame()
    {
        ToggleSettings();  // 설정 화면 토글
    }

    void RestartGame()
{
    Time.timeScale = 1;  // 게임이 멈춰있는 상태라면, 시간 흐름을 다시 정상으로 설정
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // 현재 씬을 다시 로드하여 재시작
}


    // 난이도 텍스트를 업데이트하는 함수
    void UpdateDifficultyText()
    {
        if (difficultyText != null)
        {
            difficultyText.text = "Difficulty: " + difficulty;
        }
    }
}
