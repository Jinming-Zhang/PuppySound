using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField]
    Volume postProcessingVolume;
    Vignette vignette;
    [SerializeField]
    AnimationCurve curve;
    private static PostProcessingController instance;
    public static PostProcessingController Iinstance => instance;
    Coroutine pulseVignetteCr;
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        if (postProcessingVolume.profile.TryGet<Vignette>(out var tmp))
        {
            vignette = tmp;
        }
        //PulseVignette(new VignetteSetting(Color.red, 0.35f, 0.5f, 0.5f, 0.5f, 1.5f, .3f, 1.5f, .1f));
    }
    public void PulseVignette(VignetteSetting config)
    {
        if (pulseVignetteCr == null)
        {
            vignette.active = true;
            pulseVignetteCr = StartCoroutine(PulseVignetteCR(config));
        }

    }

    public void DisableVignette()
    {
        if (pulseVignetteCr != null)
        {
            StopCoroutine(pulseVignetteCr);
            pulseVignetteCr = null;
        }
        vignette.active = false;
    }

    IEnumerator PulseVignetteCR(VignetteSetting config)
    {
        vignette.color.Override(config.color);
        vignette.intensity.Override(config.intensityStart);
        vignette.smoothness.Override(config.smoothnessStart);

        float intensityExpandDelta = (config.intensityEnd - config.intensityStart) / config.expandDuration;
        float intensityShrinkDelta = (config.intensityEnd - config.intensityStart) / config.shrinkDuration;

        float smoothnessExpandDelta = (config.smoothnessEnd - config.smoothnessStart) / config.expandDuration;
        float smoothnessShrinkDelta = (config.smoothnessEnd - config.smoothnessStart) / config.shrinkDuration;

        while (true)
        {
            // expand
            while (vignette.intensity.value < config.intensityEnd)
            {
                vignette.intensity.value = new ClampedFloatParameter(vignette.intensity.value + intensityExpandDelta * Time.deltaTime, config.intensityStart, config.intensityEnd).value;

                vignette.smoothness.value = new ClampedFloatParameter(vignette.smoothness.value + smoothnessExpandDelta * Time.deltaTime, config.smoothnessStart, config.smoothnessEnd).value;
                yield return new WaitForEndOfFrame();
            }
            // stay
            yield return new WaitForSeconds(config.expandedStayDuration);
            // shrink
            while (vignette.intensity.value > config.intensityStart)
            {
                vignette.intensity.value = new ClampedFloatParameter(vignette.intensity.value - intensityShrinkDelta * Time.deltaTime, config.intensityStart, config.intensityEnd).value;

                vignette.smoothness.value = new ClampedFloatParameter(vignette.smoothness.value - smoothnessShrinkDelta * Time.deltaTime, config.smoothnessStart, config.smoothnessEnd).value;
                yield return new WaitForEndOfFrame();
            }
            // stay
            yield return new WaitForSeconds(config.shrinkedStayDuration);
        }
    }

    public class VignetteSetting
    {
        public Color color;
        public float intensityStart;
        public float intensityEnd;
        public float smoothnessStart;
        public float smoothnessEnd;
        public float expandDuration;
        public float expandedStayDuration;
        public float shrinkDuration;
        public float shrinkedStayDuration;
        public VignetteSetting(Color color, float intensityStart, float intensityEnd, float smoothnessStart, float smoothnessEnd, float expandDuration = 2, float expandedStayDuration = 2, float shrinkDuration = 2, float shrinkedStayDuration = 2)
        {
            this.color = color;
            this.intensityStart = intensityStart;
            this.intensityEnd = intensityEnd;
            this.smoothnessStart = smoothnessStart;
            this.smoothnessEnd = smoothnessEnd;
            this.expandDuration = expandDuration;
            this.expandedStayDuration = expandedStayDuration;
            this.shrinkDuration = shrinkDuration;
            this.shrinkedStayDuration = shrinkedStayDuration;
        }
    }
}
