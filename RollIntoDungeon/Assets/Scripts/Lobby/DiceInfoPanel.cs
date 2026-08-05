using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// "선택한 주사위 정보" 패널. DiceInventoryView의 슬롯 클릭 이벤트를 구독해 상세 정보를 보여준다.
public class DiceInfoPanel : MonoBehaviour
{
    [SerializeField] private DiceInventoryView inventoryView;
    [SerializeField] private GameObject panelRoot; // 선택 전에는 숨겨둘 실제 내용물(이름/설명/이미지/버튼 등)의 루트
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image[] faceImages = new Image[6];
    [SerializeField] private DicePreviewRotator preview;
    [SerializeField] private Button equipButton;
    [SerializeField] private LobbyAudio lobbyAudio;

    // 장착 슬롯(다음 항목)이 이 이벤트를 구독하면 된다.
    public event Action<DiceData> OnEquipRequested;

    private DiceData selectedData;

    void Awake()
    {
        equipButton.interactable = false;

        if (panelRoot != null)
            panelRoot.SetActive(false); // 초기값을 채우기 전까지만 잠깐 숨김 (Start에서 소지 목록 첫 주사위로 채움)
    }

    void Start()
    {
        // DiceInventory.Awake()보다 먼저 실행될 수 있어(오브젝트 간 Awake 순서 미보장), 접근은 Start에서 수행한다.
        if (DiceInventory.Instance != null && DiceInventory.Instance.OwnedDice.Count > 0)
            HandleDiceSelected(DiceInventory.Instance.OwnedDice[0].data);
    }

    void OnEnable()
    {
        inventoryView.OnDiceSlotClicked += HandleDiceSelected;
        equipButton.onClick.AddListener(HandleEquipClicked);
    }

    void OnDisable()
    {
        inventoryView.OnDiceSlotClicked -= HandleDiceSelected;
        equipButton.onClick.RemoveListener(HandleEquipClicked);
    }

    void HandleDiceSelected(DiceData data)
    {
        selectedData = data;

        if (panelRoot != null)
            panelRoot.SetActive(true); // 슬롯을 클릭한 시점에만 내용물 표시

        nameText.text = data.displayName;
        preview.SetData(data);

        for (int i = 0; i < faceImages.Length; i++)
        {
            bool hasFace = data.faces != null && i < data.faces.Length;
            faceImages[i].enabled = hasFace;
            if (hasFace) faceImages[i].sprite = data.faces[i].image;
        }

        equipButton.interactable = true;
    }

    void HandleEquipClicked()
    {
        if (selectedData == null) return;

        lobbyAudio.PlaySelectSound();
        OnEquipRequested?.Invoke(selectedData);
    }
}
