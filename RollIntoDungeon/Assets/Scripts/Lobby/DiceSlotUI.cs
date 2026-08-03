using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// 주사위 목록의 슬롯 1개. 아이콘 + 이름 + 소유 개수를 표시하고, 클릭 시 자신의 데이터를 이벤트로 발행한다.
// 빈 슬롯은 SetEmpty()로 아이콘/이름/개수를 지우고 클릭을 막는다.
public class DiceSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Button button;

    public DiceData Data { get; private set; }
    public bool IsEmpty => Data == null;

    public event Action<DiceData> OnClicked;

    void Awake()
    {
        button.onClick.AddListener(HandleClicked);
    }

    void HandleClicked()
    {
        if (IsEmpty) return;
        OnClicked?.Invoke(Data);
    }

    public void Setup(DiceData data, int count)
    {
        Data = data;

        iconImage.enabled = true;
        iconImage.sprite = data.icon;
        nameText.text = data.displayName;
        countText.text = $"x{count}";
        button.interactable = true;
    }

    public void SetEmpty()
    {
        Data = null;

        iconImage.enabled = false;
        nameText.text = string.Empty;
        countText.text = string.Empty;
        button.interactable = false;
    }
}
