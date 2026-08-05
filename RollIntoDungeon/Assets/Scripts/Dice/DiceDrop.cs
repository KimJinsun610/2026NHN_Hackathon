using UnityEngine;

public class DiceDrop : MonoBehaviour
{
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float maxTorque = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // DiceSpawner가 동적으로 생성한 주사위에 굴림 기준점을 지정할 때 사용
    public void SetDropPoint(Transform point) => dropPoint = point;

    // Dice.cs가 굴리기를 트리거할 때 호출 (개별 R키 테스트는 DiceManager로 이동)
    public void Drop()
    {
        transform.position = dropPoint.position;
        transform.rotation = Random.rotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 randomTorque = new Vector3(
            Random.Range(-maxTorque, maxTorque),
            Random.Range(-maxTorque, maxTorque),
            Random.Range(-maxTorque, maxTorque));

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }
}
