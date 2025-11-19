using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIMainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button practiceButton;
    [SerializeField] private Button evaluationButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    [Header("Panels")]
    [SerializeField] private CanvasGroup machineSelectorCanvasGroup;
    [SerializeField] private UIAnimationHelper animator;

    private void Awake()
    {
        practiceButton.onClick.AddListener(() => OnModeSelected(PlayMode.Practice));
        evaluationButton.onClick.AddListener(() => OnModeSelected(PlayMode.Evaluation));
        creditsButton.onClick.AddListener(OnCredits);
        exitButton.onClick.AddListener(OnExit);

        machineSelectorCanvasGroup.alpha = 0f;
        machineSelectorCanvasGroup.interactable = false;
        machineSelectorCanvasGroup.blocksRaycasts = false;
    }

    private void OnModeSelected(PlayMode mode)
    {
        MachineSelectionManager.Instance.SetMode(mode);
        // show selector
        animator.FadeCanvasGroup(machineSelectorCanvasGroup, 1f).OnComplete(() =>
        {
            machineSelectorCanvasGroup.interactable = true;
            machineSelectorCanvasGroup.blocksRaycasts = true;
        });
    }

    private void OnCredits()
    {
        // Implement credits popup or scene
        Debug.Log("Credits pressed");
    }

    private void OnExit()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        practiceButton.onClick.RemoveAllListeners();
        evaluationButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }
}
