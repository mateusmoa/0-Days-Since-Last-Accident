using System.Collections;
using UnityEngine;
using Unity.XR.CoreUtils; // XROrigin

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private PlacaSpawner spawner;
    [SerializeField] private ScreenFader fader;   // seu fade pra preto (quad preto na câmera)
    [SerializeField] private Transform rigRoot;   // root que tomba (pode ser o próprio XROrigin)

    [Header("Conforto / Queda")]
    [SerializeField] private bool usarQueda = true;
    [SerializeField] private float tempoQueda = 0.8f;
    [SerializeField] private float anguloQueda = 80f;

    private bool morrendo;

    // chame do seu Wire Task quando levar choque
    public void Morrer()
    {
        if (morrendo) return;
        morrendo = true;
        StartCoroutine(SequenciaMorte());
    }

    private IEnumerator SequenciaMorte()
    {
        // 1. trava locomoção/input aqui (desabilita seus providers)

        // 2. queda + fade ao MESMO tempo (o fade esconde o tombo = menos enjoo)
        Quaternion rotIni = rigRoot.rotation; // guarda a rotação atual (inclui pra onde você virou)
        Quaternion rotFim = rotIni * Quaternion.Euler(anguloQueda, 0, 0);
        Vector3 posIni = rigRoot.position;
        Vector3 posFim = posIni + Vector3.down * 1.2f;

        fader.FadeOut(tempoQueda); // dispara o fade em paralelo

        if (usarQueda)
        {
            float t = 0;
            while (t < tempoQueda)
            {
                t += Time.deltaTime;
                float k = t / tempoQueda;
                float eased = k * k; // ease-in = sensação de gravidade
                rigRoot.rotation = Quaternion.Slerp(rotIni, rotFim, eased);
                rigRoot.position = Vector3.Lerp(posIni, posFim, eased);
                yield return null;
            }
        }
        else yield return new WaitForSeconds(tempoQueda);

        // 3. tela preta: desfaz SÓ o tombo (mantém o yaw) e teleporta
        rigRoot.rotation = rotIni; // volta exatamente pra rotação original, sem zerar pra onde você virou
        Respawnar();
        spawner.SpawnNext(); // avança pra próxima placa da sequência

        // 4. volta
        yield return new WaitForSeconds(0.3f);
        yield return fader.FadeIn(0.4f);

        // 5. reabilita input
        morrendo = false;
    }

    private void Respawnar()
    {
        var cp = CheckpointManager.Instance;
        // teleporta o rig compensando o offset da câmera dentro dele
        Vector3 offset = xrOrigin.Camera.transform.position - xrOrigin.transform.position;
        offset.y = 0;
        xrOrigin.transform.position = cp.Position - offset;
    }
}
