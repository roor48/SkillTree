using UnityEngine;

[CreateAssetMenu(fileName = "ElementData", menuName = "SO/ElementData")]
public class ElementData : ScriptableObject
{
    public int id;
    public BigNumber[] costs;
    public int[] childrenIds;
}
