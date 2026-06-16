using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors; // XRI 3.x. No XRI 2.x, APAGUE esta linha.

[RequireComponent(typeof(XRSocketInteractor))]
public class QuitSocket : MonoBehaviour
{
    [SerializeField] private ScreenFader fader;     // arraste o FadeQuad aqui
    [SerializeField] private float tempoFade = 1f;  // quanto tempo leva pra escurecer antes de fechar

    private XRSocketInteractor socket;
    private bool saindo;

    void Awake() => socket = GetComponent<XRSocketInteractor>();
    void OnEnable() => socket.selectEntered.AddListener(OnCartaoInserido);
    void OnDisable() => socket.selectEntered.RemoveListener(OnCartaoInserido);

    private void OnCartaoInserido(SelectEnterEventArgs args)
    {
        if (saindo) return;
        saindo = true;
        StartCoroutine(SairComFade());
    }

    private IEnumerator SairComFade()
    {
        // se você esqueceu de ligar o fader, fecha mesmo assim (sem escurecer)
        if (fader != null)
        {
            fader.FadeOut(tempoFade);
            yield return new WaitForSeconds(tempoFade);
        }

        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // pra testar no editor
        #endif
    }
}
