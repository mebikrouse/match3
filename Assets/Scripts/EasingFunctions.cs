public static class EasingFunctions
{
    /* https://gist.github.com/cjddmut/d789b9eb78216998e95c */

    public static float EaseOutBack(float start, float end, float value)
    {
        float s = 1.70158f;
        end -= start;
        value = (value) - 1;
        return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
    }
}
