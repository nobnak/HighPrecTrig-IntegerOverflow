#ifndef TICK_CONVERSION_HLSL
#define TICK_CONVERSION_HLSL

uint _GTick;
int _TickPeriod;

static const float UINT_PERIOD = 4294967296.0;
static const float INV_UINT_PERIOD = 1.0 / UINT_PERIOD;
static const float SECONDS_PER_DAY = 86400.0;
static const float SECONDS_PER_HOUR = 3600.0;
static const float TICKS_PER_SECOND_DAY = UINT_PERIOD / SECONDS_PER_DAY;
static const float TICKS_PER_SECOND_HOUR = UINT_PERIOD / SECONDS_PER_HOUR;
static const float SECONDS_PER_TICK_DAY = SECONDS_PER_DAY / UINT_PERIOD;
static const float SECONDS_PER_TICK_HOUR = SECONDS_PER_HOUR / UINT_PERIOD;
static const float UINT_TO_2PI = TWO_PI / UINT_PERIOD;

#define TICK_PERIOD_DAY 0
#define TICK_PERIOD_HOUR 1

uint SecondsToTick(float dt) {
    float ticksPerSecond = _TickPeriod == TICK_PERIOD_DAY ? TICKS_PER_SECOND_DAY : TICKS_PER_SECOND_HOUR;
    return (uint)fmod(dt * ticksPerSecond, UINT_PERIOD);
}

uint2 SecondsToTick(float2 dt) {
    float ticksPerSecond = _TickPeriod == TICK_PERIOD_DAY ? TICKS_PER_SECOND_DAY : TICKS_PER_SECOND_HOUR;
    return (uint2)fmod(dt * ticksPerSecond, UINT_PERIOD);
}

float TickToSeconds(uint tick) {
    float secondsPerTick = _TickPeriod == TICK_PERIOD_DAY ? SECONDS_PER_TICK_DAY : SECONDS_PER_TICK_HOUR;
    return (float)tick * secondsPerTick;
}

float2 TickToSeconds(uint2 tick) {
    float secondsPerTick = _TickPeriod == TICK_PERIOD_DAY ? SECONDS_PER_TICK_DAY : SECONDS_PER_TICK_HOUR;
    return (float2)tick * secondsPerTick;
}

float ToNSecondPeriod(uint tick, float period) {
    float basePeriod = _TickPeriod == TICK_PERIOD_DAY ? SECONDS_PER_DAY : SECONDS_PER_HOUR;
    uint multiplier = (uint)(basePeriod / period);
    uint nSecondCounter = tick * multiplier;
    return (float)nSecondCounter * INV_UINT_PERIOD;
}

float2 ToNSecondPeriod(uint2 tick, float period) {
    float basePeriod = _TickPeriod == TICK_PERIOD_DAY ? SECONDS_PER_DAY : SECONDS_PER_HOUR;
    uint multiplier = (uint)(basePeriod / period);
    uint2 nSecondCounter = tick * multiplier;
    return (float2)nSecondCounter * INV_UINT_PERIOD;
}

float UintToRadian(uint val) {
    return val * UINT_TO_2PI;
}

float2 UintToRadian(uint2 val) {
    return (float2)val * UINT_TO_2PI;
}

uint TickModPeriod(uint tick, float period) {
    uint periodTicks = SecondsToTick(period);
    return tick % periodTicks;
}

uint2 TickModPeriod(uint2 tick, float period) {
    uint periodTicks = SecondsToTick(period);
    return tick % periodTicks;
}

#endif // TICK_CONVERSION_HLSL
