using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractMapGenerator),true)]
public class RandomMapGeneratorEditor : Editor
{
    AbstractMapGenerator m_Generator;

    private void Awake()
    {
        m_Generator = (AbstractMapGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Create Dungeon"))
        {
            m_Generator.GeneratorMap();
        }
        if(GUILayout.Button("Clear Dungeon"))
        {
            m_Generator.ClearMap();
        }
    }
}
