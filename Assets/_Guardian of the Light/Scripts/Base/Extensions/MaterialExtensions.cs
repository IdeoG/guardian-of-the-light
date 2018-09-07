using UnityEngine;

public static class MaterialExtensions
{
    public static void ReduceEmissionColorAlpha(this Material mat, float deltaAlpha)
    {
        var color = mat.GetColor("_EmissionColor");
        color -= color * Mathf.LinearToGammaSpace (deltaAlpha);
        mat.SetColor("_EmissionColor", color);
    }
    
    public static void EnhanceEmissionColorAlpha(this Material mat, float deltaAlpha)
    {
        var color = mat.GetColor("_EmissionColor");
        color += color * Mathf.LinearToGammaSpace (deltaAlpha);
        mat.SetColor("_EmissionColor", color);
    }
}