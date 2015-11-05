using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Qrator.Common;

namespace QBank
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string ID { get; set; }
        public System.Windows.Forms.FileDialog openFileDialog1;
        private Stopwatch timeSpent;
        #region quesiton code 
        private bool ValidateAndSaveQuestion()
        {
            if (comboSubject.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a subject.", "Qrator", MessageBoxButton.OK, MessageBoxImage.Warning);
                comboSubject.Focus();
                return false;
            }

            //if (htmlwysiwyg1.getPlainText() == null && htmlwysiwyg1.getHTML() == null)
             if (txtQuestion.Text == null )
            {
                MessageBox.Show("Please enter question text.", "Qrator", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuestion.Focus();
                return false;
            }

            //if (htmlwysiwyg2.getPlainText() == null && htmlwysiwyg2.getHTML() == null)
             if (txtOp1.Text == null)
            {
                MessageBox.Show("Please enter text for 1st. option.", "Qrator", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOp1.Focus();
                return false;
            }

            //if (htmlwysiwyg3.getPlainText() == null && htmlwysiwyg3.getHTML() == null)
             if (txtOp2.Text == null)
            {
                MessageBox.Show("Please enter text for 2nd. option.", "Qrator", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOp2.Focus();
                return false;
            }

            if (chkOp1.IsChecked == false && chkOp2.IsChecked == false && chkOp3.IsChecked == false && chkOp4.IsChecked == false && chkOp5.IsChecked == false)
            {
                MessageBox.Show("At least one option must be checked.", "Qrator", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOp1.Focus();
                return false;
            }

            if (chkOp3.IsChecked == true &&  (txtOp3.Text == null))//(htmlwysiwyg4.getPlainText() == null && htmlwysiwyg4.getHTML() == null))
            {
                MessageBox.Show("Please enter text for 3rd. option since you have selected it as correct answer.", "Qrator", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOp3.Focus();
                return false;
            }

            if (chkOp4.IsChecked == true && (txtOp4.Text == null)) //(htmlwysiwyg5.getPlainText() == null && htmlwysiwyg5.getHTML() == null))
            {
                MessageBox.Show("Please enter text for 4th. option since you have selected it as correct answer.", "Qrator", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOp4.Focus();
                return false;
            }

            if (chkOp5.IsChecked == true && (txtOp5.Text == null))//(htmlwysiwyg6.getPlainText() == null && htmlwysiwyg6.getHTML() == null))
            {
                MessageBox.Show("Please enter text for 5th. option since you have selected it as correct answer.", "Qrator", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOp5.Focus();
                return false;
            }

            short complexity = 0;
            if (radEasy.IsChecked == true)
                complexity = 1;
            else if (radMod.IsChecked == true)
                complexity = 2;
            else if (radHard.IsChecked == true)
                complexity = 3;

            List<Answer> answers = new List<Answer>();
            Answer a1 = new Answer
            {
                IsCorrect = (bool)chkOp1.IsChecked,
                OptionText = txtOp1.Text
            };
            answers.Add(a1);

            Answer a2 = new Answer
            {
                IsCorrect = (bool)chkOp2.IsChecked,
                OptionText = txtOp2.Text
            };
            answers.Add(a2);

            Answer a3 = new Answer
            {
                IsCorrect = (bool)chkOp3.IsChecked,
                OptionText = txtOp3.Text
            };
            answers.Add(a3);

            Answer a4 = new Answer
            {
                IsCorrect = (bool)chkOp4.IsChecked,
                OptionText = txtOp4.Text
            };
            answers.Add(a4);

            //if (htmlwysiwyg6.getPlainText() != null && htmlwysiwyg6.getPlainText().Trim() != "")
            if (txtOp5.Text == null)
            {
                Answer a5 = new Answer
                {
                    IsCorrect = (bool)chkOp5.IsChecked,
                    OptionText = txtOp5.Text
                };
                answers.Add(a5);
            }

            timeSpent.Stop();
            TimeSpan ts = timeSpent.Elapsed;

            if (this.ID == null)
            {
                Question q = new Question
                {
                    AnswerExplanation = txtExplination.Text,//htmlwysiwyg7.getHTML(),
                    Instruction = txtDirection.Text,// htmlwysiwyg8.getHTML(),
                    Complexity = complexity,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    QuestionText = txtQuestion.Text, //htmlwysiwyg1.getHTML(),
                    Subject = comboSubject.Text,
                    Topic = comboTopic.Text,
                    OptionTexts = answers,
                    ID = Guid.NewGuid().ToString(),
                    TotalTimeSpent = ts.TotalMinutes
                };
                this.ID = q.ID;
                QBProcessor.CleanseQuestionBankData(q);
                QBManager.QBank.Questions.Add(q);
                addQuestion(q);
                clearFields();
            }
            else
            {
                Question quest = QBManager.QBank.Questions.Find(q => q.ID == this.ID);
                quest.AnswerExplanation = txtExplination.Text;// htmlwysiwyg7.getHTML();
                quest.Instruction = txtDirection.Text; // htmlwysiwyg8.getHTML();
                quest.Complexity = complexity;
                quest.LastModifiedDate = DateTime.Now;
                quest.QuestionText = txtQuestion.Text; // htmlwysiwyg1.getHTML();
                quest.Subject = comboSubject.Text;
                quest.Topic = comboTopic.Text;

                //Update option texts
                quest.OptionTexts[0].IsCorrect = answers[0].IsCorrect;
                quest.OptionTexts[0].ModifiedDate = DateTime.Now;
                quest.OptionTexts[0].OptionText = answers[0].OptionText;

                quest.OptionTexts[1].IsCorrect = answers[1].IsCorrect;
                quest.OptionTexts[1].ModifiedDate = DateTime.Now;
                quest.OptionTexts[1].OptionText = answers[1].OptionText;

                quest.OptionTexts[2].IsCorrect = answers[2].IsCorrect;
                quest.OptionTexts[2].ModifiedDate = DateTime.Now;
                quest.OptionTexts[2].OptionText = answers[2].OptionText;

                quest.OptionTexts[3].IsCorrect = answers[3].IsCorrect;
                quest.OptionTexts[3].ModifiedDate = DateTime.Now;
                quest.OptionTexts[3].OptionText = answers[3].OptionText;

                //An option has been removed
                if (quest.OptionTexts.Count > answers.Count)
                {
                    quest.OptionTexts[4].IsDeleted = true;
                }
                else if (quest.OptionTexts.Count < answers.Count)   //An option has been added
                {
                    Answer a = new Answer();
                    a.IsCorrect = answers[4].IsCorrect;
                    a.ModifiedDate = DateTime.Now;
                    a.OptionText = answers[4].OptionText;
                    quest.OptionTexts.Add(a);
                }
                else if (answers.Count > 4)
                {
                    quest.OptionTexts[4].IsCorrect = answers[4].IsCorrect;
                    quest.OptionTexts[4].ModifiedDate = DateTime.Now;
                    quest.OptionTexts[4].OptionText = answers[4].OptionText;
                }

                if (quest.TotalTimeSpent != 0.00)
                {
                    quest.TotalTimeSpent = quest.TotalTimeSpent + ts.TotalMinutes;
                }
                else
                {
                    quest.TotalTimeSpent = ts.TotalMinutes;
                }
                QBProcessor.CleanseQuestionBankData(quest);
                int index = 0;
                foreach (Question q in itmsQuestions.Items)
                {
                    
                    if (q.ID == this.ID)
                    {
                        itmsQuestions.Items.Remove(q);
                        itmsQuestions.Items.Insert(index, quest);
                        break;
                    }
                    index++;
                }
                clearFields();
            }

            QBManager.Save();
            return true;
        }

        private void NewQuestion_Click(object sender, EventArgs e )
        {
            clearFields();
        }
        //Save
        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateAndSaveQuestion())
            {
                QBManager.Load();
                //frmParent.RefreshGrid(QBManager.QBank.Questions);

                MessageBox.Show(this, "Question saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                if (this.ID == null)
                {
                    //Reload the form with empty fields
                    //comboBox1.SelectedIndex = -1;
                    comboTopic.SelectedIndex = -1;

                    //htmlwysiwyg1.setHTML("");
                    //htmlwysiwyg2.setHTML("");
                    //htmlwysiwyg3.setHTML("");
                    //htmlwysiwyg4.setHTML("");
                    //htmlwysiwyg5.setHTML("");
                    //htmlwysiwyg6.setHTML("");
                    //htmlwysiwyg7.setHTML("");
                    txtQuestion.Text = string.Empty;
                    txtOp1.Text = string.Empty;
                    txtOp2.Text = string.Empty;
                    txtOp3.Text = string.Empty;
                    txtOp4.Text = string.Empty;
                    txtOp5.Text = string.Empty;
                    txtExplination.Text = string.Empty;

                    if (!chkDescr.IsChecked == true)
                        // htmlwysiwyg8.setHTML("");
                        txtDirection.Text = string.Empty;

                    chkOp1.IsChecked = false;
                    chkOp2.IsChecked = false;
                    chkOp3.IsChecked = false;
                    chkOp4.IsChecked = false;
                    chkOp5.IsChecked = false;

                    radMod.IsChecked = true;
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void clearFields()
        {
            //Reload the form with empty fields
            //comboBox1.SelectedIndex = -1;
            comboTopic.SelectedIndex = -1;

            this.ID = null;
            txtQuestion.Text = string.Empty;
            txtOp1.Text = string.Empty;
            txtOp2.Text = string.Empty;
            txtOp3.Text = string.Empty;
            txtOp4.Text = string.Empty;
            txtOp5.Text = string.Empty;
            txtExplination.Text = string.Empty;

            if (!chkDescr.IsChecked == true)
                // htmlwysiwyg8.setHTML("");
                txtDirection.Text = string.Empty;

            chkOp1.IsChecked = false;
            chkOp2.IsChecked = false;
            chkOp3.IsChecked = false;
            chkOp4.IsChecked = false;
            chkOp5.IsChecked = false;

            radMod.IsChecked = true;
            this.ID = null;
        }
        private void LoadAndDisplayQuestion(Question quest)
        {
            comboSubject.Text = quest.Subject;
            comboTopic.Text = quest.Topic;

            //setHTML(quest.AnswerExplanation);
            //htmlwysiwyg8.setHTML(quest.Instruction);
            txtExplination.Text = quest.AnswerExplanation;
            txtDirection.Text = quest.Instruction;
            // quest.Complexity;
           // htmlwysiwyg1.setHTML(quest.QuestionText);
           txtQuestion.Text = quest.QuestionText;
            int complexity = quest.Complexity;
            if (complexity == 1)
                radEasy.IsChecked = true;
            else if (complexity == 2)
                radMod.IsChecked = true;
            else if (complexity == 3)
                radHard.IsChecked = true;

            if (quest.OptionTexts.Count > 0)
            {
                //htmlwysiwyg2.setHTML(quest.OptionTexts[0].OptionText);
                txtOp1.Text = quest.OptionTexts[0].OptionText;
                chkOp1.IsChecked = quest.OptionTexts[0].IsCorrect;
            }

            if (quest.OptionTexts.Count > 1)
            {
                //htmlwysiwyg3.setHTML(quest.OptionTexts[1].OptionText);
                txtOp2.Text = quest.OptionTexts[1].OptionText;
                chkOp2.IsChecked = quest.OptionTexts[1].IsCorrect;
            }

            if (quest.OptionTexts.Count > 2)
            {
                //htmlwysiwyg4.setHTML(quest.OptionTexts[2].OptionText);
                txtOp3.Text = quest.OptionTexts[2].OptionText;
                chkOp3.IsChecked = quest.OptionTexts[2].IsCorrect;
            }

            if (quest.OptionTexts.Count > 3)
            {
                //htmlwysiwyg5.setHTML(quest.OptionTexts[3].OptionText);
                txtOp4.Text = quest.OptionTexts[3].OptionText;
                chkOp4.IsChecked = quest.OptionTexts[3].IsCorrect;
            }

            if (quest.OptionTexts.Count > 4)
            {
                //htmlwysiwyg6.setHTML(quest.OptionTexts[4].OptionText);
                txtOp4.Text = quest.OptionTexts[4].OptionText;
                chkOp4.IsChecked = quest.OptionTexts[4].IsCorrect;
            }
            timeSpent.Start();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            //string[] subjects = MasterUtil.GetSubjects(QBManager.QBank.ExamGrade);
            //comboBox1.Items.AddRange(subjects);

            //QuestionBank qb = QBManager.QBank;
            //Question quest = qb.Questions.Find(q => q.ID == this.ID);
            //this.Text = qb.Name + " - Edit Question # " + (qb.GetQuestionIndex(this.ID) + 1).ToString();

            //if (this.ID != null && quest != null)
            //{
            //    timeSpent.Start();
            //    LoadAndDisplayQuestion(quest);
            //}
            //else
            //{
            //    this.Text = qb.Name + " - Add Question";
            //    button3.Visible = false;
            //    button4.Visible = false;
            //    button5.Visible = false;
            //}

            //EnableEditorToolBar(htmlwysiwyg1);
            //EnableEditorToolBar(htmlwysiwyg2);
            //EnableEditorToolBar(htmlwysiwyg3);
            //EnableEditorToolBar(htmlwysiwyg4);
            //EnableEditorToolBar(htmlwysiwyg5);
            //EnableEditorToolBar(htmlwysiwyg6);
            //EnableEditorToolBar(htmlwysiwyg7);
            //EnableEditorToolBar(htmlwysiwyg8);
        }

        //private void EnableEditorToolBar(htmlwysiwyg ctl)
        //{
        //    //Make sure to enable toolbar in text control
        //    ctl.ShowAlignCenterButton = true;
        //    ctl.ShowAlignLeftButton = true;
        //    ctl.ShowAlignRightButton = true;
        //    ctl.ShowBackColorButton = true;
        //    ctl.ShowBolButton = true;
        //    ctl.ShowBulletButton = true;
        //    ctl.ShowCopyButton = true;
        //    ctl.ShowCutButton = true;
        //    ctl.ShowIndentButton = true;
        //    ctl.ShowItalicButton = true;
        //    ctl.ShowJustifyButton = true;
        //    ctl.ShowOrderedListButton = true;
        //    ctl.ShowOutdentButton = true;
        //    ctl.ShowPasteButton = true;
        //    ctl.ShowTxtBGButton = true;
        //    ctl.ShowTxtColorButton = true;
        //    ctl.ShowUnderlineButton = true;
        //}

        //Delete

        private void ExpandGrid(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Name == "collapse")
            {
                if (collapse.IsChecked == true)
                {
                    collapse1.IsChecked = false;
                    collapse2.IsChecked = false;
                }
            }

            if (((CheckBox)sender).Name == "collapse1")
            {
                if (collapse1.IsChecked == true)
                {
                    collapse.IsChecked = false;
                    collapse2.IsChecked = false;
                }
            }

            if (((CheckBox)sender).Name == "collapse2")
            {
                if (collapse2.IsChecked == true)
                {
                    collapse.IsChecked = false;
                    collapse1.IsChecked = false;
                }
                
            }
        }
        private void delete_Click(object sender, EventArgs e)
        {
            if ( this.ID == null)
            {
                MessageBox.Show("No selected question to be deleted","OK", MessageBoxButton.OK, MessageBoxImage.Warning);
                
            }
            MessageBoxResult dr = MessageBox.Show("Are you sure to delete this question?", "Confirmation", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                Question q = QBManager.QBank.Questions.Where(qt => qt.ID == this.ID).FirstOrDefault();
                if (q.MasterCode != null)
                    q.IsDeleted = true; //Logically delete
                else
                    QBManager.QBank.Questions.Remove(q);    //physically delete because this questions is not yet synced with server
                QBManager.Save();
                //frmParent.RefreshGrid(QBManager.QBank.Questions);
                removeQuestion(q);
                //this.Close();

            }
        }

        private void comboSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuestionBank qb = QBManager.QBank;
            comboTopic.Items.Clear();
            string text = ((System.Windows.Controls.ContentControl)(comboSubject.SelectedItem)).Content.ToString();
            string[] topics = MasterUtil.GetTopics(qb.ExamGrade, text);
            foreach (string t in topics)
            {
                comboTopic.Items.Add(t);
            }
           
        }

        //private void GetNextQuestion()
        //{
        //    QuestionBank qb = QBManager.QBank;
        //    Question quest = qb.GetNextQuestion(this.ID);

        //    if (quest != null)
        //    {
        //        this.ID = quest.ID;
        //        this.Text = qb.Name + " - Edit Question # " + (qb.GetQuestionIndex(this.ID) + 1).ToString();
        //        LoadAndDisplayQuestion(quest);
        //    }
        //    else
        //    {
        //        MessageBox.Show("No more question available. You are already at the last question.");
        //    }
        //}

        //private void GetPreviousQuestion()
        //{
        //    QuestionBank qb = QBManager.QBank;
        //    Question quest = qb.GetPreviousQuestion(this.ID);

        //    if (quest != null)
        //    {
        //        this.ID = quest.ID;
        //        this.Text = qb.Name + " - Edit Question # " + (qb.GetQuestionIndex(this.ID) + 1).ToString();
        //        LoadAndDisplayQuestion(quest);
        //    }
        //    else
        //    {
        //        MessageBox.Show("No more question available. You are already at the first question.");
        //    }
        //}

        private bool HasQuestionChanged()
        {
            int i = 0;
            QuestionBank qb = QBManager.QBank;
            Question quest = qb.GetQuestion(this.ID);

            if (comboSubject.Text != quest.Subject)
            {
                i++;
            }

            if (comboTopic.Text != quest.Topic)
            {
                i++;
            }

            if (txtQuestion.Text != quest.QuestionText)
            {
                i++;
            }

            if (txtDirection.Text != quest.Instruction)
            {
                i++;
            }

            if (txtOp1.Text != quest.OptionTexts[0].OptionText)
            {
                i++;
            }

            if (txtOp2.Text != quest.OptionTexts[1].OptionText)
            {
                i++;
            }

            if (txtOp3.Text != quest.OptionTexts[2].OptionText)
            {
                i++;
            }

            if (txtOp4.Text != quest.OptionTexts[3].OptionText)
            {
                i++;
            }

            if (quest.OptionTexts.Count == 5 && txtOp5.Text != quest.OptionTexts[4].OptionText)
            {
                i++;
            }

            if (txtExplination.Text != quest.AnswerExplanation)
            {
                i++;
            }

            //Has right answer changed
            if (chkOp1.IsChecked != quest.OptionTexts[0].IsCorrect)
            {
                i++;
            }

            if (chkOp2.IsChecked != quest.OptionTexts[1].IsCorrect)
            {
                i++;
            }

            if (chkOp3.IsChecked != quest.OptionTexts[2].IsCorrect)
            {
                i++;
            }

            if (chkOp4.IsChecked != quest.OptionTexts[3].IsCorrect)
            {
                i++;
            }

            if (quest.OptionTexts.Count == 5 && chkOp5.IsChecked != quest.OptionTexts[4].IsCorrect)
            {
                i++;
            }

            //Has Complexity value changed
            if (radEasy.IsChecked == true && quest.Complexity != 1)
                i++;
            else if (radMod.IsChecked == true && quest.Complexity != 2)
                i++;
            else if (radHard.IsChecked == true && quest.Complexity != 3)
                i++;

            return (i > 0);
        }

        //Next question
        private void button4_Click(object sender, EventArgs e)
        {
            if (HasQuestionChanged())
            {
                MessageBoxResult dr = MessageBox.Show("Do you want to save changes before you proceed?", "Confirm", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (dr == MessageBoxResult.Yes)
                {
                    if (ValidateAndSaveQuestion())
                    {
                        // GetNextQuestion();
                    }
                }
                else if (dr == MessageBoxResult.No)
                    //GetNextQuestion();
                    return;
                else
                    return;
            }
            else
                //GetNextQuestion();
                return;
        }

        //Previous question
        private void button5_Click(object sender, EventArgs e)
        {
            if (HasQuestionChanged())
            {
                MessageBoxResult dr = MessageBox.Show("Do you want to save changes before you proceed?", "Confirm", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (dr == MessageBoxResult.Yes)
                {
                    if (!ValidateAndSaveQuestion())
                    {
                        //GetPreviousQuestion();
                        return;
                    }
                }
                else if (dr == MessageBoxResult.No)
                    //GetPreviousQuestion();
                    return;
                else
                    return;
            }
            else
                //GetPreviousQuestion();
                return;
        }

        #endregion
        public MainWindow()
        {
            timeSpent = new Stopwatch();
            InitializeComponent();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog1.DefaultExt = "dbf";
            this.openFileDialog1.Filter = "Question Bank Files|*.qbf";
            this.openFileDialog1.InitialDirectory = "C:\\";
            this.openFileDialog1.Title = "Open Question Bank File";
            collapse.IsChecked = true ;
            collapse1.IsChecked = false;
            collapse2.IsChecked = false;
        }

        private void Ribbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void openNewQuestionBank(object sender, EventArgs e)
        {
            childQuestionBank child = new childQuestionBank();
            child.Show();
        }

        private void openValidateQuestionBank(object sender, EventArgs e)
        {
        }

        private void openExistQuestionBank(object sender, EventArgs e)
        {
            
            //if (Properties.Settings.Default["LastFolderPath"].ToString() != "")
            //{
            //    openFileDialog1.InitialDirectory = Properties.Settings.Default["LastFolderPath"].ToString();
            //}

            //  if (Properties.Settings.Default["LastFolderPath"].ToString() != "")
            //{
            //    openFileDialog1.InitialDirectory = Properties.Settings.Default["LastFolderPath"].ToString();
           // }

              System.Windows.Forms.DialogResult dr = openFileDialog1.ShowDialog();
           
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                QBManager.FileName = openFileDialog1.FileName;

                // Saves settings in application configuration file
                //Properties.Settings.Default["LastFolderPath"] = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                //Properties.Settings.Default.Save();

                //toolStripStatusLabel1.Text = openFileDialog1.FileName;

                QBManager.Load();
                if (QBManager.QBank.Mode == null)
                    QBManager.QBank.Mode = "Operator";

                //.Rows.Clear();
                List<Question> list = QBManager.QBank.Questions;
                foreach (Question q in list)
                {
                    itmsQuestions.Items.Add(q);
                }

                //toolStripStatusLabel3.Text = QBManager.QBank.ExamGrade;
                //toolStripStatusLabel4.Text = QBManager.QBank.LastModifiedDate.ToString();

                //editDetailsToolStripMenuItem.Enabled = true;
                //newQuestionToolStripMenuItem.Enabled = true;
                //toolStripMenuItem3.Enabled = true;
                //syncToolStripMenuItem.Enabled = true;
            }
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateAndSaveQuestion();
        }

        public void addQuestion(Question q)
        {
            itmsQuestions.Items.Add(q);

        }

        public void removeQuestion(Question q)
        {
            itmsQuestions.Items.Remove(q);

        }

        private void btnEdit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Question SelectedQuestion = (Question)((System.Windows.FrameworkElement)(sender)).DataContext;
            this.ID = SelectedQuestion.ID;
            LoadAndDisplayQuestion(SelectedQuestion);
        }

        private void btnDelete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("Are you sure to delete this question?", "Confirmation", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                Question q = (Question)((System.Windows.FrameworkElement)(sender)).DataContext;
                if (q.MasterCode != null)
                    q.IsDeleted = true; //Logically delete
                else
                    QBManager.QBank.Questions.Remove(q);    //physically delete because this questions is not yet synced with server
                QBManager.Save();
                //frmParent.RefreshGrid(QBManager.QBank.Questions);
                removeQuestion(q);
                //this.Close();

            }
        }
    }
}
