using UnityEngine;

public class PlayerSensors : MonoBehaviour
{
    private PlayerController player;

    const float GROUND_CHECK_DISTANCE = 0.01f;
    [SerializeField] bool isBottomContacted, isTopContacted, isLeftContacted, isRightContacted;

    public bool IsBottomContacted() { return isBottomContacted; }
    public bool IsTopContacted() { return isTopContacted; }
    public bool IsLeftContacted() { return isLeftContacted; }
    public bool IsRightContacted() { return isRightContacted; }

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        AroundCheck();
    }

    private void AroundCheck()
    {
        var bounds = player.MyColli.bounds;

        Vector2 pos;
        Vector2 size;
        Collider2D coli;

        int combinedLayerMask = player.GetGroundLayerMask() | player.ButtonLayerMask();

        {
            pos = new Vector2(bounds.center.x, bounds.center.y - bounds.extents.y - GROUND_CHECK_DISTANCE);
            size = new Vector2(bounds.size.x * 0.9f, GROUND_CHECK_DISTANCE * 2f);
            coli = Physics2D.OverlapBox(pos, size, 0f, combinedLayerMask);
            DrawBoxCast(pos, size, 0f, Vector2.zero, 0f, Color.red);
            isBottomContacted = (coli != null);
        }
        {    
            pos = new Vector2(bounds.center.x, bounds.center.y + bounds.extents.y + GROUND_CHECK_DISTANCE);
            size = new Vector2(bounds.size.x * 0.9f, GROUND_CHECK_DISTANCE * 2f);
            coli = Physics2D.OverlapBox(pos, size, 0f, combinedLayerMask);
            DrawBoxCast(pos, size, 0f, Vector2.zero, 0f, Color.red);
            isTopContacted = (coli != null);
        }
        {
            pos = new Vector2(bounds.center.x - bounds.extents.x - GROUND_CHECK_DISTANCE, bounds.center.y);
            size = new Vector2(GROUND_CHECK_DISTANCE * 2f, bounds.size.y * 0.9f);
            coli = Physics2D.OverlapBox(pos, size, 0f, combinedLayerMask);
            DrawBoxCast(pos, size, 0f, Vector2.zero, 0f, Color.red);
            isLeftContacted = (coli != null);
        }
        {
            pos = new Vector2(bounds.center.x + bounds.extents.x + GROUND_CHECK_DISTANCE, bounds.center.y);
            size = new Vector2(GROUND_CHECK_DISTANCE * 2f, bounds.size.y * 0.9f);
            coli = Physics2D.OverlapBox(pos, size, 0f, combinedLayerMask);
            DrawBoxCast(pos, size, 0f, Vector2.zero, 0f, Color.red);
            isRightContacted = (coli != null);
        }
    }

    private void DrawBoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, Color color)
    {
        // 1. 박스의 기본 네 모서리 좌표 설정
        float halfWidth = size.x / 2f;
        float halfHeight = size.y / 2f;

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
}
