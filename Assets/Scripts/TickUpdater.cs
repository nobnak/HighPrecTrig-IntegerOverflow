using UnityEngine;

[ExecuteAlways]
public class TickUpdater : MonoBehaviour {
    public enum TickPeriod {
        Day = 0,
        Hour = 1
    }

    [SerializeField] TickPeriod period = TickPeriod.Day;

    const double UINT_PERIOD = 4294967296.0;
    const double SECONDS_PER_DAY = 86400.0;
    const double SECONDS_PER_HOUR = 3600.0;
    const double TICKS_PER_SECOND_DAY = UINT_PERIOD / SECONDS_PER_DAY;
    const double TICKS_PER_SECOND_HOUR = UINT_PERIOD / SECONDS_PER_HOUR;

    static readonly int P_GTick = Shader.PropertyToID("_GTick");
    static readonly int P_TickPeriod = Shader.PropertyToID("_TickPeriod");

    private void Update() {
        double periodSeconds = period == TickPeriod.Day ? SECONDS_PER_DAY : SECONDS_PER_HOUR;
        double ticksPerSecond = period == TickPeriod.Day ? TICKS_PER_SECOND_DAY : TICKS_PER_SECOND_HOUR;
        double elapsedSeconds = Time.timeAsDouble % periodSeconds;
        uint tick = (uint)(elapsedSeconds * ticksPerSecond % UINT_PERIOD);
        Shader.SetGlobalInt(P_GTick, (int)tick);
        Shader.SetGlobalInt(P_TickPeriod, (int)period);
    }
}

