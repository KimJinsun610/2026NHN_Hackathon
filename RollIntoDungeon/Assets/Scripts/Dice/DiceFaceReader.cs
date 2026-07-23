using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class DiceFaceReader : MonoBehaviour
{
    public enum DiceFaceType
    {
        Attack,
        Defense
    }

    [Serializable]
    public struct DiceFace
    {
        public Vector3 localNormal;   // 이 면이 위를 향할 때의 로컬 바깥쪽 방향
        public DiceFaceType faceType; // 공격면 / 방어면
        public int value;             // 이 면의 적용 값
    }

    [SerializeField]
    private DiceFace[] faces = new DiceFace[]
    {
        new DiceFace { localNormal = Vector3.up,      faceType = DiceFaceType.Attack,  value = 1 },
        new DiceFace { localNormal = Vector3.down,    faceType = DiceFaceType.Defense, value = 6 },
        new DiceFace { localNormal = Vector3.right,   faceType = DiceFaceType.Attack,  value = 2 },
        new DiceFace { localNormal = Vector3.left,    faceType = DiceFaceType.Defense, value = 5 },
        new DiceFace { localNormal = Vector3.forward, faceType = DiceFaceType.Attack,  value = 3 },
        new DiceFace { localNormal = Vector3.back,    faceType = DiceFaceType.Defense, value = 4 },
    };

    private Rigidbody rb;
    private bool hasReportedResult;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.IsSleeping())
        {
            if (!hasReportedResult)
            {
                hasReportedResult = true;
                DiceFace result = GetTopFace();
                Debug.Log($"{name} 결과: {result.faceType} {result.value}");
            }
        }
        else
        {
            hasReportedResult = false;
        }
    }

    // 현재 위를 향하고 있는 면(타입 + 값)을 판단해서 반환
    public DiceFace GetTopFace()
    {
        DiceFace bestFace = faces[0];
        float bestDot = float.MinValue;

        foreach (var face in faces)
        {
            Vector3 worldNormal = transform.TransformDirection(face.localNormal);
            float dot = Vector3.Dot(worldNormal, Vector3.up);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestFace = face;
            }
        }

        return bestFace;
    }

    public int GetTopFaceValue() => GetTopFace().value;

    public DiceFaceType GetTopFaceType() => GetTopFace().faceType;

    [ContextMenu("현재 위를 보는 로컬 방향 출력 (캘리브레이션용)")]
    private void LogCurrentUpLocalDirection()
    {
        Vector3 localUp = transform.InverseTransformDirection(Vector3.up);
        Debug.Log($"[캘리브레이션] 지금 위를 향하는 로컬 방향: {localUp} — 이 값을 해당 눈금의 Local Normal에 입력하세요.");
    }
}
