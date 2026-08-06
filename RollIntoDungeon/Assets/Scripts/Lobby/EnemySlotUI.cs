using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// 던전 정보창 적 목록의 슬롯 1개. 아이콘 + 이름을 표시하고, 클릭 시 자신의 데이터를 이벤트로 발행한다.
public class EnemySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectionFrame; // 선택 시에만 활성화되는 테두리 프레임 오브젝트

    public Enemy Data { get; private set; }

    public event Action<Enemy> OnClicked;

    void Awake()
    {
        button.onClick.AddListener(HandleClicked);
    }

    void HandleClicked()
    {
        if (Data == null) return;
        OnClicked?.Invoke(Data);
    }

    public void Setup(Enemy data)
    {
        Data = data;

        iconImage.sprite = data.Icon;
        nameText.text = data.EnemyName;
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        if (selectionFrame != null)
            selectionFrame.SetActive(selected);
    }
}
