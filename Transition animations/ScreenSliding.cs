using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MenuEngine.TransitionAnimations
{
    public class ScreenSliding : TransitionAnimation
    {
        private const float DELTA_MOVEMENT_TIME = 0.01f;

        [SerializeField]
        private RectTransform Screen;

        [SerializeField]
        private RectTransform canvas;

        private enum SlidingTypeEnum
        {
            Horizontal, Vertical
        }

        [SerializeField]
        private SlidingTypeEnum SlidingType;

        private Page _currentPage;

        private Page _nextPage;

        void Start()
        {

        }

        void Update()
        {

        }

        public override void Animate(Page currentPage, Page nextPage)
        {
            Screen.gameObject.SetActive(false);
            StopAllCoroutines();
            _currentPage = currentPage;
            _nextPage = nextPage;
            StartCoroutine(AnimateTransition());
        }

        private IEnumerator AnimateTransition()
        {
            if (SlidingType == SlidingTypeEnum.Horizontal)
            {
                float XDelta = canvas.rect.width + Screen.rect.width / 2;
                Screen.localPosition = new Vector2(XDelta, 0);
                Screen.gameObject.SetActive(true);
                int iteractionCount = (int)(Duration / 2 / DELTA_MOVEMENT_TIME);
                for (int i = 0; i < iteractionCount; i++)
                {
                    Screen.localPosition = new Vector2(Mathf.Lerp(Screen.localPosition.x, 0, 0.1f), 0f);
                    //yield return new WaitForSecondsRealtime(DELTA_MOVEMENT_TIME);
                    yield return new WaitForFixedUpdate();
                }
                _currentPage.PageObject.SetActive(false);
                _nextPage.PageObject.SetActive(true);
                for (int i = 0; i < iteractionCount; i++)
                {
                    Screen.localPosition = new Vector2(Mathf.Lerp(Screen.localPosition.x, -XDelta, 0.1f), 0f);
                    //yield return new WaitForSecondsRealtime(DELTA_MOVEMENT_TIME);
                    yield return new WaitForFixedUpdate();
                }
                Screen.gameObject.SetActive(false);
            }
            else if(SlidingType == SlidingTypeEnum.Vertical)
            {
                float YDelta = canvas.rect.height + Screen.rect.height / 2;
                Screen.localPosition = new Vector2(0, YDelta);
                Screen.gameObject.SetActive(true);
                int iteractionCount = (int)(Duration / 2 / DELTA_MOVEMENT_TIME);
                for (int i = 0; i < iteractionCount; i++)
                {
                    Screen.localPosition = new Vector2(0f, Mathf.Lerp(Screen.localPosition.y, 0, 0.1f));
                    //yield return new WaitForSecondsRealtime(DELTA_MOVEMENT_TIME);
                    yield return new WaitForFixedUpdate();
                }
                _currentPage.PageObject.SetActive(false);
                _nextPage.PageObject.SetActive(true);
                for (int i = 0; i < iteractionCount; i++)
                {
                    Screen.localPosition = new Vector2(0f, Mathf.Lerp(Screen.localPosition.y, -YDelta, 0.1f));
                    //yield return new WaitForSecondsRealtime(DELTA_MOVEMENT_TIME);
                    yield return new WaitForFixedUpdate();
                }
                Screen.gameObject.SetActive(false);
            }
        }

    }
}
