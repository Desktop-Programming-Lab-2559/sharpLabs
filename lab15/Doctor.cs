using System;
using System.Threading;
using System.Threading.Tasks;

namespace lab15
{
    public class Doctor : Human
    {
        private readonly Hospital _hospital;
        private readonly int _number;
        private readonly Random rnd;
        public bool IsWork { get; set; }

        public Doctor(Hospital hospital, int number)
        {
            _hospital = hospital;
            _number = number;
            rnd = new Random(_number);
        }

        public async void WorkAsync()
        {
            await Task.Run(Work);
        }

        private void Work()
        {
            while (!_hospital.IsDayOver)
            {
                if (!_hospital.IsHasNewPatient || IsWork)
                {
                    Thread.Sleep(10);
                    continue;
                }

                IsWork = true;
                var patient = _hospital.BeginInspection();
                var t = DateTime.Now;
                Inspection();
                Consulting();
                var time = DateTime.Now - t;
                IsWork = false;
                _hospital.EndInspection($"Doctor {_number} has ended inspection. Common time {time.TotalMilliseconds}ms\n");
            }
        }

        private void Inspection()
        {
            Thread.Sleep(rnd.Next(1, _hospital.Timing + 1) * Hospital.MagicTiming);
        }

        private void Consulting()
        {
            if (rnd.Next(10) >= 5) return;
            var docNum = _hospital.GetDoctorForConsulting();
            _hospital.StandardLogger($"Doctor {_number} is consulting with doctor {docNum}\n");
            Thread.Sleep(rnd.Next(1, _hospital.Timing + 1) * Hospital.MagicTiming);
            _hospital.FreeDoctor(docNum);
        }
    }
}