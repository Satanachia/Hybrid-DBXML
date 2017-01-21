namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public class Stopwatch
    {
        private long elapsed;
        public static readonly long Frequency;
        public static readonly bool IsHighResolution;
        private bool isRunning;
        private long startTimeStamp;
        private static readonly double tickFrequency;

        static Stopwatch()
        {
            if (!QueryPerformanceFrequency(out Frequency))
            {
                IsHighResolution = false;
                Frequency = 0x989680L;
                tickFrequency = 1.0;
            }
            else
            {
                IsHighResolution = true;
                tickFrequency = 10000000.0;
                tickFrequency /= (double) Frequency;
            }
        }

        public Stopwatch()
        {
            this.Reset();
        }

        private long GetElapsedDateTimeTicks()
        {
            long rawElapsedTicks = this.GetRawElapsedTicks();
            if (IsHighResolution)
            {
                double num2 = rawElapsedTicks;
                num2 *= tickFrequency;
                return (long) num2;
            }
            return rawElapsedTicks;
        }

        public static long GetElapsedMilliseconds(long _startTime)
        {
            double num = GetTimestamp() - _startTime;
            num *= tickFrequency;
            return (long) num;
        }

        private long GetRawElapsedTicks()
        {
            long elapsed = this.elapsed;
            if (this.isRunning)
            {
                long num3 = GetTimestamp() - this.startTimeStamp;
                elapsed += num3;
            }
            return elapsed;
        }

        public static long GetTimestamp()
        {
            if (IsHighResolution)
            {
                long lpPerformanceCount = 0L;
                QueryPerformanceCounter(out lpPerformanceCount);
                return lpPerformanceCount;
            }
            return DateTime.UtcNow.Ticks;
        }

        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);
        public void Reset()
        {
            this.elapsed = 0L;
            this.isRunning = false;
            this.startTimeStamp = 0L;
        }

        public void Start()
        {
            if (!this.isRunning)
            {
                this.startTimeStamp = GetTimestamp();
                this.isRunning = true;
            }
        }

        public static Stopwatch StartNew()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }

        public void Stop()
        {
            if (this.isRunning)
            {
                long num2 = GetTimestamp() - this.startTimeStamp;
                this.elapsed += num2;
                this.isRunning = false;
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                return new TimeSpan(this.GetElapsedDateTimeTicks());
            }
        }

        public long ElapsedMilliseconds
        {
            get
            {
                return (this.GetElapsedDateTimeTicks() / 0x2710L);
            }
        }

        public long ElapsedTicks
        {
            get
            {
                return this.GetRawElapsedTicks();
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }
    }
}

