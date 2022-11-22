using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Unity.VisualScripting.FullSerializer;

[ExecuteAlways]
public class MultiLanguageText : MonoBehaviour
{
    public TextLanguage.Type language;
    public TextMeshProUGUI text;
    public TextLanguage[] b;

    int currLanIndex = 0;

    void Start()
    {
        print("hit");
        
    }    
}

[ExecuteAlways]
[System.Serializable]
public class TextLanguage
{
    public enum Type { a1, a2, a3 };
    public Type type;

    [SerializeField]
    private string _text;
    public string text
    {
        get { return _text; }
        set
        {
            _text = value;
            Debug.Log(value);
        }
    }

    public bool use;
}
