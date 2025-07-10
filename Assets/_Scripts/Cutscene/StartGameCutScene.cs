using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameCutScene : Singleton<StartGameCutScene>
{
    public Transform startingPoint;
    public Transform Map;

    private GameObject playerGameObject;
    private Transform playerPosition;
    private Rigidbody2D playerRigidbody;


    void Start()
    {
        GetComponent();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCutScene();
        }
    }
    private void GetComponent()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        playerPosition = playerGameObject.transform;
        playerRigidbody = playerGameObject.GetComponent<Rigidbody2D>();
    }

    public void StartCutScene()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            CutSceneBlocker.Instance.isCutSceneActive = true;
            MapSpawner.Instance.SetScrollSpeed(0);
            playerRigidbody.gravityScale = 0f;
        });
        //Di chuyển map sang trái
        sequence.Append(Map
                            .DOMove(new Vector3(-3f, 0, 0), 1.5f)
                            .SetEase(Ease.Linear));

        //chờ 0.5s rồi di chuyển camera lại gần
        sequence.AppendInterval(0.5f);

        sequence.Append(Camera.main.transform
                             .DOMove(new Vector3(-7, 0, -10), 1f));

        sequence.Join(
            DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 7f, 1f));

        //chờ 0.5s rồi di chuyển player đến startingPoint
        sequence.AppendInterval(0.5f);

        sequence.Append(playerPosition
                            .DOMove(startingPoint.position, 1.5f)
                            .SetEase(Ease.OutBack));

        // Di chuyển camera về vị trí ban đầu
        sequence.AppendInterval(0.5f);
        sequence.Append(Camera.main.transform
                       .DOMove(new Vector3(0, 0, -10), 1f));

        sequence.Join(
            DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 10f, 1f)
                    .SetEase(Ease.OutBack));

        sequence.OnComplete(() =>
        {
            MapSpawner.Instance.SetScrollSpeed(10);
            playerRigidbody.gravityScale = 7f;
            CutSceneBlocker.Instance.isCutSceneActive = false;
        });
    }
}
