using UnityEngine;
using TMPro;

// 던전 정보창. EnemyListView의 슬롯 클릭 이벤트를 구독해 선택된 적의 상세 정보를 보여준다.
public class DungeonInfoPanel : MonoBehaviour
{
    [SerializeField] private StageData stageData;
    [SerializeField] private EnemyListView listView;

    [Header("텍스트")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private GameObject bossBadge;

    [Header("프리뷰")]
    [SerializeField] private Transform previewPivot; // 프리뷰 카메라가 바라보는 위치에 미리 배치해 둔 피벗

    private GameObject currentPreviewInstance;

    void OnEnable()
    {
        listView.OnEnemySlotClicked += HandleEnemySelected;
    }

    void OnDisable()
    {
        listView.OnEnemySlotClicked -= HandleEnemySelected;
    }

    void Start()
    {
        // 초기값: 던전에 등장하는 첫 번째 적 정보를 바로 표시
        if (stageData != null && stageData.enemiesToSpawn != null && stageData.enemiesToSpawn.Count > 0)
            HandleEnemySelected(stageData.enemiesToSpawn[0].enemyPrefab);
    }

    void HandleEnemySelected(Enemy enemy)
    {
        nameText.text = enemy.EnemyName;
        hpText.text = enemy.MaxHp.ToString();
        attackText.text = enemy.AttackPower.ToString();
        defenseText.text = enemy.DefensePower.ToString();
        bossBadge.SetActive(enemy is Boss);

        RefreshPreview(enemy);
    }

    void RefreshPreview(Enemy enemy)
    {
        if (currentPreviewInstance != null)
            Destroy(currentPreviewInstance);

        if (enemy.VisualRoot == null) return;

        currentPreviewInstance = Instantiate(enemy.VisualRoot, previewPivot);
        currentPreviewInstance.transform.localPosition = Vector3.zero;
        currentPreviewInstance.transform.localRotation = Quaternion.identity;

        // visualRoot 원본 에셋의 레이어가 무엇이든, 프리뷰 카메라가 보는 레이어로 강제 통일한다.
        SetLayerRecursively(currentPreviewInstance, previewPivot.gameObject.layer);
    }

    static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}
