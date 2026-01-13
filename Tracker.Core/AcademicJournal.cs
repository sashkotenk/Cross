using System;
using System.Collections.Generic; // Підключаємо простір імен для роботи з колекціями (List, Dictionary).
using System.Linq; // Підключаємо LINQ для зручних операцій над даними (Average, Select тощо).

namespace Tracker.Core
{
    // Клас, що відповідає за бізнес-логіку ведення журналу оцінок.
    public class AcademicJournal
    {
        // Приватне поле для зберігання даних. 
        // Використовуємо Dictionary, де ключ - це об'єкт Learner, а значення - оцінка (int).
        // Це забезпечує унікальність студентів (не можна додати одного студента двічі з різними оцінками).
        private readonly Dictionary<Learner, int> _journalEntries;

        // Конструктор без параметрів.
        public AcademicJournal()
        {
            // Ініціалізуємо словник, щоб уникнути NullReferenceException при спробі додати дані.
            _journalEntries = new Dictionary<Learner, int>();
        }

        // Метод для реєстрації оцінки студента.
        // Приймає об'єкт студента та ціле число (оцінку).
        public void RegisterGrade(Learner student, int score)
        {
            // Валідація вхідних даних (Design by Contract).
            // Перевіряємо, чи оцінка входить у допустимий діапазон (0-100).
            if (score < 0 || score > 100)
            {
                // Якщо ні - викидаємо виключення з поясненням. Це перехопить UI шар.
                throw new ArgumentException("Оцінка має бути в діапазоні від 0 до 100.");
            }

            // Перевіряємо, чи студент дорівнює null, щоб не "впасти".
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student), "Дані студента не можуть бути пустими.");
            }

            // Логіка оновлення або додавання (Upsert).
            if (_journalEntries.ContainsKey(student))
            {
                // Якщо студент вже є, оновлюємо його оцінку.
                _journalEntries[student] = score;
            }
            else
            {
                // Якщо немає, додаємо новий запис у словник.
                _journalEntries.Add(student, score);
            }
        }

        // Метод для обчислення середнього арифметичного (бізнес-метрика).
        public double CalculateGroupAverage()
        {
            // Якщо журнал порожній, повертаємо 0, щоб уникнути ділення на нуль.
            if (_journalEntries.Count == 0) return 0.0;

            // Використовуємо метод розширення .Average() з LINQ для колекції значень (Values).
            // Це набагато чистіше, ніж писати цикл foreach.
            return _journalEntries.Values.Average();
        }

        // Метод для отримання копії даних для UI.
        // Повертаємо Dictionary, щоб зовнішній код міг читати дані, але не змінював приватне поле _journalEntries.
        public Dictionary<Learner, int> GetEntries()
        {
            // Повертаємо посилання на словник.
            return _journalEntries;
        }
    }
}