using System;
using System.Drawing; // Для роботи з координатами (Point, Size) та кольорами.
using System.Windows.Forms; // Основний простір імен WinForms.
using Tracker.Core; // Підключаємо нашу бізнес-логіку.

namespace Tracker.Desktop
{
    // Головна форма програми, наслідується від стандартного класу Form.
    // Додаємо 'partial', щоб об'єднати цей файл із дизайнерським [cite: 881]
    public partial class MainDashboard : Form
    {
        // Поле для екземпляра нашого сервісу журналу (Dependency).
        private readonly AcademicJournal _journalService;

        // Оголошення елементів керування (Controls) інтерфейсу.
        private Label _lblHeader = null!;
        private GroupBox _grpInput = null!;
        private TextBox _txtName = null!;
        private TextBox _txtEmail = null!;
        private NumericUpDown _numGrade = null!;
        private Button _btnSubmit = null!;
        private ListBox _lstResults = null!;
        private StatusStrip _statusStrip = null!;
        private ToolStripStatusLabel _lblAverageStatus = null!;

        // Конструктор форми.
        public MainDashboard()
        {

            InitializeComponent();

            // Ініціалізація бізнес-логіки.
            _journalService = new AcademicJournal();
            

            // Налаштування властивостей самої форми.
            this.Text = "Academic Tracker Pro"; // Заголовок вікна
            this.Size = new Size(1280, 720);     // Розмір вікна
            this.StartPosition = FormStartPosition.CenterScreen; // Відцентрувати при запуску
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Заборонити зміну розміру (спрощує верстку)
            this.MaximizeBox = false; // Прибрати кнопку "Розгорнути"

            // Виклик методу побудови UI (замість InitializeComponent дизайнера).
            BuildUserInterface();
        }

        // Метод, де ми програмно створюємо та розміщуємо всі кнопки та поля.
        private void BuildUserInterface()
        {
            // 1. Створюємо стильний заголовок.
            _lblHeader = new Label
            {
                Text = "Academic Performance Journal",
                Font = new Font("Segoe UI", 16, FontStyle.Bold), // Сучасний шрифт
                ForeColor = Color.DarkSlateBlue,
                AutoSize = true,
                Location = new Point(15, 15)
            };
            this.Controls.Add(_lblHeader); // Додаємо на форму

            // 2. Група для вводу даних (GroupBox) - для візуального порядку.
            _grpInput = new GroupBox
            {
                Text = "New Entry Details",
                Location = new Point(15, 60),
                Size = new Size(450, 180),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(_grpInput);

            // -- Внутрішні елементи GroupBox --

            // Лейбл та поле для Ім'я
            var lblName = new Label { Text = "Full Name:", Location = new Point(20, 30), AutoSize = true };
            _txtName = new TextBox { Location = new Point(150, 28), Width = 280 };

            // Лейбл та поле для Email
            var lblEmail = new Label { Text = "Email:", Location = new Point(20, 70), AutoSize = true };
            _txtEmail = new TextBox { Location = new Point(150, 68), Width = 280 };

            // Лейбл та поле для Оцінки
            var lblGrade = new Label { Text = "Score (0-100):", Location = new Point(20, 110), AutoSize = true };
            // Використовуємо NumericUpDown замість TextBox, щоб користувач не міг ввести текст.
            _numGrade = new NumericUpDown
            {
                Location = new Point(150, 108),
                Width = 100,
                Minimum = 0,
                Maximum = 100
            };

            // Додаємо елементи в GroupBox (а не на форму!)
            _grpInput.Controls.AddRange(new Control[] { lblName, _txtName, lblEmail, _txtEmail, lblGrade, _numGrade });

            // 3. Кнопка збереження.
            _btnSubmit = new Button
            {
                Text = "Submit Grade",
                Location = new Point(340, 250),
                Size = new Size(125, 40),
                BackColor = Color.CornflowerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, // Плоский стиль (Modern UI)
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            // Підписуємось на подію Click (Event Subscription).
            _btnSubmit.Click += OnSubmitClicked;
            this.Controls.Add(_btnSubmit);

            // 4. Список студентів (ListBox).
            _lstResults = new ListBox
            {
                Location = new Point(15, 310),
                Size = new Size(450, 200),
                Font = new Font("Consolas", 10) // Моноширинний шрифт для кращої читабельності даних
            };
            this.Controls.Add(_lstResults);

            // 5. Рядок стану (StatusStrip) для виводу середнього балу.
            _statusStrip = new StatusStrip();
            _lblAverageStatus = new ToolStripStatusLabel { Text = "Class Average Score: 0.00" };
            _statusStrip.Items.Add(_lblAverageStatus);
            this.Controls.Add(_statusStrip);
        }

        // Обробник події натискання кнопки (Event Handler).
        private void OnSubmitClicked(object? sender, EventArgs e)
        {
            try
            {
                // Зчитуємо дані з полів вводу.
                string name = _txtName.Text.Trim(); // Trim() видаляє зайві пробіли.
                string email = _txtEmail.Text.Trim();

                // Explicit casting: перетворюємо decimal (з NumericUpDown) в int.
                int grade = (int)_numGrade.Value;

                // Валідація на рівні UI: перевіряємо, чи заповнені текстові поля.
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Please fill in all required fields!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Перериваємо виконання методу.
                }

                // Створення об'єкта бізнес-логіки.
                Learner newLearner = new Learner(name, email);

                // Виклик методу сервісу для додавання даних.
                _journalService.RegisterGrade(newLearner, grade);

                // Оновлення інтерфейсу (Data Binding вручну).
                RefreshDataList();

                // Очищення полів для нового вводу (UX).
                _txtName.Clear();
                _txtEmail.Clear();
                _numGrade.Value = 0;

                // Повертаємо фокус на поле імені для швидкого вводу наступного.
                _txtName.Focus();
            }
            catch (Exception ex) // Перехоплення помилок (Error Handling).
            {
                // Виводимо повідомлення про помилку користувачу.
                MessageBox.Show($"An error occurred: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для оновлення відображення даних.
        private void RefreshDataList()
        {
            // Зупиняємо перемальовування списку для продуктивності.
            _lstResults.BeginUpdate();
            _lstResults.Items.Clear(); // Очищаємо старі дані.

            // Отримуємо актуальні дані з сервісу.
            var entries = _journalService.GetEntries();

            // Проходимо по словнику (KeyValuePair) і додаємо рядки в ListBox.
            foreach (var entry in entries)
            {
                // Форматуємо рядок виводу.
                string displayLine = $"{entry.Key.FullName} [{entry.Key.Email}] — Score: {entry.Value}";
                _lstResults.Items.Add(displayLine);
            }

            // Розраховуємо та оновлюємо середній бал у статус-барі.
            double avg = _journalService.CalculateGroupAverage();
            _lblAverageStatus.Text = $"Class Average Score: {avg:F2}";

            // Відновлюємо перемальовування.
            _lstResults.EndUpdate();
        }
    }
}