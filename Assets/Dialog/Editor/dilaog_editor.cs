using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(dialog))]
public class dilaog_editor : Editor
{
    dialog Dialog = null;
    List<bool> show_dialog = new List<bool>();

    void OnEnable()
    {
        Dialog = (dialog)target;
        for (int i = 0; i < Dialog.dialog_cycles.Count; i++)
        {
            show_dialog.Add(false);
        }

    }

    public override void OnInspectorGUI()
    {
        Dialog.delay = EditorGUILayout.FloatField("출력 딜레이", Dialog.delay);

        for (int i = 0; i < Dialog.dialog_cycles.Count; i++)
        {
            show_dialog[i] = EditorGUILayout.Foldout(show_dialog[i], Dialog.dialog_cycles[i].cycle_index.ToString());
            
            
            if (show_dialog[i])
            {
                EditorGUILayout.LabelField(Dialog.dialog_cycles[i].cycle_name);
                EditorGUI.indentLevel += 1;
                EditorGUILayout.LabelField("Cycle name");
                
                Dialog.dialog_cycles[i].cycle_name = EditorGUILayout.TextArea(Dialog.dialog_cycles[i].cycle_name);
                EditorGUILayout.Space();
                for (int j = 0; j < Dialog.dialog_cycles[i].info.Count; j++)
                {

                    EditorGUILayout.LabelField("name");
                    Dialog.dialog_cycles[i].info[j].name = EditorGUILayout.TextArea(Dialog.dialog_cycles[i].info[j].name, GUILayout.MinWidth(20));
                    EditorGUILayout.LabelField("지문");
                    Dialog.dialog_cycles[i].info[j].content = EditorGUILayout.TextArea(Dialog.dialog_cycles[i].info[j].content, GUILayout.MinWidth(60), GUILayout.MinHeight(30));

                    EditorGUILayout.Space();

                }

                if (GUILayout.Button("지문 추가"))
                {
                    dialog_info de = new dialog_info();
                    Dialog.dialog_cycles[i].info.Add(de);
                }
                if (GUILayout.Button("삭제"))
                {
                    dialog_info de = new dialog_info();

                    Dialog.dialog_cycles[i].info.RemoveAt(Dialog.dialog_cycles[i].info.Count - 1);
                }

                if (GUILayout.Button("해당 위치 다이얼로그 추가"))
                {
                    Dialog_cycle d = new Dialog_cycle();
                    for (int j = Dialog.dialog_cycles.Count - 1; j >= i; j--)
                    {
                        if (j == Dialog.dialog_cycles.Count - 1)
                        {
                            Dialog.dialog_cycles.Add(Dialog.dialog_cycles[j]);

                        }
                        else
                        {
                            Dialog.dialog_cycles[j + 1] = Dialog.dialog_cycles[j];
                        }

                    }
                    Dialog.dialog_cycles[i + 1] = d;
                    Dialog.dialog_cycles[i + 1].cycle_index = i + 1;
                    for (int j = 0; j <= Dialog.dialog_cycles.Count - 1; j++)
                    {
                        Dialog.dialog_cycles[j].cycle_index = j;
                    }

                    show_dialog.Insert(i, false);
                }

                if (GUILayout.Button("현재 다이얼로그 삭제"))
                {
                    Dialog.dialog_cycles.RemoveAt(i);
                    for (int j = 0; j < Dialog.dialog_cycles.Count - 1; j++)
                    {
                        Dialog.dialog_cycles[j].cycle_index = j;
                    }

                    show_dialog.RemoveAt(i);
                }
                EditorGUI.indentLevel -= 1;
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("다이얼로그 추가"))
        {
            Dialog_cycle d = new Dialog_cycle();
            Dialog.dialog_cycles.Add(d);
            Dialog.dialog_cycles[Dialog.dialog_cycles.Count - 1].cycle_index = Dialog.dialog_cycles.Count - 1;
            show_dialog.Add(false);
        }

        if (GUILayout.Button("다이얼로그 삭제"))
        {

            Dialog.dialog_cycles.RemoveAt(Dialog.dialog_cycles.Count - 1);
            show_dialog.RemoveAt(Dialog.dialog_cycles.Count-1);
        }
    }
}
