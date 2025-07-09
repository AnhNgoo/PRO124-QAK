using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    public Transform rocket;
    public Transform target;
    public GameObject warning;

    private SpriteRenderer warningSprite;


    void OnEnable()
    {
        GetComponent();
        StartMove();
    }

    void GetComponent()
    {
        warningSprite = warning.GetComponent<SpriteRenderer>();
    }
    void StartMove()
    {

        Sequence sequence = DOTween.Sequence();


        sequence.AppendCallback(() =>
        {
            rocket.gameObject.SetActive(true);
            rocket.position = transform.position;
            warning.SetActive(true);
        });

        sequence.Append(warningSprite
                            .DOFade(0, 0.1f)
                            .SetEase(Ease.Linear)
                            .SetLoops(20, LoopType.Yoyo));

        sequence.AppendCallback(() => warning.SetActive(false));

        float duration = GetDuration();
        sequence.Append(rocket
                            .DOMove(target.position, duration)
                            .SetEase(Ease.Linear));
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() => gameObject.SetActive(false));
    }

    float GetDuration()
    {
        // Tính duration tỉ lệ nghịch với scrollSpeed, giới hạn trong [1, 2]
        float scrollSpeed = MapSpawner.Instance.scrollSpeed;
        float duration = Mathf.Clamp(2f / scrollSpeed, 1f, 2f);
        Debug.Log($"GetDuration trả về: {duration} với scrollSpeed: {scrollSpeed}");
        return duration;
    }
}
