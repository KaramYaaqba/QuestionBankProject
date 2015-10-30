using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Qrator.Common;



namespace QBank
{
    /// <summary>
    /// Interaction logic for QuestionBank.xaml
    /// </summary>
    public partial class childQuestionBank : Window
    {
        public bool IsNew { get; set; }
        public childQuestionBank()
        {
            InitializeComponent();
            IsNew = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            //Validate
            if (txtName.Text.Trim() == "" || txtName.Text == string.Empty)
            {
                MessageBox.Show("Please enter question bank name.", "QBank", MessageBoxButton.OK ,MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            if (comboGrade.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an entrance examination.", "QBank", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboGrade.Focus();
                return;
            }

            if (txtDescr.Text.Trim() == "" || txtDescr.Text == string.Empty)
            {
                MessageBox.Show("Please provide a description of the question bank.", "QBank", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDescr.Focus();
                return;
            }

            //Save Information
            if (this.IsNew)
            {
                QuestionBank qb = new QuestionBank
                {
                    Description = txtDescr.Text,
                    Name = txtName.Text,
                    TimeAllocated = Int16.Parse(txtTime.Text),
                    Questions = new List<Question>(),
                    ExamGrade = comboGrade.Text
                };

                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                System.Windows.Forms.DialogResult dr = saveFileDialog1.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    QBManager.FileName = saveFileDialog1.FileName;
                    qb.CreatedDate = DateTime.Now;
                    qb.CreatedBy = Environment.UserName;
                    qb.LastModifiedDate = DateTime.Now;
                    qb.Mode = "Operator";
                    QBManager.QBank = qb;
                    QBManager.Save();

                    MessageBox.Show("Question bank created successfully. Please add questions.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    //frmParent.RefreshGrid(qb.Questions);
                    this.Close();
                }
            }
            else
            {
                QBManager.QBank.Description = txtDescr.Text;
                QBManager.QBank.LastModifiedDate = DateTime.Now;
                QBManager.QBank.Name = txtName.Text;
                QBManager.QBank.TimeAllocated = Int16.Parse(txtTime.Text);
                QBManager.QBank.ExamGrade = comboGrade.Text;
                QBManager.Save();
                MessageBox.Show("Question bank updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }

        }
    }
}
