using UnityEngine;

// =======================================================================
// Idêntico ao original. Troca o material entre "certo" e "errado"
// conforme a conexão. Vai no objeto que serve de feedback visual.
// =======================================================================
public class MatchFeedback : MonoBehaviour
{
    public Material matchMaterial;
    public Material misMatchMaterial;

    private Renderer _renderer;

    private void Start() => _renderer = GetComponent<Renderer>();

    public void ChangeMaterialWithMatch(bool isCorrectMatch)
    {
        _renderer.material = isCorrectMatch ? matchMaterial : misMatchMaterial;
    }
}
