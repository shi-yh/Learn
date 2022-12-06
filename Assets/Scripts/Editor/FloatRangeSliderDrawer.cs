


using ObjectManagement;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRangeSlierAttribute))]
public class FloatRangeSliderDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        int originalIndentLevel = EditorGUI.indentLevel;
        EditorGUI.BeginProperty(position,label,property);

        SerializedProperty minProperty = property.FindPropertyRelative("min");
        SerializedProperty maxProperty = property.FindPropertyRelative("max");
        
        float minValue = minProperty.floatValue;
        float maxValue = maxProperty.floatValue;


        float fieldWidth = position.width / 4 - 4f;
        float sliderWidth = position.width / 2;

        position.width = fieldWidth ;
        minValue = EditorGUI.FloatField(position,minValue);
       
        position.x += fieldWidth+4;
        position.width = sliderWidth;
        FloatRangeSlierAttribute limit = attribute as FloatRangeSlierAttribute;
        EditorGUI.MinMaxSlider(position,ref minValue,ref maxValue,limit.Min,limit.Max);
        
        position.x += sliderWidth+4f;
        position.width = fieldWidth;
        maxValue = EditorGUI.FloatField(position,maxValue);


        if (minValue<limit.Min)
        {
            minValue = limit.Min;
        }
        else if (minValue > limit.Max)
        {
            minValue = limit.Max;
        }
        else if(maxValue<minValue)
        {
            maxValue = minValue;
        }
        else if(maxValue>limit.Max)
        {
            maxValue = limit.Max;
        }
        
        
        minProperty.floatValue = minValue;
        maxProperty.floatValue = maxValue;
        
        
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = originalIndentLevel;



    }
}