using UnityEngine;

// 로딩 아이콘 등 단순 회전 연출이 필요한 UI 오브젝트에 부착.
public class SpinningIcon : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 180f;

    void Update() => transform.Rotate(0f, 0f, -degreesPerSecond * Time.deltaTime);
}
