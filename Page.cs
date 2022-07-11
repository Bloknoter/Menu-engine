using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MenuEngine.TransitionAnimations;

namespace MenuEngine
{
    [System.Serializable]
    public class Page
    {
        public string Name;
        public GameObject PageObject;
        public Transition[] transitions;

        public override string ToString()
        {
            if (transitions != null && transitions.Length > 0)
                return $"Page: \nName: {Name} \nTrasitions amount: {transitions.Length}";
            else
                return $"Page: \nName: {Name} \nNo transitions";
        }
    }

    [System.Serializable]
    public class Transition
    {
        public enum TransitionTypeEnum
        {
            None,
            ScreenSliding,
            //HorizontalSlide, 
            //VerticalSlide
        }
        public string transition;
        public TransitionTypeEnum TransitionType = TransitionTypeEnum.None;
        public TransitionAnimation transitionAnimation;
    }
}
