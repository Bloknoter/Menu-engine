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
        private const string PAGE_FIELD_NAME = "Set name";

        private SerializedProperty StartPageName;

        private SerializedProperty pages;

        private SerializedProperty audioSource;

        private GUIContent deleteIcon;
        private GUIContent addIcon;
        private GUIContent addTransitionIcon;
        private GUIContent pageIcon;
        private GUIContent errorIcon;
        private GUIContent warningIcon;
        private GUIContent transitionIcon;

        private GUIContent emptyContent;

        private void OnEnable()
        {
            StartPageName = serializedObject.FindProperty("StartPageName");
            pages = serializedObject.FindProperty("pages");
            audioSource = serializedObject.FindProperty("audioSource");

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
            emptyContent = new GUIContent();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.HelpBox("Page handler is not case-sensitive and space-sensitive! \nEvery name must be unique!", MessageType.Info);
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
                Dictionary<string, int> names = new Dictionary<string, int>();
                for(int i = 0; i < pages.arraySize;i++)
                {
                    EditorGUILayout.BeginVertical("Tooltip");
                    SerializedProperty page = pages.GetArrayElementAtIndex(i);
                    SerializedProperty Name = page.FindPropertyRelative("Name");
                    string pageName = Name.stringValue;
                    
                    SerializedProperty pageobject = page.FindPropertyRelative("PageObject");

                    EditorGUILayout.BeginHorizontal();
                    Rect rect = EditorGUILayout.GetControlRect();
                    rect.x += 20;
                    rect.width -= 30;
                    if (pageName == "")
                        page.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(rect, page.isExpanded, $"<Empty name>", EditorStyles.popup);
                    else
                        page.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(rect, page.isExpanded, $"{pageName}", EditorStyles.popup);
                    if (pageName == "" || names.ContainsKey(pageName.ToLowerInvariant().Replace(" ", "")))
                    {
                        EditorGUILayout.LabelField(warningIcon, GUILayout.Width(20));
                    }
                    else
                    {
                        EditorGUILayout.LabelField(emptyContent, GUILayout.Width(20));
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
                        GUI.SetNextControlName(PAGE_FIELD_NAME + $"{i}");
                        Name.stringValue =  EditorGUILayout.TextField("Name", Name.stringValue);
                        pageName = Name.stringValue;
                        if (pageName == "")
                        {
                            EditorGUILayout.HelpBox("Name this page, or it won't work correctly", MessageType.Warning);
                        }
                        if (names.ContainsKey(pageName))
                        {
                            EditorGUILayout.HelpBox("Page with this name already exists", MessageType.Warning);
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
                                rect.width -= 30;
                                SerializedProperty transition = transitions.GetArrayElementAtIndex(t);
                                SerializedProperty nextpage = transition.FindPropertyRelative("transition");
                                transition.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(rect, transition.isExpanded, $"settings");
                                EditorGUILayout.LabelField("->", GUILayout.Width(20));
                                EditorGUILayout.EndFoldoutHeaderGroup();
                                List<string> pagesnames = new List<string>();
                                int selected = 0;
                                for (int p = 0; p < pages.arraySize; p++)
                                {
                                    SerializedProperty otherName = pages.GetArrayElementAtIndex(p).FindPropertyRelative("Name");
                                    if (otherName.stringValue != "" && otherName.stringValue != pageName)
                                    {
                                        pagesnames.Add(otherName.stringValue);
                                        if (nextpage.stringValue != "" && nextpage.stringValue == otherName.stringValue)
                                        {
                                            selected = pagesnames.Count - 1;
                                        }
                                    }
                                }
                                if (pagesnames.Count > 0)
                                {
                                    int id = EditorGUILayout.IntPopup(selected, pagesnames.ToArray(), null);
                                    nextpage.stringValue = pagesnames[id];
                                }
                                else
                                {
                                    EditorGUILayout.LabelField("No other pages were created");
                                }
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
                                    /*List<string> pagesnames = new List<string>();
                                    int selected = 0;
                                    for (int p = 0; p < pages.arraySize; p++)
                                    {
                                        SerializedProperty otherName = pages.GetArrayElementAtIndex(p).FindPropertyRelative("Name");
                                        if (otherName.stringValue != "" && otherName.stringValue != pageName)
                                        {
                                            pagesnames.Add(otherName.stringValue);
                                            if (nextpage.stringValue != "" && nextpage.stringValue == otherName.stringValue)
                                            {
                                                selected = pagesnames.Count - 1;
                                            }
                                        }
                                    }
                                    if (pagesnames.Count > 0)
                                    {
                                        int id = EditorGUILayout.IntPopup("transition to page", selected, pagesnames.ToArray(), null);
                                        nextpage.stringValue = pagesnames[id];
                                    }
                                    else
                                    {
                                        EditorGUILayout.LabelField("No other pages were created");
                                    }*/
                                    
                                    SerializedProperty hideCurrent = transition.FindPropertyRelative("hideCurrent");
                                    hideCurrent.boolValue = EditorGUILayout.Toggle("Hide current page", hideCurrent.boolValue);
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
                                    SerializedProperty sound = transition.FindPropertyRelative("sound");
                                    sound.objectReferenceValue = EditorGUILayout.ObjectField("Sound", sound.objectReferenceValue, typeof(AudioClip), true);
                                    EditorGUI.indentLevel -= 2;
                                }
                            }
                        }
                        
                    }
                    string pageNameValid = pageName.ToLowerInvariant().Replace(" ", "");
                    if (!names.ContainsKey(pageNameValid))
                        names.Add(pageNameValid, 0);
                    EditorGUILayout.EndVertical();
                    //EditorGUILayout.LabelField("____________________________________________________________________________");
                    EditorGUILayout.Separator();
                    EditorGUILayout.Separator();
                    EditorGUILayout.Separator();
                }

                

            }

            audioSource.objectReferenceValue = EditorGUILayout.ObjectField("Audio output", audioSource.objectReferenceValue, typeof(AudioSource), true);

            if(pages.arraySize > 0)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                string currentpage = ((MenuController)serializedObject.targetObject).CurrentPage;
                if(string.IsNullOrEmpty(currentpage))
                {
                    currentpage = StartPageName.stringValue;
                }
                EditorGUILayout.LabelField($"Current page", GUILayout.Width(100));
                EditorGUILayout.LabelField(transitionIcon, GUILayout.Width(30));
                EditorGUILayout.LabelField(currentpage);
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
