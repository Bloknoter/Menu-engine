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

        private GUIContent deleteIcon;
        private GUIContent addIcon;
        private GUIContent addTransitionIcon;
        private GUIContent pageIcon;
        private GUIContent errorIcon;
        private GUIContent warningIcon;
        private GUIContent transitionIcon;

        private void OnEnable()
        {
            StartPageName = serializedObject.FindProperty("StartPageName");
            pages = serializedObject.FindProperty("pages");

            deleteIcon = EditorGUIUtility.IconContent("d_TreeEditor.Trash");
            deleteIcon.tooltip = "delete";
            addIcon = EditorGUIUtility.IconContent("d_CreateAddNew");
            addIcon.tooltip = "add";
            addTransitionIcon = EditorGUIUtility.IconContent("CreateAddNew");
            addTransitionIcon.tooltip = "add new transition";
            pageIcon = EditorGUIUtility.IconContent("d_UnityEditor.ConsoleWindow");
            pageIcon.tooltip = "add new page";
            errorIcon = EditorGUIUtility.IconContent("console.erroricon.sml");
            errorIcon.tooltip = "fix errors!";
            warningIcon = EditorGUIUtility.IconContent("console.warnicon.sml");
            warningIcon.tooltip = "fix warnings!";
            transitionIcon = EditorGUIUtility.IconContent("d_tab_next");
            //"d_forward"
                
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.HelpBox("The work with the names of pages does not depends on the string case and the spaces. \n \nBe carefull! Every name must be unique!", MessageType.Info);
            EditorGUILayout.Separator();
            StartPageName.stringValue = EditorGUILayout.TextField("Start page", StartPageName.stringValue);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(addIcon, GUILayout.Width(10));
            if(GUILayout.Button(pageIcon, EditorStyles.miniButton, GUILayout.Width(50)))
            {
                pages.InsertArrayElementAtIndex(pages.arraySize);
                pages.GetArrayElementAtIndex(pages.arraySize - 1).FindPropertyRelative("Name").stringValue = "New page";
                pages.GetArrayElementAtIndex(pages.arraySize - 1).FindPropertyRelative("PageObject").objectReferenceValue = null;
                pages.GetArrayElementAtIndex(pages.arraySize - 1).FindPropertyRelative("transitions").ClearArray();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (pages.arraySize > 0)
            {
                
                for(int i = 0; i < pages.arraySize;i++)
                {
                    EditorGUILayout.BeginVertical("Tooltip");
                    SerializedProperty page = pages.GetArrayElementAtIndex(i);
                    SerializedProperty Name = page.FindPropertyRelative("Name");
                    SerializedProperty pageobject = page.FindPropertyRelative("PageObject");

                    EditorGUILayout.BeginHorizontal();
                    Rect rect = EditorGUILayout.GetControlRect();
                    rect.x += 20;
                    rect.width -= 30;
                    if (Name.stringValue == "")
                        page.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(rect, page.isExpanded, $"Page {i + 1}", EditorStyles.popup);
                    else
                        page.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(rect, page.isExpanded, $"{Name.stringValue}", EditorStyles.popup);
                    if(Name.stringValue == "")
                    {
                        EditorGUILayout.LabelField(warningIcon, GUILayout.Width(20));
                    }
                    if (pageobject.objectReferenceValue == null)
                    {
                        EditorGUILayout.LabelField(errorIcon, GUILayout.Width(20));
                    }
                    EditorGUI.EndFoldoutHeaderGroup();
                    if (GUILayout.Button(deleteIcon, GUILayout.Width(50)))
                    {
                        pages.DeleteArrayElementAtIndex(i);
                        i--;
                        continue;
                    }
                    EditorGUILayout.EndHorizontal();
                    if (page.isExpanded)
                    {
                        EditorGUILayout.PropertyField(Name);
                       
                        if (Name.stringValue == "")
                        {
                            EditorGUILayout.HelpBox("Name this page, or it won't work correctly", MessageType.Warning);
                        }

                        
                        EditorGUILayout.PropertyField(page.FindPropertyRelative("PageObject"), new GUIContent("Page object"));
                        if (pageobject.objectReferenceValue == null)
                        {
                            EditorGUILayout.HelpBox("You must set the page Gameobject!", MessageType.Error);
                        }

                        if (pages.arraySize > 1)
                        {
                            SerializedProperty transitions = page.FindPropertyRelative("transitions");
                            if (GUILayout.Button(addTransitionIcon, EditorStyles.miniButton, GUILayout.Width(30)))
                            {
                                transitions.InsertArrayElementAtIndex(transitions.arraySize);
                            }

                            for (int t = 0; t < transitions.arraySize; t++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                rect = EditorGUILayout.GetControlRect();
                                rect.x += 15;
                                rect.width -= 50;
                                SerializedProperty transition = transitions.GetArrayElementAtIndex(t);
                                SerializedProperty nextpage = transition.FindPropertyRelative("transition");
                                transition.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(rect, transition.isExpanded, $"{Name.stringValue} -> {nextpage.stringValue}");
                                EditorGUILayout.EndFoldoutHeaderGroup();
                                if (GUILayout.Button(EditorGUIUtility.IconContent("d_TreeEditor.Trash"), GUILayout.Width(40)))
                                {
                                    transitions.DeleteArrayElementAtIndex(t);
                                    t--;
                                    continue;
                                }
                                EditorGUILayout.EndHorizontal();
                                if (transition.isExpanded)
                                {
                                    EditorGUI.indentLevel += 2;
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
                                    
                                    EditorGUILayout.EndHorizontal();
                                    SerializedProperty type = transition.FindPropertyRelative("TransitionType");
                                    EditorGUILayout.PropertyField(type, new GUIContent("Animation"));
                                    if (type.enumValueIndex == 1)
                                    {
                                        SerializedProperty transitionAnimation = transition.FindPropertyRelative("transitionAnimation");
                                        transitionAnimation.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Screen sliding animation"),
                                            transitionAnimation.objectReferenceValue, typeof(ScreenSliding), true);
                                    }
                                    if (type.enumValueIndex == 2)
                                    {
                                        SerializedProperty transitionAnimation = transition.FindPropertyRelative("transitionAnimation");
                                        transitionAnimation.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Custom animation"),
                                            transitionAnimation.objectReferenceValue, typeof(TransitionAnimation), true);
                                    }
                                    EditorGUI.indentLevel -= 2;
                                }
                            }
                        }
                        
                    }
                    EditorGUILayout.EndVertical();
                    //EditorGUILayout.LabelField("____________________________________________________________________________");
                    EditorGUILayout.Separator();
                    EditorGUILayout.Separator();
                    EditorGUILayout.Separator();
                }

                string currentpage = ((MenuController)serializedObject.targetObject).CurrentPage;
                EditorGUILayout.LabelField($"Current page: {currentpage}");

            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
