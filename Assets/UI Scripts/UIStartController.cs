using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class UIStartController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button startButton;
    [SerializeField] private CanvasGroup panelOptionsCanvasGroup;
    [SerializeField] private UIAnimationHelper animator;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartClicked);
        panelOptionsCanvasGroup.alpha = 0f;
        panelOptionsCanvasGroup.interactable = false;
        panelOptionsCanvasGroup.blocksRaycasts = false;
    }

    private void OnStartClicked()
    {
        // animate out start button (scale & fade)
        startButton.transform.DOScale(0.8f, 0.25f).SetEase(Ease.InBack);
        startButton.GetComponent<CanvasGroup>()?.DOFade(0f, 0.25f);

        // show main options
        animator.FadeCanvasGroup(panelOptionsCanvasGroup, 1f).OnComplete(() =>
        {
            panelOptionsCanvasGroup.interactable = true;
            panelOptionsCanvasGroup.blocksRaycasts = true;
        });
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnStartClicked);
    }
}


