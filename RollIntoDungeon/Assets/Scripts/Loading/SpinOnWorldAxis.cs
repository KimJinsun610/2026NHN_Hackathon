using UnityEngine;

// 초기 기울기(Transform Rotation)는 그대로 둔 채 월드 축 기준으로 계속 회전시킨다.
// 기울어진 팽이가 도는 듯한 연출 — 로딩 화면 3D 주사위 등에 사용.
public class SpinOnWorldAxis : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.up;
    [SerializeField] private float degreesPerSecond = 90f;

    void Update() => transform.Rotate(axis, degreesPerSecond * Time.deltaTime, Space.World);
}
