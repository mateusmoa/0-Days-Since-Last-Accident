using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors; // XRI 3.x. No XRI 2.x, APAGUE esta linha.

[RequireComponent(typeof(XRSocketInteractor))]
public class CheckpointSocket : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;   // ponto no chão na frente do relógio
    [SerializeField] private int diasPorEstaFase = 1;  // quantos dias este ponto adiciona (0 = não conta)

    private XRSocketInteractor socket;

    void Awake() => socket = GetComponent<XRSocketInteractor>();
    void OnEnable() => socket.selectEntered.AddListener(OnCartaoInserido);
    void OnDisable() => socket.selectEntered.RemoveListener(OnCartaoInserido);

    private void OnCartaoInserido(SelectEnterEventArgs args)
    {
        CheckpointManager.Instance.SetCheckpoint(respawnPoint);

        // passar de fase: só conta na PRIMEIRA vez que este ponto é batido
        if (ContadorDias.Instance != null)
            ContadorDias.Instance.PassarFase(this, diasPorEstaFase);

        // aqui dá pra tocar um "bip de ponto batido", acender luz verde, etc.
    }
}
