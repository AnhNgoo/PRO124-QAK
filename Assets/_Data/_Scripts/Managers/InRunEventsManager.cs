using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRunEventsManager : Singleton<InRunEventsManager>
{
    public Transform smallEvents;
    public Transform bigEvents;

    public GameObject warning;
    public GameObject obstacleBlock;

    public float smallEventDistance = 500f;
    public float bigEventDistance = 5000f;

    private float nextSmallEventDistance = 0;
    private float nextBigEventDistance = 0;
    public bool isBigEventActive { get; set; } = false;

    void Start()
    {
        nextSmallEventDistance = smallEventDistance;
        nextBigEventDistance = bigEventDistance;
    }

    private void Update()
    {
        BigEvent();
        SmallEvent();
    }

    /// <summary>
    /// Kiểm tra khoảng cách và kích hoạt sự kiện lớn (boss,..) <br />
    /// Bật isBigEventActive để không kích hoạt các sự kiện khác
    /// </summary>
    private void BigEvent()
    {
        if (DistanceTracker.Instance.distanceTraveled >= nextBigEventDistance && !isBigEventActive)
        {
            nextBigEventDistance += bigEventDistance;
            isBigEventActive = true;

            CutSceneBigEvent();
        }
    }

    /// <summary>
    /// Chạy cutscene sự kiện lớn để cảnh báo người chơi
    /// </summary>
    private void CutSceneBigEvent()
    {
        Sequence sequence = DOTween.Sequence();

        //hiện cảnh báo và bật chặn vật cản
        sequence.AppendCallback(() =>
        {
            warning.SetActive(true);
            obstacleBlock.SetActive(true);
        });

        sequence.AppendInterval(2f); // Thời gian hiển thị cảnh báo

        // ẩn cảnh báo
        sequence.AppendCallback(() =>
        {
            warning.SetActive(false);
        });

        //Random sự kiện lớn
        sequence.AppendCallback(() =>
        {

            int randomIndex = Random.Range(0, bigEvents.childCount);
            Transform bigEvent = bigEvents.GetChild(randomIndex);
            bigEvent?.gameObject.SetActive(true);
        });

    }

    /// <summary>
    /// Kích hoạt sự kiện nhỏ (Bot đuổi theo, tên lửa,...) <br />
    /// Nếu đang có sự kiện lớn, không kích hoạt sự kiện nhỏ
    /// </summary>
    private void SmallEvent()
    {
        if (DistanceTracker.Instance.distanceTraveled >= nextSmallEventDistance)
        {
            nextSmallEventDistance += smallEventDistance;

            if (isBigEventActive)
                return;   // Nếu đang có sự kiện lớn, không kích hoạt sự kiện nhỏ

            int randomIndex = Random.Range(0, smallEvents.childCount);
            Transform smallEvent = smallEvents.GetChild(randomIndex);
            smallEvent.gameObject.SetActive(true);
        }
    }
}
