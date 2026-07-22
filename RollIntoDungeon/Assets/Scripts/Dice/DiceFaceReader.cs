using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class DiceFaceReader : MonoBehaviour
{
    [Serializable]
    public struct DiceFace
    {
        public Vector3 localNormal; // 이 면이 위를 향할 때의 로컬 바깥쪽 방향
        public int value;           // 이 면의 눈금 숫자
    }

    [SerializeField]
    private DiceFace[] faces = new DiceFace[]
    {
        new DiceFace { localNormal = Vector3.up,      value = 1 },
        new DiceFace { localNormal = Vector3.down,    value = 6 },
        new DiceFace { localNormal = Vector3.right,   value = 2 },
        new DiceFace { localNormal = Vector3.left,    value = 5 },
        new DiceFace { localNormal = Vector3.forward, value = 3 },
        new DiceFace { localNormal = Vector3.back,    value = 4 },
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
                Debug.Log($"{name} 결과: {GetTopFaceValue()}");
            }
        }
        else
        {
            hasReportedResult = false;
        }
    }

    public int GetTopFaceValue()
    {
        int bestValue = -1;
        float bestDot = float.MinValue;

        foreach (var face in faces)
        {
            Vector3 worldNormal = transform.TransformDirection(face.localNormal);
            float dot = Vector3.Dot(worldNormal, Vector3.up);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestValue = face.value;
            }
        }

        return bestValue;
    }

    [ContextMenu("현재 위를 보는 로컬 방향 출력 (캘리브레이션용)")]
    private void LogCurrentUpLocalDirection()
    {
        Vector3 localUp = transform.InverseTransformDirection(Vector3.up);
        Debug.Log($"[캘리브레이션] 지금 위를 향하는 로컬 방향: {localUp} — 이 값을 해당 눈금의 Local Normal에 입력하세요.");
    }
}
