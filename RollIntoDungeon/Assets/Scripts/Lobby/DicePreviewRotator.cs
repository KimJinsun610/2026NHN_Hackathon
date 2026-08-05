using UnityEngine;
using UnityEngine.EventSystems;

// 정보 패널의 3D 아이콘. 드래그로 모델을 직접 회전시켜 볼 수 있다.
// RawImage가 전용 프리뷰 카메라의 RenderTexture를 보여주는 구조이며, 이 스크립트는 그 RawImage에 부착한다.
public class DicePreviewRotator : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform modelPivot; // 프리뷰 카메라가 바라보는 위치에 미리 배치해 둔 피벗
    [SerializeField] private float rotateSpeed = 0.3f;

    private GameObject currentModel;

    public void SetData(DiceData data)
    {
        if (currentModel != null)
            Destroy(currentModel);

        if (data == null || data.previewPrefab == null) return;

        currentModel = Instantiate(data.previewPrefab, modelPivot);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;

        // previewPrefab 원본 에셋의 레이어가 무엇이든, 프리뷰 카메라가 보는 레이어로 강제 통일한다.
        SetLayerRecursively(currentModel, modelPivot.gameObject.layer);
    }

    static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (modelPivot == null) return;

        modelPivot.Rotate(Vector3.up, -eventData.delta.x * rotateSpeed, Space.World);
        modelPivot.Rotate(Vector3.right, eventData.delta.y * rotateSpeed, Space.World);
    }
}
