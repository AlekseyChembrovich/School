using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using School.DataAccessLayer.Models;
using School.PresentationLayer.Tools;
using School.PresentationLayer.Forms;
using School.DataAccessLayer.Repository;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using School.DataAccessLayer.Repository.EntityFramework;

namespace School.PresentationLayer
{
    public partial class FormMain : Form
    {
        public CurrentTable CurrentTable = CurrentTable.Employee;
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly IRepository<Position> _repositoryPosition;
        private readonly IRepository<RegistrationDocument> _repositoryRegistrationDocument;
        private readonly IRepository<TypeDocument> _repositoryTypeDocument;
        private readonly List<Panel> _addPanels;
        private readonly List<Button> _tablesButtons;

        public FormMain()
        {
            InitializeComponent();
            var databaseContext = new DatabaseContext();

            #region Init repositoties

            _repositoryEmployee = new RepositoryEmployee(databaseContext);
            _repositoryPosition = new BaseRepository<Position>(databaseContext);
            _repositoryRegistrationDocument = new RepositoryRegistrationDocument(databaseContext);
            _repositoryTypeDocument = new RepositoryTypeDocument(databaseContext);

            #endregion

            _addPanels = new List<Panel> { panel4, panel5, panel6, panel7 };
            _tablesButtons = new List<Button> { button1, button2, button3, button4 };
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            UpdateCombobox();
            button1_Click(null, null);
        }

        private void PickOutButtonCurrentTable(CurrentTable currentTable)
        {
            var pickOut = Color.FromArgb(52, 74, 100);
            var normal = Color.FromArgb(52, 74, 121);
            var index = (int)currentTable;
            _tablesButtons[index].BackColor = pickOut;
            _tablesButtons.Except(new[] { _tablesButtons[index] }).ToList().ForEach(x =>
            {
                x.BackColor = normal;
            });
        }

        private void UpdateCombobox()
        {
            this.FillCombobox(comboBox1, _repositoryPosition.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox2, _repositoryEmployee.GetAll().Select(x => x.Surname).ToArray());
            this.FillCombobox(comboBox3, _repositoryEmployee.GetAll().Select(x => x.Surname).ToArray());
            this.FillCombobox(comboBox4, _repositoryPosition.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox5, _repositoryTypeDocument.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox8, _repositoryTypeDocument.GetAll().Select(x => x.Name).ToArray());
        }

        private void ToggleFiltrationMenu(bool isShow)
        {
            panel11.Visible = isShow;
            panel11.Dock = isShow ? DockStyle.Bottom : DockStyle.None;
            listView1.Dock = !isShow ? DockStyle.Bottom : DockStyle.None;
            var sizeNormal = new Size(640, 186);
            if (!isShow)
            {
                var size = new Size(sizeNormal.Width, sizeNormal.Height + 70);
                listView1.Size = size;
            }
            else
            {
                var size = new Size(sizeNormal.Width, sizeNormal.Height);
                listView1.Size = size;
            }
        }

        #region Print

        public void button1_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Employee;
            PickOutButtonCurrentTable(CurrentTable);
            this.OpenAddPanel(panel4, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Фамилия", "Имя", "Отчество", "Телефон", "Должность"
            );
            if (_repositoryEmployee is not RepositoryEmployee upcastRepository) return;
            foreach (var item in upcastRepository.GetAllIncludeForeignKey())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Surname,
                    item.Name,
                    item.Patronymic,
                    item.Phone,
                    item.Position.Name
                });
                listView1.Items.Add(newItem);
            }

            UpdateCombobox();
            ToggleFiltrationMenu(false);
        }

        public void button2_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Position;
            PickOutButtonCurrentTable(CurrentTable);
            this.OpenAddPanel(panel5, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Название"
            );
            foreach (var item in _repositoryPosition.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Name,
                });
                listView1.Items.Add(newItem);
            }

            UpdateCombobox();
            ToggleFiltrationMenu(false);
        }

        private void PrintRegistrationsDocument(IEnumerable<RegistrationDocument> registrationsDocument)
        {
            this.GenerateColumns(
                listView1,
                100,
                "Код", "Дата создания", "Дата начала действия",
                "Направление действия", "Тип документа", "Создатель", "Утвердитель"
            );
            foreach (var item in registrationsDocument)
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.CreateDate.ToShortDateString(),
                    item.StartDate.ToShortDateString(),
                    item.DirectionDocument,
                    item.TypeDocument.Name,
                    item.EmployeeCreator.Surname,
                    item.EmployeeApprover.Surname
                });
                listView1.Items.Add(newItem);
            }
        }

        public void button3_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.RegistrationDocument;
            PickOutButtonCurrentTable(CurrentTable);
            this.OpenAddPanel(panel6, _addPanels);
            if (_repositoryRegistrationDocument is not RepositoryRegistrationDocument upcastRepository) return;
            PrintRegistrationsDocument(upcastRepository.GetAllIncludeForeignKey());
            UpdateCombobox();
            ToggleFiltrationMenu(true);
        }

        public void button4_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.TypeDocument;
            PickOutButtonCurrentTable(CurrentTable);
            this.OpenAddPanel(panel7, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Название", "Должность"
            );
            if (_repositoryTypeDocument is not RepositoryTypeDocument upcastRepository) return;
            foreach (var item in upcastRepository.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Name,
                    item.Position.Name
                });
                listView1.Items.Add(newItem);
            }

            UpdateCombobox();
            ToggleFiltrationMenu(false);
        }

        #endregion

        #region Add

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            maskedTextBox1.Text = string.Empty;
            comboBox1.SelectedIndex = -1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox5.Text = string.Empty;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dateTimePicker3.Value = DateTime.Now.Date;
            dateTimePicker1.Value = DateTime.Now.Date;
            comboBox6.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox6.Text = string.Empty;
            comboBox4.SelectedIndex = -1;
        }

        private void button5_Click(object sender, EventArgs e)
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
                Surname = textBox1.Text,
                Name = textBox2.Text,
                Patronymic = textBox3.Text,
                Phone = maskedTextBox1.Text,
                PositionId = position.Id
            };

            _repositoryEmployee.Insert(employee);
            button1_Click(null, null);
            button6_Click(null, null);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var position = new Position
            {
                Name = textBox5.Text
            };

            _repositoryPosition.Insert(position);
            button2_Click(null, null);
            button7_Click(null, null);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex == -1 || comboBox5.SelectedIndex == -1 ||
                comboBox3.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var typeDocument = _repositoryTypeDocument.GetModelByProperty(comboBox5.SelectedItem.ToString(), "Name");
            var creator = _repositoryEmployee.GetModelByProperty(comboBox3.SelectedItem.ToString(), "Surname");
            var approver = _repositoryEmployee.GetModelByProperty(comboBox2.SelectedItem.ToString(), "Surname");
            var registrationDocument = new RegistrationDocument
            {
                CreateDate = dateTimePicker3.Value.Date,
                StartDate = dateTimePicker1.Value.Date,
                DirectionDocument = comboBox6.SelectedItem.ToString(),
                TypeDocumentId = typeDocument.Id,
                EmployeeCreatorId = creator.Id,
                EmployeeApproverId = approver.Id
            };

            _repositoryRegistrationDocument.Insert(registrationDocument);
            button3_Click(null, null);
            button9_Click(null, null);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text) || comboBox4.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны указать все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var position = _repositoryPosition.GetModelByProperty(comboBox4.SelectedItem.ToString(), "Name");
            var typeDocument = new TypeDocument
            {
                Name = textBox6.Text,
                PositionId = position.Id
            };

            _repositoryTypeDocument.Insert(typeDocument);
            button4_Click(null, null);
            button11_Click(null, null);
        }

        #endregion

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            switch (CurrentTable)
            {
                case CurrentTable.Employee:
                    var employee = _repositoryEmployee.GetById(id);
                    _repositoryEmployee.Delete(employee);
                    button1_Click(this, null);
                    break;
                case CurrentTable.Position:
                    var position = _repositoryPosition.GetById(id);
                    _repositoryPosition.Delete(position);
                    button2_Click(this, null);
                    break;
                case CurrentTable.RegistrationDocument:
                    var registrationDocument = _repositoryRegistrationDocument.GetById(id);
                    _repositoryRegistrationDocument.Delete(registrationDocument);
                    button3_Click(this, null);
                    break;
                case CurrentTable.TypeDocument:
                    var typeDocument = _repositoryTypeDocument.GetById(id);
                    _repositoryTypeDocument.Delete(typeDocument);
                    button4_Click(this, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            switch (CurrentTable)
            {
                case CurrentTable.Employee:
                    var employee = _repositoryEmployee.GetById(id);
                    var formChangeEmployee = new FormChangeEmployee(this, employee, 
                        _repositoryEmployee, _repositoryPosition);
                    formChangeEmployee.ShowDialog();
                    break;
                case CurrentTable.Position:
                    var position = _repositoryPosition.GetById(id);
                    var formChangePosition = new FormChangePosition(this, position, _repositoryPosition);
                    formChangePosition.ShowDialog();
                    break;
                case CurrentTable.RegistrationDocument:
                    var registrationDocument = _repositoryRegistrationDocument.GetById(id);
                    var formChangeRegistrationDocument = new FormChangeRegistrationDocument(this, registrationDocument,
                        _repositoryRegistrationDocument, _repositoryTypeDocument, _repositoryEmployee);
                    formChangeRegistrationDocument.ShowDialog();
                    break;
                case CurrentTable.TypeDocument:
                    var typeDocument = _repositoryTypeDocument.GetById(id);
                    var formChangeTypeDocument = new FormChangeTypeDocument(this, typeDocument, 
                        _repositoryTypeDocument, _repositoryPosition);
                    formChangeTypeDocument.ShowDialog();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            var itemsListView = listView1.Items;
            var textBox = sender as TextBox;
            if (textBox?.Text == string.Empty)
            {
                foreach (ListViewItem item in itemsListView)
                {
                    item.Selected = false;
                }
                return;
            }

            foreach (ListViewItem item in itemsListView)
            {
                for (var i = 0; i < item.SubItems.Count; i++)
                {
                    if (item.SubItems[i].Text.ToLower().Contains(textBox?.Text.ToLower()))
                    {
                        item.Selected = true;
                        break;
                    }

                    item.Selected = false;
                }
            }
        }

        #region Excel

        private void PrintIntoExcel(CurrentTable currentTable, params string[] namesColumns)
        {
            var application = new Excel.Application();
            var worksheet = (Excel.Worksheet)application.Workbooks.Add(Type.Missing).ActiveSheet;
            const int indexFirstLetter = 65;
            var nextLetter = Convert.ToChar(indexFirstLetter + namesColumns.Length - 1);
            var excelCells = worksheet.get_Range("A1", $"{nextLetter}1").Cells;
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.Interior.Color = Color.Gold;
            excelCells.Merge(Type.Missing);
            var nameTable = currentTable switch
            {
                CurrentTable.Employee => "Сотрудники",
                CurrentTable.Position => "Должности",
                CurrentTable.RegistrationDocument => "Регистрация документов",
                CurrentTable.TypeDocument => "Типы документов",
                _ => throw new ArgumentOutOfRangeException()
            };

            worksheet.Cells[1, 1] = $"Табличны данные \"{nameTable}\"";
            for (var i = 0; i < namesColumns.Length; i++)
            {
                worksheet.Cells[2, i + 1] = namesColumns[i];
                worksheet.Cells[2, i + 1].HorizontalAlignment = Excel.Constants.xlCenter;
                worksheet.Columns[i + 1].ColumnWidth = 35;
            }

            switch (currentTable)
            {
                case CurrentTable.Employee:
                    if (_repositoryEmployee is not RepositoryEmployee upcastRepository1) return;
                    var employees = upcastRepository1.GetAllIncludeForeignKey();
                    var listEmployees = employees.ToList();
                    for (var i = 0; i < listEmployees.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listEmployees[i].Surname;
                        application.Cells[i + 3, 2] = listEmployees[i].Name;
                        application.Cells[i + 3, 3] = listEmployees[i].Patronymic;
                        application.Cells[i + 3, 4] = listEmployees[i].Phone;
                        application.Cells[i + 3, 5] = listEmployees[i].Position.Name;
                    }
                    break;
                case CurrentTable.Position:
                    var positions = _repositoryPosition.GetAll();
                    var listPositions = positions.ToList();
                    for (var i = 0; i < listPositions.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listPositions[i].Name;
                    }
                    break;
                case CurrentTable.RegistrationDocument:
                    if (_repositoryRegistrationDocument is not RepositoryRegistrationDocument upcastRepository2) return;
                    var registrationDocuments = upcastRepository2.GetAllIncludeForeignKey();
                    var listRegistrationDocuments = registrationDocuments.ToList();
                    for (var i = 0; i < listRegistrationDocuments.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listRegistrationDocuments[i].CreateDate.Date.ToShortDateString();
                        application.Cells[i + 3, 2] = listRegistrationDocuments[i].StartDate.Date.ToShortDateString();
                        application.Cells[i + 3, 3] = listRegistrationDocuments[i].DirectionDocument;
                        application.Cells[i + 3, 4] = listRegistrationDocuments[i].TypeDocument.Name;
                        application.Cells[i + 3, 5] = listRegistrationDocuments[i].EmployeeCreator.Surname;
                        application.Cells[i + 3, 6] = listRegistrationDocuments[i].EmployeeApprover.Surname;
                    }
                    break;
                case CurrentTable.TypeDocument:
                    if (_repositoryTypeDocument is not RepositoryTypeDocument upcastRepository3) return;
                    var typesDocument = upcastRepository3.GetAll();
                    var listTypesDocument = typesDocument.ToList();
                    for (var i = 0; i < listTypesDocument.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listTypesDocument[i].Name;
                        application.Cells[i + 3, 2] = listTypesDocument[i].Position.Name;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            application.Visible = true;
        }

        private void сотрундникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Employee, "Фамилия", "Имя", "Отчество", "Телефон", "Должность");
        }

        private void должностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Position, "Название");
        }

        private void регистрацияДокументаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.RegistrationDocument, "Дата оформления", "Дата начала действия", 
                "Направление действия", "Тип документа", "Создатель", "Утвердитель");
        }

        private void типыДокументаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.TypeDocument, "Название", "Должность");
        }

        #endregion

        #region Word

        private void ReplaceData(string target, string data, Word.Document documentMy)
        {
            var content = documentMy.Content;
            content.Find.ClearFormatting();
            content.Find.Execute(FindText: target, ReplaceWith: data);
        }

        private void приказОПереводеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTable != CurrentTable.RegistrationDocument)
            {
                MessageBox.Show("Выберите таблицу регистрации документов!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var registrationDocumentId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            var registrationDocument = _repositoryRegistrationDocument.GetById(registrationDocumentId);
            var typeDocument = _repositoryTypeDocument.GetById(registrationDocument.TypeDocumentId);
            var approver = _repositoryEmployee.GetById(registrationDocument.EmployeeApproverId);
            var position = _repositoryPosition.GetById(approver.PositionId);
            var application = new Word.Application
            {
                Visible = false
            };
            var path = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\PrikazPerevode.docx");
            var document = application.Documents.Open(path);
            ReplaceData("{typeDocument}", typeDocument.Name, document);
            ReplaceData("{createDate1}", registrationDocument.CreateDate.ToShortDateString(), document);
            ReplaceData("{createDate2}", registrationDocument.CreateDate.ToShortDateString(), document);
            ReplaceData("{createDate3}", registrationDocument.CreateDate.ToShortDateString(), document);
            ReplaceData("{startDate1}", registrationDocument.StartDate.ToShortDateString(), document);
            ReplaceData("{startDate2}", registrationDocument.StartDate.ToShortDateString(), document);
            ReplaceData("{startDate3}", registrationDocument.StartDate.ToShortDateString(), document);
            ReplaceData("{approverName}", approver.Surname + ' ' + approver.Name + ' ' + approver.Patronymic, document);
            ReplaceData("{approverPosition}", position.Name, document);
            document.SaveAs(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\PrikazPerevodeResult.docx"));
            application.Visible = true;
        }

        private void приказОКомандированииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTable != CurrentTable.RegistrationDocument)
            {
                MessageBox.Show("Выберите таблицу регистрации документов!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var registrationDocumentId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            var registrationDocument = _repositoryRegistrationDocument.GetById(registrationDocumentId);
            var typeDocument = _repositoryTypeDocument.GetById(registrationDocument.TypeDocumentId);
            var approver = _repositoryEmployee.GetById(registrationDocument.EmployeeApproverId);
            var position = _repositoryPosition.GetById(approver.PositionId);
            var application = new Word.Application
            {
                Visible = false
            };
            var path = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\PrikazKomandirovanii.docx");
            var document = application.Documents.Open(path);
            ReplaceData("{typeDocument}", typeDocument.Name, document);
            ReplaceData("{createDate}", registrationDocument.CreateDate.ToShortDateString(), document);
            ReplaceData("{startDate}", registrationDocument.StartDate.ToShortDateString(), document);
            ReplaceData("{approverName}", approver.Surname + ' ' + approver.Name + ' ' + approver.Patronymic, document);
            ReplaceData("{approverPosition}", position.Name, document);
            document.SaveAs(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\PrikazKomandirovaniiResult.docx"));
            application.Visible = true;
        }

        private void приказОПредоставленииСоциальногоОтпускаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTable != CurrentTable.RegistrationDocument)
            {
                MessageBox.Show("Выберите таблицу регистрации документов!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var registrationDocumentId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            var registrationDocument = _repositoryRegistrationDocument.GetById(registrationDocumentId);
            var typeDocument = _repositoryTypeDocument.GetById(registrationDocument.TypeDocumentId);
            var approver = _repositoryEmployee.GetById(registrationDocument.EmployeeApproverId);
            var position = _repositoryPosition.GetById(approver.PositionId);
            var application = new Word.Application
            {
                Visible = false
            };
            var path = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\PrikazSotsialnomOtpuske.docx");
            var document = application.Documents.Open(path);
            ReplaceData("{typeDocument}", typeDocument.Name, document);
            ReplaceData("{createDate}", registrationDocument.CreateDate.ToShortDateString(), document);
            ReplaceData("{startDate}", registrationDocument.StartDate.ToShortDateString(), document);
            ReplaceData("{approverName}", approver.Surname + ' ' + approver.Name + ' ' + approver.Patronymic, document);
            ReplaceData("{approverPosition}", position.Name, document);
            document.SaveAs(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\PrikazSotsialnomOtpuskeResult.docx"));
            application.Visible = true;
        }

        #endregion

        #region Validate

        private void EnterOnlyLetter(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }

        #endregion

        #region Filter

        private void button14_Click(object sender, EventArgs e)
        {
            if (_repositoryRegistrationDocument is not RepositoryRegistrationDocument upcastDocument) return;
            var registrationsDocument = upcastDocument.GetAllIncludeForeignKey().ToList();
            if (comboBox7.SelectedIndex != -1)
            {
                registrationsDocument = registrationsDocument
                    .Where(x => x.DirectionDocument == comboBox7.SelectedItem.ToString()).ToList();
            }

            if (comboBox8.SelectedIndex != -1)
            {
                var typeDocument =
                    _repositoryTypeDocument.GetModelByProperty(comboBox8.SelectedItem.ToString(), "Name");
                registrationsDocument = registrationsDocument
                    .Where(x => x.TypeDocumentId == typeDocument.Id).ToList();
            }

            PrintRegistrationsDocument(registrationsDocument);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            button3_Click(null, null);
            comboBox7.SelectedIndex = -1;
            comboBox8.SelectedIndex = -1;
        }

        #endregion
    }
}
