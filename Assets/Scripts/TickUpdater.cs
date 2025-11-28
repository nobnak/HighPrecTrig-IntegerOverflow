using UnityEngine;

[ExecuteAlways]
public class TickUpdater : MonoBehaviour {
    const double UINT_PERIOD = 4294967296.0;
    const double SECONDS_PER_DAY = 86400.0;
    const double TICKS_PER_SECOND = UINT_PERIOD / SECONDS_PER_DAY;

    static readonly int P_GTick = Shader.PropertyToID("_GTick");

    private void Update() {
        double elapsedSeconds = Time.timeAsDouble % SECONDS_PER_DAY;
        uint tick = (uint)(elapsedSeconds * TICKS_PER_SECOND % UINT_PERIOD);
        Shader.SetGlobalInt(P_GTick, (int)tick);
    }
}

