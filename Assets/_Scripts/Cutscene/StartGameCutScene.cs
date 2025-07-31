using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartGameCutScene : Singleton<StartGameCutScene>
{
    public List<Vector3> playersOffset = new List<Vector3>();
    public Transform startingPoint;
    public Transform Map;

    private List<GameObject> playerGameObject = new List<GameObject>();
    private List<Transform> playerPosition = new List<Transform>();
    private List<Rigidbody2D> playerRigidbody = new List<Rigidbody2D>();
    private BackToFlightPoint backToFlightPoint;

    void Start()
    {
        GetComponent();
    }
    private void GetComponent()
    {
        // 1. Tìm tất cả Player
        playerGameObject = GameObject.FindGameObjectsWithTag("Player").OrderBy(x => x.name).ToList();

        // 2. Xóa dữ liệu cũ
        playerPosition.Clear();
        playerRigidbody.Clear();

        // 3. Lặp từng player để lấy Transform và Rigidbody2D
        foreach (var player in playerGameObject)
        {
            playerPosition.Add(player.transform);
            playerRigidbody.Add(player.GetComponent<Rigidbody2D>());
        }

        foreach (var player in playerGameObject)
        {
            if (player.name != "Player 1")
            {
                player.SetActive(false); // Tắt các player khác ngoài Player 1
            }
        }
        // 4. Lấy BackToFlightPoint từ một object cụ thể
        backToFlightPoint = GameObject.Find("StartingPoint").GetComponent<BackToFlightPoint>();
    }

    public void StartCutScene()
    {
        if (playerGameObject == null || playerPosition == null || playerRigidbody == null)
        {
            GetComponent();
        }
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            CutSceneBlocker.Instance.isCutSceneActive = true;
            MapSpawner.Instance.SetScrollSpeed(0);
            playerRigidbody[0].gravityScale = 0f;
            backToFlightPoint.enabled = false; // Tắt BackToFlightPoint để không ảnh hưởng đến cutscene

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

        sequence.Append(playerPosition[0]
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
            playerRigidbody[0].gravityScale = 7f;
            CutSceneBlocker.Instance.isCutSceneActive = false;
            UIManager.Instance.InGamePanelGameobject.SetActive(true);

            DistanceTracker.Instance.ResetDistance();
            GameManager.Instance.coinIngame = 0; // Reset coin count at the start of the game
            DistanceTracker.Instance.isStopped = false; // Dừng distance tracking
            backToFlightPoint.enabled = true; // Bật lại BackToFlightPoint sau khi cutscene kết thúc
        });
    }


    public void StartCutScenePVP()
    {
        if (playerGameObject == null || playerPosition == null || playerRigidbody == null)
        {
            GetComponent();
        }
        Sequence sequence = DOTween.Sequence();


        sequence.AppendCallback(() =>
        {
            foreach (var player in playerGameObject)
            {
                player.SetActive(true);
            }
            CutSceneBlocker.Instance.isCutSceneActive = true;
            MapSpawner.Instance.SetScrollSpeed(0);

            foreach (var player in playerRigidbody)
            {
                player.gravityScale = 0f;
            }

            backToFlightPoint.enabled = false; // Tắt BackToFlightPoint để không ảnh hưởng đến cutscene

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

        for (int i = 0; i < playerGameObject.Count; i++)
        {
            // Di chuyển từng player đến startingPoint
            sequence.Append(playerGameObject[i].transform.DOMove(startingPoint.position + playersOffset[i], 1.5f)
                                                        .SetEase(Ease.OutBack));
        }

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
            foreach (var player in playerRigidbody)
            {
                player.gravityScale = 7f;
            }
            CutSceneBlocker.Instance.isCutSceneActive = false;
            UIManager.Instance.InGamePanelGameobject.SetActive(true);

            DistanceTracker.Instance.ResetDistance();
            GameManager.Instance.coinIngame = 0; // Reset coin count at the start of the game
            DistanceTracker.Instance.isStopped = false; // Dừng distance tracking
            backToFlightPoint.enabled = true; // Bật lại BackToFlightPoint sau khi cutscene kết thúc
        });
    }

}
