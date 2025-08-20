using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>
/// Khi cần load scene, ta dùng singleton gọi ReloadSceneWithLoading và truyền tham số true/false <br/>
/// False: nó sẽ bỏ qua việc gán callback Replay theo chế độ <br/>
/// True: nó sẽ gán callback Replay theo chế độ <br/>
/// Callback sẽ gán vào biến staticOnLoadingComplete <br/>
/// Sau khi xong sẽ chạy LoadScene để load lại scene <br/>
/// Start sẽ chạy để gọi hàm StartLoading <br/>
/// Hàm StartLoading sẽ kiểm tra biến staticOnLoadingComplete có gán callback Replay ở trên không <br/>
/// Nếu null thì sẽ gán callback mặc định, nếu khác null thì bỏ qua việc kiểm tra <br/>
/// Gọi coroutine để hiển thị loading
/// Khi hoàn tất sẽ gọi staticOnLoadingComplete và reset biến về null
/// </summary>
public class SceneLoader : Singleton<SceneLoader>
{
    private bool isLoading = false;
    private static bool shouldLoadOnStart = true; // Biến để kiểm tra có cần load khi start không
    private static System.Action staticOnLoadingComplete; // Dùng static để survive qua scene reload

    /// <summary>
    /// Khi vừa load scene sẽ gọi hàm này để chạy màn hình loading
    /// </summary>
    void Start()
    {
        // Kiểm tra nếu là lần đầu vào game hoặc được yêu cầu loading
        if (shouldLoadOnStart)
        {
            shouldLoadOnStart = false; // Reset lại
            StartLoading();
        }
    }

    /// <summary>
    /// Gán callback về Home vào staticOnLoadingComplete nếu nó bằng null, sau đó gọi LoadingCoroutine để loading
    /// </summary>
    public void StartLoading()
    {
        if (isLoading) return;

        //Nếu mới vô game hoặc load về home mà không gán onHidden thì sẽ gán callback mặc định
        if (staticOnLoadingComplete == null)
        {
            staticOnLoadingComplete = DefaultOnHidden; // Gán callback mặc định nếu không có
        }

        StartCoroutine(LoadingCoroutine());
    }

    /// <summary>
    /// khi loadscene thì dùng singleton để gọi hàm này
    /// isReplay: true nếu là replay thì gán hàm replay cho chế độ chơi normal/pvp
    /// isReplay: false nếu là load lại scene bình thường và hiển thị mainmenu
    /// </summary>
    public void ReloadSceneWithLoading(bool isReplay = false)
    {
        DOTween.KillAll();
        DOTween.Clear();
        if (isLoading) return;

        SaveManager.Instance.Save(); // Lưu dữ liệu trước khi reload

        // Lưu callback vào static variable
        if (isReplay)
        {
            switch (GameManager.Instance.gameMode)
            {
                case GameManager.GameMode.Normal:
                    staticOnLoadingComplete = ReplayGame;
                    break;
                case GameManager.GameMode.PVP:
                    staticOnLoadingComplete = PVPReplayGame;
                    break;
            }
        }


        // Đặt flag để khi scene load lại sẽ tự động chạy loading
        shouldLoadOnStart = true;

        // Reload scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Khi isReplay false sẽ gọi hàm này để bật main menu
    /// </summary>
    private void DefaultOnHidden()
    {

        Time.timeScale = 1; // Tiếp tục thời gian khi đóng Pause Panel
        UIManager.Instance.MainPanelGameobject.SetActive(true);
        // Phát SFX khi mở game
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("OpenGame");
        }

    }

    /// <summary>
    /// Khi isReplay true sẽ gọi hàm này để bắt đầu lại game ở chế độ normal
    /// </summary>
    private void ReplayGame()
    {
        // Dừng nhạc chủ đề và phát nhạc trong game
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic("InGame");
        }
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => StartGameCutScene.Instance.StartCutScene());
        Time.timeScale = 1; // Tiếp tục thời gian khi chơi lại
    }

    /// <summary>
    /// Khi isReplay true sẽ gọi hàm này để bắt đầu lại game ở chế độ pvp
    /// </summary>
    private void PVPReplayGame()
    {
        // Dừng nhạc chủ đề và phát nhạc trong game
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic("InGame");
        }
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() => StartGameCutScene.Instance.StartCutScenePVP());
        Time.timeScale = 1; // Tiếp tục thời gian khi chơi lại
    }

    /// <summary>
    /// Coroutine để bật UI và xử lý loading
    /// </summary>
    private IEnumerator LoadingCoroutine()
    {
        isLoading = true;

        // Hiển thị loading panel
        if (UIManager.Instance.loadingPanel != null)
            UIManager.Instance.loadingPanel.SetActive(true);

        // Random thời gian loading để tạo cảm giác thực tế
        float targetLoadingTime = Random.Range(UIManager.Instance.minLoadingTime, UIManager.Instance.maxLoadingTime);
        float currentTime = 0f;

        // Reset slider và text
        if (UIManager.Instance.loadingSlider != null)
            UIManager.Instance.loadingSlider.value = 0f;
        if (UIManager.Instance.percentageText != null)
            UIManager.Instance.percentageText.text = "0%";

        while (currentTime < targetLoadingTime)
        {
            currentTime += Time.deltaTime;

            // Tính toán progress dựa trên curve để tạo chuyển động mượt mà
            float normalizedTime = currentTime / targetLoadingTime;
            float progress = UIManager.Instance.loadingCurve.Evaluate(normalizedTime);

            // Cập nhật UI
            if (UIManager.Instance.loadingSlider != null)
                UIManager.Instance.loadingSlider.value = progress;

            if (UIManager.Instance.percentageText != null)
            {
                int percentage = Mathf.RoundToInt(progress * 100f);
                UIManager.Instance.percentageText.text = percentage + "%";
            }

            yield return null;
        }

        // Đảm bảo loading đạt 100%
        if (UIManager.Instance.loadingSlider != null)
            UIManager.Instance.loadingSlider.value = 1f;
        if (UIManager.Instance.percentageText != null)
            UIManager.Instance.percentageText.text = "100%";

        // Chờ một chút trước khi tắt loading
        yield return new WaitForSeconds(0.3f);

        // Tự động tắt loading panel
        if (UIManager.Instance.loadingPanel != null)
            UIManager.Instance.loadingPanel.SetActive(false);

        isLoading = false;

        //Gọi hàm bật mainmenu hoặc callback đã gán
        if (staticOnLoadingComplete != null)
        {
            staticOnLoadingComplete.Invoke();
            staticOnLoadingComplete = null; // Reset callback
        }

        SaveManager.Instance.Load();

    }
}

//Khi mới vô game, shouldLoadOnStart true nên nó sẽ loadscene và cho shouldLoadOnStart thành false
//Khi về home,nó sẽ gọi ReloadSceneWithLoading(false) để chạy DefaultOnHidden
//Khi replay, nó sẽ gọi ReloadSceneWithLoading(true) để chạy ReplayGame hoặc PVPReplayGame tuỳ chế độ chơi