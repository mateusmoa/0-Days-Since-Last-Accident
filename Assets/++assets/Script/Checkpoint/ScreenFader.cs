using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Renderer fadeQuad; // o quad preto

    public void FadeOut(float dur) => StartCoroutine(Fade(1f, dur));
    public IEnumerator FadeIn(float dur) => Fade(0f, dur);

    private IEnumerator Fade(float targetAlpha, float dur)
    {
        Material mat = fadeQuad.material;
        Color c = mat.color;
        float startAlpha = c.a;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t / dur);
            mat.color = c;
            yield return null;
        }
        c.a = targetAlpha;
        mat.color = c;
    }
}
