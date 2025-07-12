// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class LoadingPanel : Singleton<LoadingPanel>
// {
//     public void Show()
//     {
//         gameObject.SetActive(true);
//         SetProgress(0f);
//     }

//     public void Hide()
//     {
//         gameObject.SetActive(false);
//     }

//     public void SetProgress(float progress)
//     {
//         float clamped = Mathf.Clamp01(progress);
//         if (UIManager.Instance.progressBar != null) UIManager.Instance.progressBar.value = clamped;
//         if (UIManager.Instance.percentText != null) UIManager.Instance.percentText.text = Mathf.RoundToInt(clamped * 100f) + "%";
//     }
// }