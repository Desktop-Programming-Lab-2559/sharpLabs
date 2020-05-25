﻿// Инфекционное отделение поликлиники имеет смотровую на 𝑁 человек и 𝑀 докторов, которые готовы оказать помощь
// заболевшим или проконсультировать не заболевших людей. По правилам инфекционного отделения в смотровую может зайти
// любой человек, если в ней есть свободное место или в нее может зайти не заболевший человек, если в смотровой нет
// заболевших и наоборот: при наличии свободных мест заболевший человек может войти в смотровую, если там только
// заболевшие. Как только человек вошел в смотровую он занимает свое место и ожидает доктора. Незанятый доктор выбирает
// пациента, пришедшего раньше других, и проводит прием, который длиться от 1 до T временных единиц. В особых случаях,
// доктор может попросить у другого доктора помощи, которая так же может длиться от 1 до T временных единиц.
// Если все места в смотровой заняты, то пришедшие пациенты встают в очередь. При этом в очереди спустя некоторое
// время, при наличии нездорового человека, заражается вся очередь. Количество пациентов и интервал их появления
// произволен и случаен. Напишите программу моделирующую работу инфекционного отделения с использованием средств
// синхронизации потоков .net framework. Ваша программа должна вести всю историю работы инфекционного отделения:
// пришедшие пациенты и их состояние, время работы докторов и время оказания помощи пациентам.  Обязательно сохранять
// информацию о новых заболевших в очереди. Продемонстрируйте работу вашей программы при различных значениях
// параметров. Подберите параметры так, чтоб показать особые случаи, которые могут возникнуть в инфекционном отделении.
// Срок сдачи до 30 мая. 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace lab15
{
    public class Hospital : IDisposable
    {
        public delegate void LoggerHandler(string text);

        private readonly ConcurrentDictionary<int, Doctor> _doctors;
        private readonly ConcurrentQueue<Patient> _examiningRoom;
        private readonly ConcurrentDictionary<int, Patient> _queue;
        private readonly Random rnd = new Random();

        public Hospital(int doctorsCount, int examiningRoomSize, int timing, string logFile = "")
        {
            ExaminingRoomSize = examiningRoomSize;
            Timing = timing;
            LogFile = File.CreateText(logFile);
            DoctorsCount = doctorsCount;
            IsDayOver = false;

            _doctors = new ConcurrentDictionary<int, Doctor>();
            for (var i = 0; i < doctorsCount; i++) _doctors.GetOrAdd(i, new Doctor(this, i));

            _queue = new ConcurrentDictionary<int, Patient>();
            _examiningRoom = new ConcurrentQueue<Patient>();

            if (logFile != "") LoggerEvent += StandardLogger;
        }

        public static int MagicTiming { get; } = 500;
        public static int ExaminingRoomSize { get; private set; } = 10;
        public int Timing { get; }
        private StreamWriter LogFile { get; }
        private int DoctorsCount { get; }
        public bool IsDayOver { get; private set; }
        public bool IsHasNewPatient => _examiningRoom.Count != 0;
        public bool IsHasInfectionInRoom => _examiningRoom.Any(x => x.IsInfected);

        public void Dispose()
        {
            IsDayOver = true;
            LogFile.Flush();
        }

        public event LoggerHandler LoggerEvent;

        public void StartSimulation()
        {
            StartNewPatientDemon();
            Thread.Sleep(10);
            StartStatusDemonAsync();
            StartQueueDemonAsync();
            StartInfectionDemonAsync();

            foreach (var doctor in _doctors) doctor.Value.WorkAsync();
        }

        private async void StartStatusDemonAsync()
        {
            await Task.Run(() =>
            {
                while (!IsDayOver)
                {
                    Thread.Sleep(MagicTiming);
                    PrintHospitalState();
                }
            });
        }

        private void PrintHospitalState()
        {
            Console.Clear();
            Console.WriteLine($"Всего докторов {DoctorsCount}");
            Console.WriteLine($"Пациентов в смотровой {_examiningRoom.Count} в очереди {_queue.Count} " +
                              $"Всего {_queue.Count + _examiningRoom.Count}");
            Console.WriteLine("Смотровая");
            foreach (var patient in _examiningRoom)
                if (patient.IsInfected)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("#");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.Write("#");
                }

            Console.WriteLine();
            Console.WriteLine("Очередь");
            foreach (var patient in _queue)
                if (patient.Value.IsInfected)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("#");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.Write("#");
                }
        }

        public Patient BeginInspection()
        {
            while (true)
            {
                if (_examiningRoom.Count == 0) continue;
                try
                {
                    if (!_examiningRoom.TryDequeue(out var p)) continue;
                    return p;
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        public void EndInspection(string text)
        {
            LoggerEvent?.Invoke(text);
        }

        // public Patient GetPatient(int patientId)
        // {
        //     return _examiningRoom[patientId];
        // }


        private async void StartQueueDemonAsync()
        {
            await Task.Run(QueueDemon);
        }

        private void QueueDemon()
        {
            while (!IsDayOver)
            {
                Thread.Sleep(Timing);
                if (_examiningRoom.Count >= ExaminingRoomSize || _queue.Count == 0) continue;

                int queueId;
                try
                {
                    // newId = _examiningRoom.Count == 0 ? 1 : _examiningRoom.Keys.Max() + 1;
                    if (!IsHasNewPatient)
                        queueId = _queue.Keys.Min();
                    else if (IsHasInfectionInRoom)
                        queueId = _queue.Where(pair => pair.Value.IsInfected).Select(pair => pair.Key).Min();
                    else
                        queueId = _queue.Where(pair => !pair.Value.IsInfected).Select(pair => pair.Key).Min();
                    // try
                    // {
                    //     queueId = _queue.Where(pair => !pair.Value.IsInfected).Select(pair => pair.Key).Min();
                    // }
                    // catch (InvalidOperationException e)
                    // {
                    //     if (_queue.Any(pair => !pair.Value.IsInfected)) Console.Beep();
                    //     throw e;
                    // }
                }
                catch (InvalidOperationException)
                {
                    continue;
                }

                Patient p;
                while (!_queue.TryRemove(queueId, out p))
                {
                }
                
                _examiningRoom.Enqueue(p);
                
            }
        }

        private async void StartInfectionDemonAsync()
        {
            await Task.Run(InfectionDemon);
        }

        private void InfectionDemon()
        {
            while (!IsDayOver)
            {
                Thread.Sleep(MagicTiming);
                var previous = new Patient();
                previous.IsInfected = false;
                var keys = _queue.Keys.ToArray();
                for (var i = 0; i < keys.Length; i++)
                    try
                    {
                        if (!_queue[keys[i]].IsInfected) continue;
                        if (i > 0)
                        {
                            _queue[keys[i - 1]].IsInfected = true;
                            LoggerEvent?.Invoke($"Infecting in queue patient {keys[i - 1]}\n");
                        }

                        if (i < keys.Length - 1)
                        {
                            _queue[keys[i + 1]].IsInfected = true;
                            LoggerEvent?.Invoke($"Infecting in queue patient {keys[i + 1]}\n");
                        }
                        i++;
                    }
                    catch (KeyNotFoundException)
                    {
                    }
            }
        }

        private async void StartNewPatientDemon()
        {
            await Task.Run(NewPatientDemon);
        }

        private void NewPatientDemon()
        {
            while (!IsDayOver)
            {
                var newId = _queue.Count == 0 ? 0 : _queue.Keys.Max() + 1;
                var p = new Patient();
                _queue[newId] = p;
                LoggerEvent?.Invoke($"New patient IsInfected {p.IsInfected}\n");
                Thread.Sleep(rnd.Next(MagicTiming));
            }
        }

        public void StandardLogger(string text)
        {
            LogFile.Write(text);
        }

        public int GetDoctorForConsulting()
        {
            while (true)
            {
                foreach (var doctor in _doctors)
                {
                    if (!doctor.Value.IsWork)
                    {
                        doctor.Value.IsWork = true;
                        return doctor.Key;
                    }
                }
            }
        }

        public void FreeDoctor(int docNum)
        {
            _doctors[docNum].IsWork = false;
        }
    }
}