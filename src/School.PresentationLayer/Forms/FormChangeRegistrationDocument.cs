using System;
using System.Linq;
using System.Windows.Forms;
using School.DataAccessLayer.Models;
using School.PresentationLayer.Tools;
using School.DataAccessLayer.Repository;

namespace School.PresentationLayer.Forms
{
    public partial class FormChangeRegistrationDocument : Form
    {
        private readonly FormMain _formMain;
        private readonly RegistrationDocument _registrationDocument;
        private readonly IRepository<RegistrationDocument> _repositoryRegistrationDocument;
        private readonly IRepository<TypeDocument> _repositoryTypeDocument;
        private readonly IRepository<Employee> _repositoryEmployee;

        public FormChangeRegistrationDocument(FormMain formMain, RegistrationDocument registrationDocument,
            IRepository<RegistrationDocument> repositoryRegistrationDocument,
            IRepository<TypeDocument> repositoryTypeDocument, IRepository<Employee> repositoryEmployee)
        {
            InitializeComponent();
            _formMain = formMain;
            _registrationDocument = registrationDocument;
            _repositoryRegistrationDocument = repositoryRegistrationDocument;
            _repositoryTypeDocument = repositoryTypeDocument;
            _repositoryEmployee = repositoryEmployee;
        }

        private void FormChangeRegistrationDocument_Load(object sender, EventArgs e)
        {
            this.FillCombobox(comboBox2, _repositoryTypeDocument.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox3, _repositoryEmployee.GetAll().Select(x => x.Surname).ToArray());
            this.FillCombobox(comboBox4, _repositoryEmployee.GetAll().Select(x => x.Surname).ToArray());
            dateTimePicker1.Value = _registrationDocument.CreateDate.Date;
            dateTimePicker2.Value = _registrationDocument.CreateDate.Date;
            comboBox1.SelectedItem = _registrationDocument.DirectionDocument;
            var typeDocument = _repositoryTypeDocument.GetById(_registrationDocument.TypeDocumentId);
            comboBox2.SelectedItem = typeDocument.Name;
            var creator = _repositoryEmployee.GetById(_registrationDocument.EmployeeCreatorId);
            comboBox3.SelectedItem = creator.Surname;
            var approver = _repositoryEmployee.GetById(_registrationDocument.EmployeeApproverId);
            comboBox4.SelectedItem = approver.Surname;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || 
                comboBox3.SelectedIndex == -1 || comboBox4.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var typeDocument = _repositoryTypeDocument.GetModelByProperty(comboBox2.SelectedItem.ToString(), "Name");
            var creator = _repositoryEmployee.GetModelByProperty(comboBox3.SelectedItem.ToString(), "Surname");
            var approver = _repositoryEmployee.GetModelByProperty(comboBox4.SelectedItem.ToString(), "Surname");
            var registrationDocument = new RegistrationDocument
            {
                Id = _registrationDocument.Id,
                CreateDate = dateTimePicker1.Value.Date,
                StartDate = dateTimePicker2.Value.Date,
                DirectionDocument = comboBox1.SelectedItem.ToString(),
                TypeDocumentId = typeDocument.Id,
                EmployeeCreatorId = creator.Id,
                EmployeeApproverId = approver.Id,
            };

            _repositoryRegistrationDocument.Update(registrationDocument);
            _formMain.button3_Click(null, null);
            this.Close();
        }
    }
}
