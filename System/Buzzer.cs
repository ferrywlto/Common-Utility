using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace System
{
    public class Buzzer
    {
        int frequency, duration, beepCount, currentCount;

        public Buzzer(int frequency, int duration, int count)
        {
            this.frequency = frequency;
            this.duration = duration;
            this.beepCount = count;
        }

        public void on()
        {
            currentCount = beepCount;
            Task t = new Task(() => beep());
            t.Start();
        }

        public void off() { currentCount = 0; }

        void beep()
        {
            while (currentCount >= 0)
            {
                if (frequency > 37 && frequency < 32767 && duration > 0) Console.Beep(frequency, duration); else Console.Beep();
                Thread.Sleep(100);
                currentCount--;
            }
        }

        //Below are for convenience setup
        private static Buzzer Beeper;
        public static int 
            Frequency = 3000,
            Duration = 1000,
            Count = 30;

        public static Buzzer CloneDefault()
        {
            return new Buzzer(Frequency, Duration, Count);
        }
        public static void Config(int frequency, int duration, int count)
        {
            Frequency = frequency;
            Duration = duration;
            Count = count;
        }
        public static void Beep()
        {
            if (Beeper == null) 
                Beeper = new Buzzer(Frequency, Duration, Count);
            Beeper.on();
        }
        public static void Stop()
        {
            if (Beeper != null)
                Beeper.off();
        }
    }
}
