using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

// =======================================================================
// O "cérebro": sorteia as cores, embaralha as posições iniciais das pontas
// e conta quantos pares foram conectados corretamente.
//
// Praticamente idêntico ao original. Coloque este script num GameObject
// PAI, e todas as MatchEntity como FILHAS dele.
// =======================================================================
public class MatchSystemManager : MonoBehaviour
{
    public List<Material> colorMaterials;

    [Tooltip("Se ligado, embaralha as posições de origem das pontas (vira quebra-cabeça).")]
    public bool randomizeStartPositions = true;

    private List<MatchEntity> _matchEntities;
    private int _targetMatchCount;
    private int _currentMatchCount = 0;

    private void Start()
    {
        _matchEntities = GetComponentsInChildren<MatchEntity>().ToList();
        _targetMatchCount = _matchEntities.Count;

        SetEntityColors();

        if (randomizeStartPositions)
            RandomizeConnectorPlacement();
    }

    private void SetEntityColors()
    {
        Shuffle(colorMaterials);

        for (int i = 0; i < _matchEntities.Count; i++)
            _matchEntities[i].SetColor(colorMaterials[i]);
    }

    private void RandomizeConnectorPlacement()
    {
        var homes = new List<Vector3>();

        for (int i = 0; i < _matchEntities.Count; i++)
            homes.Add(_matchEntities[i].GetConnectorHome());

        Shuffle(homes);

        for (int i = 0; i < _matchEntities.Count; i++)
            _matchEntities[i].SetConnectorHome(homes[i]);
    }

    public void NewMatchRecord(bool matchConnected)
    {
        _currentMatchCount += matchConnected ? 1 : -1;

        Debug.Log($"Conexões corretas: {_currentMatchCount}/{_targetMatchCount}");

        if (_currentMatchCount == _targetMatchCount)
            Debug.Log("MANDOU BEM! TODOS OS FIOS CONECTADOS!");
    }

    // Fisher-Yates. (Pequena correção vs. o original: Range(0, n + 1) inclui o
    // próprio índice n, deixando o embaralhamento sem viés.)
    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
