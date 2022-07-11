using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MenuEngine
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private Page[] pages;

        [SerializeField]
        private string StartPageName;

        private Page currentPage;
        public string CurrentPage
        {
            get
            {
                if (currentPage != null)
                    return currentPage.Name;
                else
                    return StartPageName;
            }
        }

        void Start()
        {
            currentPage = pages[0];
            for (int i = 0; i < pages.Length;i++)
            {
                if (CompareStrings(pages[i].Name, StartPageName))
                {
                    currentPage = pages[i];
                }
                else
                {
                    pages[i].PageObject.SetActive(false);
                }
            }
            currentPage.PageObject.SetActive(true);
        }

        void Update()
        {

        }

        public void SetPage(string pageName)
        {
            for (int i = 0; i < pages.Length;i++)
            {
                if (CompareStrings(pages[i].Name, pageName))
                {
                    bool hastransition = false;
                    if (currentPage.transitions != null && currentPage.transitions.Length > 0)
                    {
                        for (int t = 0;t < currentPage.transitions.Length;t++)
                        {
                            if (CompareStrings(currentPage.transitions[t].transition, pageName))
                            {
                                if (currentPage.transitions[t].TransitionType == Transition.TransitionTypeEnum.None)
                                {
                                    currentPage.PageObject.SetActive(false);
                                }
                                else
                                {
                                    currentPage.transitions[t].transitionAnimation.Animate(currentPage, pages[i]);
                                    hastransition = true;
                                }
                                break;
                            }
                        }
                    }
                    currentPage = pages[i];
                    if (!hastransition)
                        currentPage.PageObject.SetActive(true);
                    
                    break;
                }
            }
        }

        private bool CompareStrings(string string1, string string2)
        {
            return string1.ToLowerInvariant().Replace(" ", "").Equals(string2.ToLowerInvariant().Replace(" ", ""),
                System.StringComparison.CurrentCultureIgnoreCase);
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
