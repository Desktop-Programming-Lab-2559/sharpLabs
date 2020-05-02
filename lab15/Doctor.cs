using System;
using System.Threading;
using System.Threading.Tasks;

namespace lab15
{
    public class Doctor : Human
    {
        private readonly Hospital _hospital;
        private readonly int _number;

        public Doctor(Hospital hospital, int number)
        {
            _hospital = hospital;
            _number = number;
        }

        public async void WorkAsync()
        {
            await Task.Run(Work);
        }

        private void Work()
        {
            var rnd = new Random(_number);
            while (!_hospital.IsDayOver)
            {
                if (!_hospital.IsHasNewPatient)
                {
                    Thread.Sleep(10);
                    continue;
                }

                var patient = _hospital.BeginInspection();
                var t = DateTime.Now;
                Thread.Sleep(rnd.Next(1, _hospital.Timing + 1) * Hospital.MagicTiming);
                if (rnd.Next(10) < 5)
                    Thread.Sleep(rnd.Next(1, _hospital.Timing + 1) * Hospital.MagicTiming);
                var time = DateTime.Now - t;
                _hospital.EndInspection($"Doctor {_number} has ended inspection. Common time {time.TotalMilliseconds}ms\n");
            }
        }
    }
}