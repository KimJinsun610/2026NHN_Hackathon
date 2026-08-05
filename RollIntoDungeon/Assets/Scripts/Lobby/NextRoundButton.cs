using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// "다음 라운드 입장" 버튼. 장착 슬롯(EquippedDice)이 전부 채워졌을 때만 눌리고,
// 클릭 시 기존 로딩 흐름(SceneLoader)을 그대로 재사용해 PlayScene으로 전환한다.
public class NextRoundButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string playSceneName = "PlayScene";
    [SerializeField] private GameObject hintText; // "주사위 소지 개수를 채워주세요." 안내, 장착 슬롯이 다 안 찼을 때만 표시
    [SerializeField] private LobbyAudio lobbyAudio;

    void OnEnable()
    {
        button.onClick.AddListener(HandleClicked);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(HandleClicked);

        if (EquippedDice.Instance != null)
            EquippedDice.Instance.OnEquippedChanged -= Refresh;
    }

    void Start()
    {
        // EquippedDice.Instance 접근은 Awake 순서 미보장 문제로 Start에서 수행 (RerollCountView와 동일 이유)
        EquippedDice.Instance.OnEquippedChanged += Refresh;
        Refresh();
    }

    void Refresh()
    {
        bool isFull = EquippedDice.Instance.IsFull;

        button.interactable = isFull;

        if (hintText != null)
            hintText.SetActive(!isFull);
    }

    void HandleClicked()
    {
        if (!EquippedDice.Instance.IsFull) return;

        button.interactable = false; // 사운드 재생 대기 중 중복 클릭 방지
        lobbyAudio.PlaySelectSound();
        StartCoroutine(LoadAfterSound());
    }

    // SceneManager.LoadScene은 동기 호출이라 즉시 씬이 언로드되면서 AudioSource까지 같이 파괴된다.
    // 클릭 사운드가 실제로 들리도록 재생 길이만큼 전환을 늦춘다.
    IEnumerator LoadAfterSound()
    {
        yield return new WaitForSeconds(lobbyAudio.SelectSoundLength);
        SceneLoader.LoadWithLoading(playSceneName);
    }
}
