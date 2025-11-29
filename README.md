# High-Precision Trigonometry via Integer Overflow

## Overview

This project implements a high-precision time management system using the integer overflow method. It solves the precision degradation problem that occurs with float-based time management after long periods by leveraging the overflow characteristics of uint (32-bit unsigned integer).

## Problem: Precision Degradation in Float-Based Time Management

Traditional time management using float types suffers from the following issues:

### Causes of Precision Degradation

1. **Floating-Point Precision Limits**
   - Float types are 32-bit and have only about 7 significant digits
   - With large values (e.g., seconds after several hours), addition of small values loses precision
   - Example: In calculations like `100000.0f + 0.001f`, lower digits may be truncated

2. **Accumulated Error**
   - Errors accumulate over time
   - In long-running applications, time precision gradually degrades

3. **Shader Issues**
   - Float operations on GPU are particularly prone to precision loss after long periods
   - This produces inaccurate results in periodic animations and time-based calculations

## Solution: Integer Overflow Method

### Basic Principle

We utilize the overflow characteristics of uint (32-bit unsigned integer) to achieve periodic time management:

1. **Using uint's maximum value (2^32 = 4,294,967,296) as the period**
   - Map this value to one day (86400 seconds) or one hour (3600 seconds)
   - Day mode: `TICKS_PER_SECOND = 4,294,967,296 / 86,400 ≈ 49,710`
   - Hour mode: `TICKS_PER_SECOND = 4,294,967,296 / 3,600 ≈ 1,193,047`

2. **Automatic Reset via Overflow**
   - When the tick value exceeds uint's maximum, it automatically wraps to 0
   - This naturally achieves periodic time management
   - No need for explicit modulo operations (`%`)

3. **Precision Preservation through Integer Arithmetic**
   - Integer addition and subtraction are always exact
   - Precision is maintained regardless of elapsed time
   - Convert to float only when necessary

### Implementation Features

#### Script Side (TickUpdater.cs)

```csharp
// Map one day or one hour to the full uint range
double ticksPerSecond = period == TickPeriod.Day 
    ? UINT_PERIOD / SECONDS_PER_DAY 
    : UINT_PERIOD / SECONDS_PER_HOUR;

// Convert time to tick (automatically periodic via overflow)
uint tick = (uint)(elapsedSeconds * ticksPerSecond % UINT_PERIOD);
```

#### Shader Side (TickConversion.hlsl)

```hlsl
// Convert tick to seconds (convert to float only when needed)
float TickToSeconds(uint tick) {
    float secondsPerTick = _TickPeriod == TICK_PERIOD_DAY 
        ? SECONDS_PER_TICK_DAY 
        : SECONDS_PER_TICK_HOUR;
    return (float)tick * secondsPerTick;
}
```

### Advantages

1. **Precision Preservation**
   - Integer arithmetic maintains precision regardless of elapsed time
   - Conversion to float occurs only when actually used

2. **Performance**
   - Integer operations are faster than floating-point operations
   - Efficient operation on GPU

3. **Simplified Periodic Processing**
   - Automatic reset via overflow eliminates the need for explicit modulo operations
   - Code becomes simpler

4. **Long-Running Support**
   - Precision is maintained even after days or weeks
   - No accumulated errors occur

## Usage Example

Example usage in shaders:

```hlsl
// Add offset to global tick
uint tick = _GTick + SecondsToTick(_SimTimeOffset);

// Convert to seconds when needed
float time = TickToSeconds(tick);

// Use in trigonometric functions, etc.
float sine = sin(time * TWO_PI);
```

## Configuration

You can select the period using the `Period` property of the `TickUpdater` component:

- **Day**: Use one day (86400 seconds) as the period
- **Hour**: Use one hour (3600 seconds) as the period

Tick calculation and shader-side processing are automatically adjusted according to the selected period.
