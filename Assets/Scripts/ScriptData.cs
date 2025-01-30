using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Script", menuName = "ScriptableObject/ScriptData")]
public class ScriptData : ScriptableObject
{
    [Header("Ʃ�丮�� ���")]
    public ScriptText[] scriptTexts;
}
[System.Serializable]
public class ScriptText
{
    [TextArea(2,5)]
    public string text;
}