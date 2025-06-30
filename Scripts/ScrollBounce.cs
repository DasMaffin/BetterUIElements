using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollBounce : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float speed = 1f;
    public float pauseTime = 1f;
    public bool horizontal;
    public bool vertical;

    private ScrollRect scrollRect;
    private float hDir = 1f;
    private float vDir = 1f;
    private float pauseTimer = 0f;
    private bool isPointerUp = true;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    private void LateUpdate()
    {
        if(pauseTimer >= 0f)
        {
            if(isPointerUp)
                pauseTimer -= Time.deltaTime;
            return;
        }

        if(horizontal)
            scrollRect.horizontalNormalizedPosition = Bounce(scrollRect.horizontalNormalizedPosition, ref hDir);

        if(vertical)
            scrollRect.verticalNormalizedPosition = Bounce(scrollRect.verticalNormalizedPosition, ref vDir);
    }

    private float Bounce(float position, ref float direction)
    {
        position += direction * speed * Time.deltaTime;
        if(position >= 1f)
        {
            position = 1f;
            direction = -1f;
        }
        else if(position <= 0f)
        {
            position = 0f;
            direction = 1f;
        }
        return position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerUp = false;
        pauseTimer = pauseTime;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerUp = true;
    }
}
