using UnityEngine;

/// <summary>
/// Spawna um prefab num ponto configurável.
/// Ligue o método Spawn() no evento OnClick() de um botão de UI.
/// </summary>
public class ButtonItemSpawner : MonoBehaviour
{
    [Header("O que spawnar")]
    [Tooltip("Prefab do item que será criado ao clicar no botão.")]
    public GameObject itemPrefab;

    [Header("Onde spawnar")]
    [Tooltip("Arraste um GameObject vazio (Empty) para marcar o local. " +
             "Se ficar vazio, usa a posição deste objeto.")]
    public Transform spawnPoint;

    [Tooltip("Ajuste fino a partir do ponto de spawn, em metros.")]
    public Vector3 spawnOffset = Vector3.zero;

    [Header("Opções")]
    [Tooltip("Destrói o item anterior antes de criar um novo?")]
    public bool destroyPrevious = true;

    private GameObject _lastSpawned;

    /// <summary>Ligue este método no OnClick() do botão.</summary>
    public void Spawn()
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning("[ButtonItemSpawner] Nenhum prefab atribuído no Inspector.");
            return;
        }

        if (destroyPrevious && _lastSpawned != null)
            Destroy(_lastSpawned);

        Transform origin = spawnPoint != null ? spawnPoint : transform;
        Vector3 pos      = origin.position + spawnOffset;
        Quaternion rot   = origin.rotation;

        _lastSpawned = Instantiate(itemPrefab, pos, rot);
    }

    /// <summary>Remove o último item spawnado (opcional).</summary>
    public void RemoveLast()
    {
        if (_lastSpawned != null)
            Destroy(_lastSpawned);
    }

    // Mostra no Editor onde o item vai aparecer
    void OnDrawGizmosSelected()
    {
        Transform origin = spawnPoint != null ? spawnPoint : transform;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin.position + spawnOffset, 0.05f);
    }
}
