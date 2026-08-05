using UnityEngine;
using UnityEngine.UI;

// "다음 라운드 입장" 버튼. 장착 슬롯(EquippedDice)이 전부 채워졌을 때만 눌리고,
// 클릭 시 기존 로딩 흐름(SceneLoader)을 그대로 재사용해 PlayScene으로 전환한다.
public class NextRoundButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string playSceneName = "PlayScene";
    [SerializeField] private GameObject hintText; // "주사위 소지 개수를 채워주세요." 안내, 장착 슬롯이 다 안 찼을 때만 표시

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
        SceneLoader.LoadWithLoading(playSceneName);
    }
}
