using System;
using UnityEngine;

/// <summary>
/// Guarda o estado do EPI (luva) e cuida da troca de material dos dois controles.
/// Coloque este script em um GameObject vazio na cena (ex: "EPI Manager").
///
/// HasGlove == true  -> equivale ao seu "have glove": 1
/// HasGlove == false -> equivale ao seu "have glove": 0
/// </summary>
public class GloveManager : MonoBehaviour
{
    // Acesso rápido de qualquer script: GloveManager.Instance.HasGlove
    public static GloveManager Instance { get; private set; }

    [Header("Estado inicial")]
    [SerializeField] private bool startWithGlove = false;

    [Header("Controles (Mesh Renderer do Controller_Base)")]
    [Tooltip("Mesh Renderer do Controller_Base do controle ESQUERDO")]
    [SerializeField] private MeshRenderer leftControllerRenderer;
    [Tooltip("Mesh Renderer do Controller_Base do controle DIREITO")]
    [SerializeField] private MeshRenderer rightControllerRenderer;

    [Header("Material da luva")]
    [Tooltip("Material azul que você criou")]
    [SerializeField] private Material gloveMaterial;
    [Tooltip("Índice do material no Mesh Renderer (Element 0 = 0)")]
    [SerializeField] private int materialIndex = 0;

    /// <summary>true = com luva (1), false = sem luva (0).</summary>
    public bool HasGlove { get; private set; }

    /// <summary>Disparado sempre que o estado muda (passa o novo valor).</summary>
    public event Action<bool> OnGloveStateChanged;

    // Guardamos os materiais originais pra conseguir "desequipar" depois, se quiser.
    private Material leftOriginalMaterial;
    private Material rightOriginalMaterial;

    private void Awake()
    {
        // Singleton simples de cena.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CacheOriginalMaterials();

        if (startWithGlove)
            EquipGlove();
        else
            HasGlove = false;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void CacheOriginalMaterials()
    {
        if (leftControllerRenderer != null &&
            materialIndex < leftControllerRenderer.sharedMaterials.Length)
            leftOriginalMaterial = leftControllerRenderer.sharedMaterials[materialIndex];

        if (rightControllerRenderer != null &&
            materialIndex < rightControllerRenderer.sharedMaterials.Length)
            rightOriginalMaterial = rightControllerRenderer.sharedMaterials[materialIndex];
    }

    /// <summary>Equipa a luva: liga o estado e pinta os dois controles de azul.</summary>
    public void EquipGlove()
    {
        HasGlove = true;
        SetMaterial(leftControllerRenderer, gloveMaterial);
        SetMaterial(rightControllerRenderer, gloveMaterial);
        OnGloveStateChanged?.Invoke(HasGlove);
    }

    /// <summary>Tira a luva: volta os materiais originais. (opcional, caso queira usar depois)</summary>
    public void RemoveGlove()
    {
        HasGlove = false;
        SetMaterial(leftControllerRenderer, leftOriginalMaterial);
        SetMaterial(rightControllerRenderer, rightOriginalMaterial);
        OnGloveStateChanged?.Invoke(HasGlove);
    }

    private void SetMaterial(MeshRenderer renderer, Material material)
    {
        if (renderer == null || material == null) return;

        // .materials (e NÃO .sharedMaterials): mexe só nesta instância em runtime
        // e não bagunça o asset original nem outros objetos que usem o mesmo material.
        Material[] mats = renderer.materials;
        if (materialIndex < 0 || materialIndex >= mats.Length)
        {
            Debug.LogWarning($"[GloveManager] materialIndex {materialIndex} fora do alcance em {renderer.name}.", renderer);
            return;
        }
        mats[materialIndex] = material;
        renderer.materials = mats;
    }
}
