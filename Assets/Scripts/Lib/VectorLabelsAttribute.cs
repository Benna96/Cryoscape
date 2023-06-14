// https://answers.unity.com/questions/1573537/how-to-change-the-names-of-a-vector-3-that-is-set.html

using UnityEngine;
public class VectorLabelsAttribute : PropertyAttribute
{
    public readonly string[] Labels;

    public VectorLabelsAttribute(params string[] labels)
    {
        Labels = labels;
    }
}