using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Troca de cena. Coloque em um objeto qualquer da cena de menu
/// (ex: um GameObject vazio chamado "GameManager") e ligue os botoes
/// a estes metodos pelo evento OnClick do botao.
/// </summary>
public class TrocarCena : MonoBehaviour
{
    // ---- Opcao A: nome digitado direto no proprio botao ----
    // Recomendada quando voce tem varios botoes, cada um indo pra uma cena.
    // No OnClick escolha "TrocarCena -> Carregar (string)" e digite o nome da cena.
    public void Carregar(string nomeDaCena)
    {
        if (string.IsNullOrWhiteSpace(nomeDaCena))
        {
            Debug.LogError("TrocarCena: nome da cena vazio.");
            return;
        }
        SceneManager.LoadScene(nomeDaCena);
    }

    // ---- Opcao B: nome configurado neste componente ----
    // Util quando o objeto so leva pra uma cena fixa.
    [Tooltip("Nome EXATO da cena de destino. Ela precisa estar em File > Build Settings.")]
    public string cenaPadrao;

    public void Carregar()
    {
        Carregar(cenaPadrao);
    }

    // Ligue ao botao "Sair".
    public void Sair()
    {
        Debug.Log("Saindo do jogo (so funciona no build final, nao dentro do Editor).");
        Application.Quit();
    }
}
