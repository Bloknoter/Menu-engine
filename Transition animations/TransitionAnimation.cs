using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MenuEngine.TransitionAnimations
{
    public abstract class TransitionAnimation : MonoBehaviour
    {
        [SerializeField]
        [Min(0)]
        protected float Duration;

        public abstract void Animate(Page currentPage, Page nextPage);
    }
}
