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
            None = 0,
            Custom = 1,
            ScreenSliding = 2,
            //HorizontalSlide = 3, 
            //VerticalSlide
        }
        public string transition;
        public bool hideCurrent = true;
        public TransitionTypeEnum TransitionType = TransitionTypeEnum.None;
        public TransitionAnimation transitionAnimation;
        public AudioClip sound;

        public override string ToString()
        {
            string result = $"Transition to '{transition}' page";
            if (hideCurrent)
                result += " | hide current page";
            else
                result += " | don't hide current page";

            if (TransitionType == TransitionTypeEnum.None)
                result += " | no animation";
            else if (TransitionType == TransitionTypeEnum.Custom)
                result += " | custom animation";
            else
                result += $" | {TransitionType} animation";

            if (sound != null)
                result += $" | sound: {sound.name}";
            else
                result += " | no sound";

            return result;
        }
    }
}
