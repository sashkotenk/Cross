using Xunit; // Бібліотека для тестування.
using Tracker.Core; // Підключаємо тестовану бібліотеку.
using System;

namespace Tracker.Tests
{
    public class JournalTests
    {
        // Атрибут [Fact] позначає метод як тест.
        [Fact]
        public void CalculateGroupAverage_ShouldReturnCorrectMath()
        {
            // Arrange (Підготовка): створюємо об'єкти та умови.
            var journal = new AcademicJournal();
            var s1 = new Learner("Студент А", "a@test.com");
            var s2 = new Learner("Студент Б", "b@test.com");

            // Act (Дія): виконуємо методи, які тестуємо.
            journal.RegisterGrade(s1, 80);
            journal.RegisterGrade(s2, 100);
            double average = journal.CalculateGroupAverage();

            // Assert (Перевірка): порівнюємо очікуваний результат з реальним.
            // (80 + 100) / 2 = 90.
            Assert.Equal(90.0, average);
        }

        [Fact]
        public void RegisterGrade_NegativeScore_ShouldThrowException()
        {
            // Arrange
            var journal = new AcademicJournal();
            var student = new Learner("Тестер", "test@test.com");

            // Act & Assert
            // Ми очікуємо, що код викине помилку ArgumentException при спробі додати -10.
            // Це тест на "стійкість" системи.
            Assert.Throws<ArgumentException>(() => journal.RegisterGrade(student, -10));
        }
    }
}