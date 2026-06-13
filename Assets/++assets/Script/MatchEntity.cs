using UnityEngine;

// =======================================================================
// Mesma ideia da MatchEntity original: é a "ponte" que liga a ponta móvel
// (connector), a cor do encaixe, o feedback visual e o manager.
//
// Quase idêntica ao original — só trocamos MovablePair -> WireConnector e
// o nome do método de interação (que agora é chamado pelo WireSocket).
// =======================================================================
public class MatchEntity : MonoBehaviour
{
    public MatchFeedback feedback;
    public WireConnector connector;          // era _movablePair
    public Renderer socketColorRenderer;     // era _fixedPairRenderer (a cor-alvo no encaixe)
    public MatchSystemManager matchSystemManager;

    private bool _matched;

    public Vector3 GetConnectorHome() => connector.GetHomePosition();

    public void SetConnectorHome(Vector3 newHome) => connector.SetHomePosition(newHome);

    public void SetColor(Material pairMaterial)
    {
        connector.GetComponent<Renderer>().material = pairMaterial;
        socketColorRenderer.material = pairMaterial;
    }

    // Chamado pelo WireSocket quando ALGUMA ponta entra/sai do encaixe.
    // (mesma lógica do PairObjectInteraction original)
    public void OnSocketInteraction(bool entered, WireConnector other)
    {
        if (entered && !_matched)
        {
            _matched = (other == connector); // só conta se for a ponta CERTA (mesma cor)

            if (_matched)
            {
                matchSystemManager.NewMatchRecord(true);
                feedback.ChangeMaterialWithMatch(true);
            }
        }
        else if (!entered && _matched)
        {
            _matched = !(other == connector);

            if (!_matched)
            {
                matchSystemManager.NewMatchRecord(false);
                feedback.ChangeMaterialWithMatch(false);
            }
        }
    }
}
