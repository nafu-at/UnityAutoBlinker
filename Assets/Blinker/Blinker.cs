using System.Collections;
using UnityEngine;

/// <summary>
///     BlendShapeを用いてランダムに瞬きをさせるためのスクリプトです。
/// </summary>
public class Blinker : MonoBehaviour
{
    [SerializeField] public SkinnedMeshRenderer skinnedMeshRenderer;

    [SerializeField] public int leftEyeBlinkIndex;
    [SerializeField] public int rightEyeBlinkIndex;

    [SerializeField] public float interval = 5f;
    [SerializeField] public float closingTime = 0.06f;
    [SerializeField] public float openingTime = 0.03f;
    [SerializeField] public float closeTime = 0.1f;

    private Coroutine _coroutine;
    private void OnEnable()
    {
        _coroutine = StartCoroutine(Blink());
    }
    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }


    private IEnumerator Blink()
    {
        while (true)
        {
            var waitTime = Time.time + Random.value * interval;
            yield return new WaitForSeconds(waitTime - Time.time);

            // Close
            var value = 0f;
            var closeSpeed = closeTime / 100f; // ブレンドシェイプの値を１操作するために必要な時間
            while (true)
            {
                value += Time.deltaTime / closeSpeed;
                if (value >= 1f)
                    break;

                if (leftEyeBlinkIndex > -1)
                    skinnedMeshRenderer.SetBlendShapeWeight(leftEyeBlinkIndex, value);
                if (rightEyeBlinkIndex > -1)
                    skinnedMeshRenderer.SetBlendShapeWeight(rightEyeBlinkIndex, value);

                yield return null;
            }
            if (leftEyeBlinkIndex > -1)
                skinnedMeshRenderer.SetBlendShapeWeight(leftEyeBlinkIndex, 100f);
            if (rightEyeBlinkIndex > -1)
                skinnedMeshRenderer.SetBlendShapeWeight(rightEyeBlinkIndex, 100f);

            // Wait
            yield return new WaitForSeconds(closingTime);

            // Open
            value = 100f;
            var openSpeed = openingTime / 100f; // ブレンドシェイプの値を１操作するために必要な時間
            while (true)
            {
                value -= Time.deltaTime / openSpeed;
                if (value < 0)
                    break;

                if (leftEyeBlinkIndex > -1)
                    skinnedMeshRenderer.SetBlendShapeWeight(leftEyeBlinkIndex, value);
                if (rightEyeBlinkIndex > -1)
                    skinnedMeshRenderer.SetBlendShapeWeight(rightEyeBlinkIndex, value);

                yield return null;
            }
            if (leftEyeBlinkIndex > -1)
                skinnedMeshRenderer.SetBlendShapeWeight(leftEyeBlinkIndex, 0f);
            if (rightEyeBlinkIndex > -1)
                skinnedMeshRenderer.SetBlendShapeWeight(rightEyeBlinkIndex, 0f);
        }
    }
}
