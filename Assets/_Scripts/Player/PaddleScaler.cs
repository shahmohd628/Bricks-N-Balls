using System.Collections;
using UnityEngine;

public class PaddleScaler : MonoBehaviour
{
    private Vector3 originalScale;
    private Coroutine scaleRoutine;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void GrowFor(float duration) => ApplyScale(0.2f, duration);
    public void ShrinkFor(float duration) => ApplyScale(0.1f, duration);

    private void ApplyScale(float multiplier, float duration)
    {
        if (scaleRoutine != null) StopCoroutine(scaleRoutine); // restarting = duration reset
        scaleRoutine = StartCoroutine(ScaleRoutine(multiplier, duration));
    }

    private IEnumerator ScaleRoutine(float multiplier, float duration)
    {
        transform.localScale = originalScale * multiplier;
        yield return new WaitForSeconds(duration);
        transform.localScale = originalScale;
        scaleRoutine = null;
    }
}