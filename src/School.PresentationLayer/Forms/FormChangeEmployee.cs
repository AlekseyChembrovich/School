using System;
using System.Linq;
using System.Windows.Forms;
using School.DataAccessLayer.Models;
using School.PresentationLayer.Tools;
using School.DataAccessLayer.Repository;

namespace School.PresentationLayer.Forms
{
    public partial class FormChangeEmployee : Form
    {
        private readonly FormMain _formMain;
        private readonly Employee _employee;
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly IRepository<Position> _repositoryPosition;

        public FormChangeEmployee(FormMain formMain, Employee employee,
            IRepository<Employee> repositoryEmployee, IRepository<Position> repositoryPosition)
        {
            InitializeComponent();
            _formMain = formMain;
            _employee = employee;
            _repositoryEmployee = repositoryEmployee;
            _repositoryPosition = repositoryPosition;
        }

        private void FormChangeEmployee_Load(object sender, EventArgs e)
        {
            this.FillCombobox(comboBox1, _repositoryPosition.GetAll().Select(x => x.Name).ToArray());
            textBox1.Text = _employee.Surname;
            textBox2.Text = _employee.Name;
            textBox3.Text = _employee.Patronymic;
            maskedTextBox1.Text = _employee.Phone;
            var position = _repositoryPosition.GetById(_employee.PositionId);
            comboBox1.SelectedItem = position.Name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || 
                string.IsNullOrWhiteSpace(textBox3.Text) || maskedTextBox1.Text.Trim(' ').Length < 18 ||
                comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var position = _repositoryPosition.GetModelByProperty(comboBox1.SelectedItem.ToString(), "Name");
            var employee = new Employee
            {
                Id = _employee.Id,
                Surname = textBox1.Text,
                Name = textBox2.Text,
                Patronymic = textBox3.Text,
                Phone = maskedTextBox1.Text,
                PositionId = position.Id
            };

            _repositoryEmployee.Update(employee);
            _formMain.button1_Click(null, null);
            this.Close();
        }

        private void EnterOnlyLetter(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }
    }
}
