using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

using MenuEngine.TransitionAnimations;

namespace MenuEngine.EditorScripts
{
    [CustomEditor(typeof(MenuController))]
    [CanEditMultipleObjects]
    public class MenuControllerEditor : Editor
    {
        private SerializedProperty StartPageName;

        private SerializedProperty pages;

        private void OnEnable()
        {
            StartPageName = serializedObject.FindProperty("StartPageName");
            pages = serializedObject.FindProperty("pages");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.HelpBox("The work with the names of pages does not depends on the string case and the spaces. \n \nBe carefull! Every name must be unique!", MessageType.Info);
            StartPageName.stringValue = EditorGUILayout.TextField("Start page", StartPageName.stringValue);

            if(GUILayout.Button("Add page", GUILayout.Width(100)))
            {
                pages.InsertArrayElementAtIndex(pages.arraySize);
                pages.GetArrayElementAtIndex(pages.arraySize - 1).FindPropertyRelative("Name").stringValue = "New page";
                pages.GetArrayElementAtIndex(pages.arraySize - 1).FindPropertyRelative("PageObject").objectReferenceValue = null;
                pages.GetArrayElementAtIndex(pages.arraySize - 1).FindPropertyRelative("transitions").ClearArray();
            }

            EditorGUILayout.LabelField("____________________________________________________________________________");

            if (pages.arraySize > 0)
            {
                
                for(int i = 0; i < pages.arraySize;i++)
                {
                    SerializedProperty page = pages.GetArrayElementAtIndex(i);
                    SerializedProperty Name = page.FindPropertyRelative("Name");
                    if (Name.stringValue == "")
                        page.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(page.isExpanded, $"Page {i + 1}");
                    else
                        page.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(page.isExpanded, $"{Name.stringValue}");
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    if (page.isExpanded)
                    {
                        EditorGUI.indentLevel += 1;
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(Name);
                        if (GUILayout.Button("Delete", GUILayout.Width(80)))
                        {
                            pages.DeleteArrayElementAtIndex(i);
                            i--;
                            continue;
                        }
                        EditorGUILayout.EndHorizontal();

                        if (Name.stringValue == "")
                        {
                            EditorGUILayout.HelpBox("Name this page, or it won't work correctly", MessageType.Warning);
                        }

                        SerializedProperty pageobject = page.FindPropertyRelative("PageObject");
                        EditorGUILayout.PropertyField(page.FindPropertyRelative("PageObject"), new GUIContent("Page object"));
                        if (pageobject.objectReferenceValue == null)
                        {
                            EditorGUILayout.HelpBox("You must set the page Gameobject!", MessageType.Error);
                        }

                        if (pages.arraySize > 1)
                        {
                            SerializedProperty transitions = page.FindPropertyRelative("transitions");
                            if (GUILayout.Button("Add transition", GUILayout.Width(120)))
                            {
                                transitions.InsertArrayElementAtIndex(transitions.arraySize);
                            }


                            EditorGUI.indentLevel += 1;

                            for (int t = 0; t < transitions.arraySize; t++)
                            {
                                SerializedProperty transition = transitions.GetArrayElementAtIndex(t);
                                SerializedProperty nextpage = transition.FindPropertyRelative("transition");
                                transition.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(transition.isExpanded, $"transition: {nextpage.stringValue}");
                                EditorGUILayout.EndFoldoutHeaderGroup();
                                if (transition.isExpanded)
                                {
                                    List<string> pagesnames = new List<string>();
                                    int selected = 0;
                                    for (int p = 0; p < pages.arraySize; p++)
                                    {
                                        SerializedProperty otherName = pages.GetArrayElementAtIndex(p).FindPropertyRelative("Name");
                                        if (otherName.stringValue != "" && otherName.stringValue != Name.stringValue)
                                        {
                                            pagesnames.Add(otherName.stringValue);
                                            if (nextpage.stringValue != "" && nextpage.stringValue == otherName.stringValue)
                                            {
                                                selected = pagesnames.Count - 1;
                                            }
                                        }
                                    }
                                    EditorGUILayout.BeginHorizontal();
                                    if (pagesnames.Count > 0)
                                    {
                                        int id = EditorGUILayout.IntPopup("transition to page", selected, pagesnames.ToArray(), null);
                                        nextpage.stringValue = pagesnames[id];
                                    }
                                    else
                                    {
                                        EditorGUILayout.LabelField("No other pages were created");
                                    }
                                    if (GUILayout.Button("Delete", GUILayout.Width(80)))
                                    {
                                        transitions.DeleteArrayElementAtIndex(t);
                                        t--;
                                        continue;
                                    }
                                    EditorGUILayout.EndHorizontal();
                                    SerializedProperty type = transition.FindPropertyRelative("TransitionType");
                                    EditorGUILayout.PropertyField(type, new GUIContent("Animation"));
                                    if (type.enumValueIndex == 1)
                                    {
                                        SerializedProperty transitionAnimation = transition.FindPropertyRelative("transitionAnimation");
                                        transitionAnimation.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Screen sliding animation"),
                                            transitionAnimation.objectReferenceValue, typeof(ScreenSliding), true);
                                    }
                                }
                            }
                            EditorGUI.indentLevel -= 1;
                        }
                        
                        EditorGUI.indentLevel -= 1;
                    }

                    EditorGUILayout.LabelField("____________________________________________________________________________");
                }

                string currentpage = ((MenuController)serializedObject.targetObject).CurrentPage;

                if (/*currentpage != null && currentpage != ""*/true)
                {
                    EditorGUILayout.LabelField($"Current page: {currentpage}");
                }

            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
