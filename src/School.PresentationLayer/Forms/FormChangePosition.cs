using System;
using System.Windows.Forms;
using School.DataAccessLayer.Models;
using School.DataAccessLayer.Repository;

namespace School.PresentationLayer.Forms
{
    public partial class FormChangePosition : Form
    {
        private readonly FormMain _formMain;
        private readonly Position _position;
        private readonly IRepository<Position> _repositoryPosition;

        public FormChangePosition(FormMain formMain, Position position,
            IRepository<Position> repositoryPosition)
        {
            InitializeComponent();
            _formMain = formMain;
            _position = position;
            _repositoryPosition = repositoryPosition;
        }

        private void FormChangePosition_Load(object sender, EventArgs e)
        {
            textBox1.Text = _position.Name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var position = new Position
            {
                Id = _position.Id,
                Name = textBox1.Text,
            };

            _repositoryPosition.Update(position);
            _formMain.button2_Click(null, null);
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
