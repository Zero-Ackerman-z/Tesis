using UnityEngine;
using DG.Tweening;

public class UIAnimationHelper : MonoBehaviour
{
    public float defaultScaleDuration = 0.35f;
    public float defaultFadeDuration = 0.25f;

    // Scale up with punch-like effect
    public Tween ScaleUp(Transform target, float toScale = 1.15f, float duration = -1f)
    {
        if (duration <= 0) duration = defaultScaleDuration;
        return target.DOScale(toScale, duration).SetEase(Ease.OutBack);
    }

    public Tween ScaleDown(Transform target, float toScale = 1f, float duration = -1f)
    {
        if (duration <= 0) duration = defaultScaleDuration;
        return target.DOScale(toScale, duration).SetEase(Ease.OutBack);
    }

    public Tween FadeCanvasGroup(CanvasGroup cg, float to, float duration = -1f)
    {
        if (duration <= 0) duration = defaultFadeDuration;
        return cg.DOFade(to, duration).SetEase(Ease.Linear);
    }

    public Tween MoveLocalY(Transform t, float by, float duration = 0.3f)
    {
        return t.DOLocalMoveY(t.localPosition.y + by, duration).SetEase(Ease.OutCubic);
    }
}
