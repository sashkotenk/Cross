using System;

namespace Tracker.Core
{
    // Клас, що репрезентує сутність "Студент".
    // Використовуємо модифікатор public, щоб клас був доступний в інших збірках (проектах).
    public class Learner
    {
        // Властивість для унікального ідентифікатора. 
        // set; дозволяє змінювати ID, хоча в реальних БД це робить сама база.
        public Guid Id { get; set; }

        // Властивість для повного імені.
        public string FullName { get; set; }

        // Властивість для електронної пошти.
        public string Email { get; set; }

        // Конструктор класу для ініціалізації об'єкта.
        // Приймає ім'я та пошту, генерує новий унікальний ID.
        public Learner(string fullName, string email)
        {
            // Генеруємо новий GUID (Globally Unique Identifier), щоб уникнути колізій ID.
            Id = Guid.NewGuid();

            // Ініціалізуємо поля переданими значеннями.
            FullName = fullName;
            Email = email;
        }

        // Перевизначаємо метод ToString() базового класу Object.
        // Це потрібно для коректного відображення об'єкта в UI (наприклад, у ListBox).
        public override string ToString()
        {
            // Використовуємо інтерполяцію рядків ($) для зручного форматування.
            return $"{FullName} <{Email}>";
        }
    }
}