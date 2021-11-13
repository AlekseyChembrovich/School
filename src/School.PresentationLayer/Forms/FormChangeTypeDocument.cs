using System;
using System.Linq;
using System.Windows.Forms;
using School.DataAccessLayer.Models;
using School.DataAccessLayer.Repository;
using School.PresentationLayer.Tools;

namespace School.PresentationLayer.Forms
{
    public partial class FormChangeTypeDocument : Form
    {
        private readonly FormMain _formMain;
        private readonly TypeDocument _typeDocument;
        private readonly IRepository<TypeDocument> _repositoryTypeDocument;
        private readonly IRepository<Position> _repositoryPosition;

        public FormChangeTypeDocument(FormMain formMain, TypeDocument typeDocument,
            IRepository<TypeDocument> repositoryTypeDocument, IRepository<Position> repositoryPosition)
        {
            InitializeComponent();
            _formMain = formMain;
            _typeDocument = typeDocument;
            _repositoryTypeDocument = repositoryTypeDocument;
            _repositoryPosition = repositoryPosition;
        }

        private void FormChangeTypeDocument_Load(object sender, EventArgs e)
        {
            this.FillCombobox(comboBox1, _repositoryPosition.GetAll().Select(x => x.Name).ToArray());
            textBox1.Text = _typeDocument.Name;
            var position = _repositoryPosition.GetById(_typeDocument.PositionId);
            comboBox1.SelectedItem = position.Name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var position = _repositoryPosition.GetModelByProperty(comboBox1.SelectedItem.ToString(), "Name");
            var typeDocument = new TypeDocument
            {
                Id = _typeDocument.Id,
                Name = textBox1.Text,
                PositionId = position.Id
            };

            _repositoryTypeDocument.Update(typeDocument);
            _formMain.button4_Click(null, null);
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
