#ifndef TICK_CONVERSION_HLSL
#define TICK_CONVERSION_HLSL

uint _GTick;

static const float UINT_PERIOD = 4294967296.0;
static const float INV_UINT_PERIOD = 1.0 / UINT_PERIOD;
static const float SECONDS_PER_DAY = 86400.0;
static const float TICKS_PER_SECOND = UINT_PERIOD / SECONDS_PER_DAY;
static const float SECONDS_PER_TICK = SECONDS_PER_DAY / UINT_PERIOD;
static const float UINT_TO_2PI = TWO_PI / UINT_PERIOD;

uint SecondsToTick(float dt) {
    return (uint)fmod(dt * TICKS_PER_SECOND, UINT_PERIOD);
}

uint2 SecondsToTick(float2 dt) {
    return (uint2)fmod(dt * TICKS_PER_SECOND, UINT_PERIOD);
}

float TickToSeconds(uint tick) {
    return (float)tick * SECONDS_PER_TICK;
}

float2 TickToSeconds(uint2 tick) {
    return (float2)tick * SECONDS_PER_TICK;
}

float ToNSecondPeriod(uint tick, float period) {
    uint multiplier = (uint)(SECONDS_PER_DAY / period);
    uint nSecondCounter = tick * multiplier;
    return (float)nSecondCounter * INV_UINT_PERIOD;
}

float2 ToNSecondPeriod(uint2 tick, float period) {
    uint multiplier = (uint)(SECONDS_PER_DAY / period);
    uint2 nSecondCounter = tick * multiplier;
    return (float2)nSecondCounter * INV_UINT_PERIOD;
}

float UintToRadian(uint val) {
    return val * UINT_TO_2PI;
}

float2 UintToRadian(uint2 val) {
    return (float2)val * UINT_TO_2PI;
}

#endif // TICK_CONVERSION_HLSL
