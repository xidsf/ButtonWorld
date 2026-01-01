using System.Drawing;
using UnityEngine;

public class PlayerSensors : MonoBehaviour
{
    private PlayerController player;

    const float GROUND_CHECK_DISTANCE = 0.01f;
    const float GROUND_CHECK_AREA_GAP = 0.1f;
    const float PLAYER_SAVE_GAP = 0.1f;
    [SerializeField] bool isBottomContacted, isTopContacted, isLeftContacted, isRightContacted;
    [SerializeField] bool isStucked = false;
    int combinedLayerMask;

    public bool IsBottomContacted() { return isBottomContacted; }
    public bool IsTopContacted() { return isTopContacted; }
    public bool IsLeftContacted() { return isLeftContacted; }
    public bool IsRightContacted() { return isRightContacted; }
    public bool IsStucked() { return isStucked; }


    private void Start()
    {
        player = GetComponent<PlayerController>();
        combinedLayerMask = player.GetGroundLayerMask() | player.ButtonLayerMask();
    }

    private void FixedUpdate()
    {
        AroundCheck();
        StuckCheck();
    }

    private void AroundCheck()
    {
        var bounds = player.MyColli.bounds;

        Vector2 pos;
        Vector2 size;
        Collider2D coli;

        {
            pos = new Vector2(bounds.center.x, bounds.center.y - bounds.extents.y - GROUND_CHECK_DISTANCE);
            size = new Vector2(bounds.size.x - GROUND_CHECK_AREA_GAP, GROUND_CHECK_DISTANCE * 2f);
            coli = Physics2D.OverlapBox(pos, size, 0f, combinedLayerMask);
            DrawBoxCast(pos, size, 0f, Vector2.zero, 0f);
            isBottomContacted = (coli != null);

            pos = new Vector2(bounds.center.x, bounds.center.y + bounds.extents.y + GROUND_CHECK_DISTANCE);
            coli = Physics2D.OverlapBox(pos, size, 0f, combinedLayerMask);
            DrawBoxCast(pos, size, 0f, Vector2.zero, 0f);
            isTopContacted = (coli != null);
        }
        {
            pos = new Vector2(bounds.center.x - bounds.extents.x - GROUND_CHECK_DISTANCE, bounds.center.y);
            size = new Vector2(GROUND_CHECK_DISTANCE * 2f, bounds.size.y - GROUND_CHECK_AREA_GAP);
            coli = Physics2D.OverlapBox(pos, size, 0f, combinedLayerMask);
            DrawBoxCast(pos, size, 0f, Vector2.zero, 0f);
            isLeftContacted = (coli != null);
            pos = new Vector2(bounds.center.x + bounds.extents.x + GROUND_CHECK_DISTANCE, bounds.center.y);
            coli = Physics2D.OverlapBox(pos, size, 0f, combinedLayerMask);
            DrawBoxCast(pos, size, 0f, Vector2.zero, 0f);
            isRightContacted = (coli != null);
        }
    }

    private void StuckCheck()
    {
        var bounds = player.MyColli.bounds;
        Vector2 pos = bounds.center;
        Vector2 size = new Vector2(bounds.size.x - PLAYER_SAVE_GAP, bounds.size.y - PLAYER_SAVE_GAP);
        Collider2D collider = //Physics2D.OverlapBox(pos, size, 0f, combinedLayerMask);
        Physics2D.OverlapCapsule(pos, size, CapsuleDirection2D.Vertical, 0f, combinedLayerMask);
        if (collider != null)
        {
            if ((isTopContacted && isBottomContacted) || (isLeftContacted && isRightContacted))
            {
                isStucked = true;
            }
        }
    }

    private void DrawBoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
    {
        // 1. 박스의 기본 네 모서리 좌표 설정
        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

        UnityEngine.Color color = UnityEngine.Color.red;

        Vector2[] corners = new Vector2[4];
        corners[0] = new Vector2(-halfWidth, halfHeight); // 좌상
        corners[1] = new Vector2(halfWidth, halfHeight); // 우상
        corners[2] = new Vector2(halfWidth, -halfHeight); // 우하
        corners[3] = new Vector2(-halfWidth, -halfHeight); // 좌하

        // 2. 회전(Angle) 적용 및 시작/끝 지점 계산
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2[] startCorners = new Vector2[4];
        Vector2[] endCorners = new Vector2[4];

        for (int i = 0; i < 4; i++)
        {
            startCorners[i] = origin + (Vector2)(rotation * corners[i]);
            endCorners[i] = origin + (Vector2)(rotation * corners[i]) + direction.normalized * distance;
        }

        // 3. 그리기 (시작 박스, 끝 박스, 연결선)
        for (int i = 0; i < 4; i++)
        {
            int next = (i + 1) % 4;
            Debug.DrawLine(startCorners[i], startCorners[next], color); // 시작 박스
            Debug.DrawLine(endCorners[i], endCorners[next], color);     // 끝 박스
            Debug.DrawLine(startCorners[i], endCorners[i], color);      // 연결선
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.green;

        var bounds = player.MyColli.bounds;
        Vector2 pos = bounds.center;
        Vector2 size = new Vector2(bounds.size.x - PLAYER_SAVE_GAP, bounds.size.y - PLAYER_SAVE_GAP);

        Gizmos.matrix = Matrix4x4.identity;

        DrawWireCapsule(pos, size, CapsuleDirection2D.Vertical);
    }

    private void DrawWireCapsule(Vector3 center, Vector2 size, CapsuleDirection2D direction)
    {
        float width = size.x;
        float height = size.y;

        if (direction == CapsuleDirection2D.Vertical)
        {
            float radius = width / 2f;
            float cylinderHeight = Mathf.Max(0, height - width);
            float halfHeight = cylinderHeight / 2f;

            // 모든 좌표에 'center'를 더해줍니다.
            DrawArc(center + new Vector3(0, halfHeight, 0), radius, 0, 180);
            DrawArc(center + new Vector3(0, -halfHeight, 0), radius, 180, 180);

            Gizmos.DrawLine(center + new Vector3(-radius, halfHeight, 0), center + new Vector3(-radius, -halfHeight, 0));
            Gizmos.DrawLine(center + new Vector3(radius, halfHeight, 0), center + new Vector3(radius, -halfHeight, 0));
        }
        else // Horizontal
        {
            float radius = height / 2f;
            float cylinderWidth = Mathf.Max(0, width - height);
            float halfWidth = cylinderWidth / 2f;

            DrawArc(center + new Vector3(halfWidth, 0, 0), radius, -90, 180);
            DrawArc(center + new Vector3(-halfWidth, 0, 0), radius, 90, 180);

            Gizmos.DrawLine(center + new Vector3(-halfWidth, radius, 0), center + new Vector3(halfWidth, radius, 0));
            Gizmos.DrawLine(center + new Vector3(-halfWidth, -radius, 0), center + new Vector3(halfWidth, -radius, 0));
        }
    }

    private void DrawArc(Vector3 center, float radius, float startAngle, float sweepAngle)
    {
        int segments = 10;
        float step = sweepAngle / segments;
        Vector3 lastPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float a = (startAngle + (i * step)) * Mathf.Deg2Rad;
            // 여기에서도 각 점의 위치에 center를 더해줍니다.
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, 0);

            if (i > 0) Gizmos.DrawLine(lastPoint, nextPoint);
            lastPoint = nextPoint;
        }
    }
}