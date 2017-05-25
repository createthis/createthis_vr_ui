using System.Collections;
using UnityEngine;

namespace CreateThis.VR.UI.Button {
    public class MomentaryButton : ButtonBase {
        public bool repeat;

        private float initialRepeatDelayInSeconds = 0.5f;
        private float repeatDelayInSeconds = 0.1f;
        private IEnumerator repeatCoroutine;

        protected override void SelectedExitAfterHitTravelLimitHandler(Transform controller, int controllerIndex) {
            if (repeat) {
                StopCoroutine(repeatCoroutine);
            }
        }

        protected override void HitTravelLimitHandler(Transform controller, int controllerIndex) {
            if (!clickOnTriggerExit) {
                ClickHandler(controller, controllerIndex);
            }
            if (repeat) {
                repeatCoroutine = RepeatCoroutine(controller, controllerIndex);
                StartCoroutine(repeatCoroutine);
            }
        }

        private IEnumerator RepeatCoroutine(Transform controller, int controllerIndex) {
            yield return new WaitForSeconds(initialRepeatDelayInSeconds);
            while (true) {
                StartCoroutine(Haptic.LongVibration(controllerIndex, 0.1f, 1f));
                ClickHandler(controller, controllerIndex);
                yield return new WaitForSeconds(repeatDelayInSeconds);
            }
        }
    }
}
