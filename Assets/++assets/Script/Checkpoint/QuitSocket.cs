using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors; // XRI 3.x. No XRI 2.x, APAGUE esta linha.

[RequireComponent(typeof(XRSocketInteractor))]
public class QuitSocket : MonoBehaviour
{
    private XRSocketInteractor socket;

    void Awake() => socket = GetComponent<XRSocketInteractor>();
    void OnEnable() => socket.selectEntered.AddListener(OnCartaoInserido);
    void OnDisable() => socket.selectEntered.RemoveListener(OnCartaoInserido);

    private void OnCartaoInserido(SelectEnterEventArgs args)
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // pra testar no editor
        #endif
    }
}
