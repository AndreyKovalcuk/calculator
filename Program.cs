using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Calculator
{
    public partial class CalculatorForm : Form
    {
        private double resultValue = 0;
        private string operationPerformed = "";
        private bool isOperationPerformed = false;
        private string currentExpression = "";

        public CalculatorForm()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(45, 45, 48);
        }

        private void InitializeComponent()
        {
            this.Text = "Калькулятор";
            this.Size = new System.Drawing.Size(400, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Создаем текстовое поле для отображения выражения
            TextBox expressionTextBox = new TextBox();
            expressionTextBox.Name = "expressionTextBox";
            expressionTextBox.Size = new System.Drawing.Size(340, 30);
            expressionTextBox.Location = new System.Drawing.Point(30, 30);
            expressionTextBox.Font = new System.Drawing.Font("Segoe UI", 16);
            expressionTextBox.TextAlign = HorizontalAlignment.Right;
            expressionTextBox.ReadOnly = true;
            expressionTextBox.BackColor = Color.FromArgb(30, 30, 30);
            expressionTextBox.ForeColor = Color.FromArgb(150, 150, 150);
            expressionTextBox.BorderStyle = BorderStyle.None;
            this.Controls.Add(expressionTextBox);

            // Создаем текстовое поле для отображения результата
            TextBox displayTextBox = new TextBox();
            displayTextBox.Name = "displayTextBox";
            displayTextBox.Size = new System.Drawing.Size(340, 50);
            displayTextBox.Location = new System.Drawing.Point(30, 60);
            displayTextBox.Font = new System.Drawing.Font("Segoe UI", 24);
            displayTextBox.TextAlign = HorizontalAlignment.Right;
            displayTextBox.ReadOnly = true;
            displayTextBox.BackColor = Color.FromArgb(30, 30, 30);
            displayTextBox.ForeColor = Color.White;
            displayTextBox.BorderStyle = BorderStyle.None;
            this.Controls.Add(displayTextBox);

            // Создаем кнопки
            string[,] buttonTexts = {
                {"7", "8", "9", "/"},
                {"4", "5", "6", "*"},
                {"1", "2", "3", "-"},
                {"0", ".", "=", "+"}
            };

            // Размеры и отступы для центрирования
            int buttonSize = 80;
            int buttonSpacing = 10;
            int totalWidth = (buttonSize * 4) + (buttonSpacing * 3);
            int totalHeight = (buttonSize * 4) + (buttonSpacing * 3);
            int startX = (this.ClientSize.Width - totalWidth) / 2;
            int startY = 130;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    CustomButton button = new CustomButton();
                    button.Text = buttonTexts[i, j];
                    button.Size = new System.Drawing.Size(buttonSize, buttonSize);
                    button.Location = new System.Drawing.Point(
                        startX + j * (buttonSize + buttonSpacing),
                        startY + i * (buttonSize + buttonSpacing)
                    );
                    button.Font = new System.Drawing.Font("Segoe UI", 20);
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;

                    // Настройка цветов кнопок
                    if (button.Text == "=")
                    {
                        button.BackColor = Color.FromArgb(0, 122, 204);
                        button.ForeColor = Color.White;
                    }
                    else if ("+-*/".Contains(button.Text))
                    {
                        button.BackColor = Color.FromArgb(60, 60, 65);
                        button.ForeColor = Color.White;
                    }
                    else
                    {
                        button.BackColor = Color.FromArgb(45, 45, 48);
                        button.ForeColor = Color.White;
                    }

                    if (button.Text == "=")
                    {
                        button.Click += (s, e) =>
                        {
                            if (currentExpression != "" && displayTextBox.Text != "")
                            {
                                currentExpression += displayTextBox.Text;
                                expressionTextBox.Text = currentExpression;
                                try
                                {
                                    System.Data.DataTable dt = new System.Data.DataTable();
                                    var result = dt.Compute(currentExpression, "");
                                    displayTextBox.Text = result.ToString();
                                    currentExpression = result.ToString();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Ошибка в выражении!");
                                    currentExpression = "";
                                    displayTextBox.Text = "";
                                }
                            }
                        };
                    }
                    else if ("+-*/".Contains(button.Text))
                    {
                        button.Click += (s, e) =>
                        {
                            if (displayTextBox.Text != "")
                            {
                                if (currentExpression == "")
                                {
                                    currentExpression = displayTextBox.Text;
                                }
                                else
                                {
                                    currentExpression += displayTextBox.Text;
                                }
                                currentExpression += " " + button.Text + " ";
                                expressionTextBox.Text = currentExpression;
                                isOperationPerformed = true;
                            }
                        };
                    }
                    else
                    {
                        button.Click += (s, e) =>
                        {
                            if (isOperationPerformed)
                            {
                                displayTextBox.Clear();
                                isOperationPerformed = false;
                            }
                            displayTextBox.Text += button.Text;
                        };
                    }

                    this.Controls.Add(button);
                }
            }

            // Добавляем кнопку очистки
            CustomButton clearButton = new CustomButton();
            clearButton.Text = "C";
            clearButton.Size = new System.Drawing.Size(totalWidth, 60);
            clearButton.Location = new System.Drawing.Point(startX, startY + totalHeight + 20);
            clearButton.Font = new System.Drawing.Font("Segoe UI", 20);
            clearButton.BackColor = Color.FromArgb(60, 60, 65);
            clearButton.ForeColor = Color.White;
            clearButton.FlatStyle = FlatStyle.Flat;
            clearButton.FlatAppearance.BorderSize = 0;
            clearButton.Click += (s, e) =>
            {
                displayTextBox.Text = "";
                expressionTextBox.Text = "";
                currentExpression = "";
                resultValue = 0;
                operationPerformed = "";
                isOperationPerformed = false;
            };
            this.Controls.Add(clearButton);
        }
    }

    public class CustomButton : Button
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Рисуем градиентный фон
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                this.BackColor,
                ControlPaint.Light(this.BackColor),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            // Рисуем текст
            TextRenderer.DrawText(g, this.Text, this.Font, this.ClientRectangle, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CalculatorForm());
        }
    }
} 