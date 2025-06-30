using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using Unity.VisualScripting.Dependencies.Sqlite;

[AddComponentMenu("UI/Better Image")]
public class BetterImage : Image
{
    [SerializeField]
    private bool useCornerRadiusPercent = false;
    [SerializeField, Min(0)]
    private float cornerRadius = 20f;
    [SerializeField, Min(0)]
    private float cornerRadiusPercent = 20f;
    [SerializeField, Tooltip("The amount of segments to draw the corner with. Minimum 1."), Min(1)]
    int segments = 16; // Number of segments to approximate corners

    public float CornerRadius
    {
        get => cornerRadius;
        set
        {
            cornerRadius = value;
            SetVerticesDirty(); // Mark mesh for regeneration
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Rect rect = GetPixelAdjustedRect();
        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;

        if(useCornerRadiusPercent)
        {
            cornerRadius = Mathf.Min(rect.width / 2f, rect.height / 2f) * (cornerRadiusPercent / 100);
        }

        AddRoundedRect(vh, rect, uv, cornerRadius);
    }

    private void AddRoundedRect(VertexHelper vh, Rect rect, Vector4 uv, float radius)
    {
        // Clamp radius to half of the shortest side
        float clampedRadius = Mathf.Min(radius, rect.width / 2f, rect.height / 2f);

        // Helper method to add a rounded corner
        void AddCorner(Vector2 center, float startAngle, float endAngle)
        {
            float angleStep = (endAngle - startAngle) / segments;
            Vector2 prev = center + new Vector2(Mathf.Cos(startAngle * Mathf.Deg2Rad), Mathf.Sin(startAngle * Mathf.Deg2Rad)) * clampedRadius;

            for(int i = 1; i <= segments; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector2 next = center + new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * clampedRadius;

                vh.AddUIVertexQuad(CreateQuad(center, prev, next, rect, uv));
                prev = next;
            }
        }

        // Helper method to fill the space between the corners
        void AddConnector(Vector2 OutsideOne, Vector2 OutsideTwo, bool isHorizontal)
        {
            vh.AddUIVertexQuad(CreateQuad(OutsideOne, OutsideOne, OutsideTwo, rect, uv));

            if(isHorizontal)
            {
                vh.AddUIVertexQuad(CreateQuad(
                    OutsideOne,
                    OutsideTwo,
                    new Vector2(OutsideTwo.x, OutsideTwo.y - clampedRadius),
                    rect,
                    uv
                ));
                vh.AddUIVertexQuad(CreateQuad(
                    OutsideOne,
                    new Vector2(OutsideTwo.x, OutsideTwo.y - clampedRadius),
                    new Vector2(OutsideOne.x, OutsideOne.y - clampedRadius),
                    rect,
                    uv
                ));
                vh.AddUIVertexQuad(CreateQuad(
                    OutsideOne,
                    new Vector2(OutsideOne.x, OutsideOne.y - clampedRadius),
                    OutsideOne,
                    rect,
                    uv
                ));
            }
            else // vertical
            {
                vh.AddUIVertexQuad(CreateQuad(
                    OutsideOne,
                    OutsideTwo,
                    new Vector2(OutsideTwo.x + clampedRadius, OutsideTwo.y),
                    rect,
                    uv
                ));
                vh.AddUIVertexQuad(CreateQuad(
                    OutsideOne,
                    new Vector2(OutsideTwo.x + clampedRadius, OutsideTwo.y),
                    new Vector2(OutsideOne.x + clampedRadius, OutsideOne.y),
                    rect,
                    uv
                ));
                vh.AddUIVertexQuad(CreateQuad(
                    OutsideOne,
                    new Vector2(OutsideOne.x + clampedRadius, OutsideOne.y),
                    OutsideOne,
                    rect,
                    uv
                ));
            }
        }

        Vector2 min = rect.min;
        Vector2 max = rect.max;

        // Calculate corner centers
        Vector2 topLeft = new Vector2(min.x + clampedRadius, max.y - clampedRadius);
        Vector2 topRight = new Vector2(max.x - clampedRadius, max.y - clampedRadius);
        Vector2 bottomRight = new Vector2(max.x - clampedRadius, min.y + clampedRadius);
        Vector2 bottomLeft = new Vector2(min.x + clampedRadius, min.y + clampedRadius);

        // fill four corners
        AddCorner(topLeft, 90f, 180f);
        AddCorner(topRight, 0f, 90f);
        AddCorner(bottomRight, 270f, 360f);
        AddCorner(bottomLeft, 180f, 270f);

        // connect space between the corners
        AddConnector(new Vector2(topLeft.x, topLeft.y + clampedRadius), new Vector2(topRight.x, topRight.y + clampedRadius), true);
        AddConnector(new Vector2(bottomLeft.x, bottomLeft.y), new Vector2(bottomRight.x, bottomRight.y), true);
        AddConnector(new Vector2(topLeft.x - clampedRadius, topLeft.y), new Vector2(bottomLeft.x - clampedRadius, bottomLeft.y), false);
        AddConnector(new Vector2(topRight.x, topRight.y), new Vector2(bottomRight.x, bottomRight.y), false);

        // fill the middle
        vh.AddUIVertexQuad(CreateQuad(topLeft, topLeft, topRight, rect, uv));
        vh.AddUIVertexQuad(CreateQuad(topLeft, topRight, bottomRight, rect, uv));
        vh.AddUIVertexQuad(CreateQuad(topLeft, bottomRight, bottomLeft, rect, uv));
        vh.AddUIVertexQuad(CreateQuad(topLeft, bottomLeft, topLeft, rect, uv));
    }

    private UIVertex[] CreateQuad(Vector2 center, Vector2 p1, Vector2 p2, Rect rect, Vector4 uv)
    {
        Color32 color32 = color;
        UIVertex[] verts = new UIVertex[4];

        verts[0].position = center;
        verts[1].position = p1;
        verts[2].position = p2;
        verts[3].position = center; // optional duplicate

        for(int i = 0; i < verts.Length; i++)
        {
            verts[i].color = color32;

            // Convert position to UV
            Vector2 pos = verts[i].position;
            float u = Mathf.Lerp(uv.x, uv.z, Mathf.InverseLerp(rect.xMin, rect.xMax, pos.x));
            float v = Mathf.Lerp(uv.y, uv.w, Mathf.InverseLerp(rect.yMin, rect.yMax, pos.y));
            verts[i].uv0 = new Vector2(u, v);
        }

        return verts;
    }
}
