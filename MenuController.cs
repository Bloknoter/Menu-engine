using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MenuEngine
{
    [RequireComponent(typeof(AudioSource))]
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private Page[] pages;

        [SerializeField]
        private string StartPageName;

        [SerializeField]
        private AudioSource audioSource;

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
            audioSource.loop = false;
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
                            Transition transition = currentPage.transitions[t];
                            if (CompareStrings(transition.transition, pageName))
                            {  
                                audioSource.clip = transition.sound;
                                if (audioSource.clip != null)
                                    audioSource.Play();
                                if (transition.TransitionType == Transition.TransitionTypeEnum.None)
                                {
                                    if (transition.hideCurrent)
                                        currentPage.PageObject.SetActive(false);
                                }
                                else
                                {
                                    currentPage.transitions[t].transitionAnimation.Animate(currentPage, pages[i], currentPage.transitions[t]);
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

        public void LoadScene(string scene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
