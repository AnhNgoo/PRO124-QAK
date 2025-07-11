using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private bool isLoading = false;
    private static bool isFirstLoad = true; // Kiểm tra lần đầu vào game
    private static bool shouldLoadOnStart = false; // Biến để kiểm tra có cần load khi start không
    private static System.Action staticOnLoadingComplete; // Dùng static để survive qua scene reload

    void Start()
    {
        // Kiểm tra nếu là lần đầu vào game hoặc được yêu cầu loading
        if (isFirstLoad || shouldLoadOnStart)
        {
            isFirstLoad = false;
            shouldLoadOnStart = false; // Reset lại
            StartLoading();
        }
    }

    /// <summary>
    /// Hàm này để gán vào button hoặc gọi từ bất cứ đâu
    /// </summary>
    public void StartLoading()
    {
        if (isLoading) return;

        StartCoroutine(LoadingCoroutine());
    }

    /// <summary>
    /// Hàm reload scene với loading (có thể gán vào nút Home)
    /// </summary>
    public void ReloadSceneWithLoading(System.Action onHidden = null)
    {
        if (isLoading) return;

        // Lưu callback vào static variable
        staticOnLoadingComplete = onHidden;

        // Đặt flag để khi scene load lại sẽ tự động chạy loading
        shouldLoadOnStart = true;

        // Reload scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

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

        // Gọi static callback và reset nó
        if (staticOnLoadingComplete != null)
        {
            staticOnLoadingComplete.Invoke();
            staticOnLoadingComplete = null; // Reset callback
        }
    }

    /// <summary>
    /// Kiểm tra có đang loading không
    /// </summary>
    public bool IsLoading()
    {
        return isLoading;
    }
}