#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_CardList))]
public class SO_CardListEditor : Editor
{
    private SO_Card newCard;

    void OnEnable()
    {
        newCard = CreateInstance<SO_Card>();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        newCard.title = EditorGUILayout.TextField("Card Name", newCard.title);
        newCard.description = EditorGUILayout.TextField("Card Description", newCard.description);
        newCard.startingHealth = EditorGUILayout.IntField("Starting Health", newCard.startingHealth);
        newCard.attack = EditorGUILayout.IntField("Attack", newCard.attack);
        newCard.cost = EditorGUILayout.IntField("Cost", newCard.cost);
        newCard.power = EditorGUILayout.IntField("Power", newCard.power);
        newCard.imageBackground = (Sprite)EditorGUILayout.ObjectField("Background Image", newCard.imageBackground, typeof(Sprite), false);
        newCard.image = (Sprite)EditorGUILayout.ObjectField("Image", newCard.image, typeof(Sprite), false);

        SO_CardList myScript = (SO_CardList)target;
        if(GUILayout.Button("Add New Card"))
        {
            newCard.id = myScript.Cards.Count + 10001;
            myScript.AddCard(newCard);
            AssetDatabase.CreateAsset(newCard, "Assets/Scriptable Objects/Cards/Card" + newCard.id + ".asset");
            AssetDatabase.SaveAssets();
            newCard = CreateInstance<SO_Card>(); // Create a new instance for the next card
        }
    }
}
#endif