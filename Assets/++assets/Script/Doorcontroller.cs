using System.Collections;
using UnityEngine;

// =======================================================================
// Porta que GIRA pra abrir (dobradiça). Vai no PIVÔ (o vazio na beirada),
// não no mesh da porta. Chame Open() — ex.: no Select Entered da checkbox.
//
// Como gira o próprio pivô, e a porta é filha dele, ela abre pela beirada.
// =======================================================================
public class DoorController : MonoBehaviour
{
    [Tooltip("Quantos graus a porta gira ao abrir. Use negativo pra abrir pro outro lado.")]
    public float openAngle = 90f;
    [Tooltip("Eixo da dobradiça. Y (0,1,0) = porta normal em pé.")]
    public Vector3 hingeAxis = Vector3.up;
    public float openDuration = 1f;

    [Header("Som (opcional)")]
    public AudioSource audioSource;
    public AudioClip openSound;

    private Quaternion _closedRot;
    private Quaternion _openRot;
    private Coroutine _moving;

    private void Awake()
    {
        _closedRot = transform.localRotation;
        _openRot = _closedRot * Quaternion.AngleAxis(openAngle, hingeAxis);
    }

    public void Open()
    {
        if (audioSource != null && openSound != null) audioSource.PlayOneShot(openSound);
        if (_moving != null) StopCoroutine(_moving);
        _moving = StartCoroutine(RotateTo(_openRot));
    }

    public void Close()
    {
        if (_moving != null) StopCoroutine(_moving);
        _moving = StartCoroutine(RotateTo(_closedRot));
    }

    private IEnumerator RotateTo(Quaternion target)
    {
        Quaternion start = transform.localRotation;
        float t = 0f;
        while (t < openDuration)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(start, target, t / openDuration);
            yield return null;
        }
        transform.localRotation = target;
    }
}
