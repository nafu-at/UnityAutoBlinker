using System.Collections;
using UnityEngine;

namespace NafuSoft.AutoBlinker
{
    /// <summary>
    ///     BlendShapeを用いてランダムに瞬きをさせるためのスクリプトです。
    /// </summary>
    public class AutoBlinker : MonoBehaviour
    {
        [SerializeField] public SkinnedMeshRenderer skinnedMeshRenderer;

        [SerializeField] public int leftEyeBlinkIndex;
        [SerializeField] public int rightEyeBlinkIndex;

        [SerializeField] public float interval = 5f;
        [SerializeField] public float closeTime = 0.06f;
        [SerializeField] public float closingTime = 0.1f;
        [SerializeField] public float openingTime = 0.03f;

        private Coroutine _coroutine;
        private void OnEnable()
        {
            if (skinnedMeshRenderer == null)
            {
                Debug.LogWarning("Skinned Mesh Rendererが正しく設定されていません", gameObject);
                return;
            }
            _coroutine = StartCoroutine(Blink());
        }
        private void OnDisable()
        {
            if (_coroutine != null)
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
                var closeSpeed = closingTime / 100f; // ブレンドシェイプの値を１操作するために必要な時間

                while (true)
                {
                    value += Time.deltaTime / closeSpeed;
                    if (value >= 100f)
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
                yield return new WaitForSeconds(closeTime);

                // Open
                value = 100f;
                var openSpeed = openingTime / 100f; // ブレンドシェイプの値を１操作するために必要な時間
                while (true)
                {
                    value -= Time.deltaTime / openSpeed;
                    if (value < 0f)
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
}
