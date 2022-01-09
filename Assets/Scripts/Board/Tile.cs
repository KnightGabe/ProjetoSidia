using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridIndex;
    private Material myMaterial;
    public Actor occupant;

    private void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        myMaterial.EnableKeyword("_EMISSION");
    }

    private void OnMouseDown()
    {
        GridManager.Instance.TileClicked(this);
    }

    public void Highlight(Color color)
    {
        myMaterial.SetColor("_EmissionColor", color * 2);
    }

    public void Dim()
    {
        myMaterial.SetColor("_EmissionColor", Color.black);
    }
}
