using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Xml;
namespace CreateFlows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this.FlowName.SelectionChanged += FlowName_SelectionChanged;
            this.FlowContent.SelectionChanged += FlowContent_SelectionChanged;
        }

        void FlowContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox _listbox = e.Source as ListBox;
            Step _step = _listbox.SelectedItem as Step;
            LoadData(_step);
        }

        void LoadData(Step step)
        {
            if (step == null) return;
            txtAssemblyName.Text = step.AssemblyName;
            txtType.Text = step.Type;
            chkIgnore.IsChecked = step.IsIgnore;
        }

        void SetBlank()
        {
            txtAssemblyName.Text = "";
            txtType.Text = "";
            chkIgnore.IsChecked = false;
        }

        void FlowName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox _combobox = e.Source as ComboBox;
            Flow flowitem = _combobox.SelectedItem as Flow;
            if (flowitem == null) return;
            SelectedFlow = flowitem;
            this.FlowContent.ItemsSource = flowitem.Steps;
            SetBlank();
        }
        string currentDirectory;
        /// <summary>
        /// 所有流程的xml
        /// </summary>
        XDocument flow;

        /// <summary>
        /// 流程集合
        /// </summary>
        List<Flow> currentFlow;

        /// <summary>
        /// 当前处理流程
        /// </summary>
        Flow SelectedFlow;
        void ReadFlows()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "打开流程配置XML";
            dialog.Filter = "流程|*.xml;*.XML";
            dialog.InitialDirectory = currentDirectory;
            if (true == dialog.ShowDialog())
            {
                Stream stream = dialog.OpenFile();

                flow = XDocument.Load(stream);
                stream.Close();

                try
                {
                    currentFlow = GetFlow();
                    Init();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取配置文件错误" + Environment.NewLine + ex.Message);
                }
            }

        }

        List<Flow> GetFlow()
        {
            List<Flow> result = new List<Flow>();

            try
            {
                List<XElement> asms = flow.Root.Elements("flow").ToList();

                for (int i = 0; i < asms.Count; i++)
                {
                    Flow key = new Flow();

                    key.FlowName = asms[i].Attribute("name").Value;
                    result.Add(key);
                    List<XElement> itemChildren = asms[i].Elements("step").ToList();

                    if (itemChildren != null)
                    {
                        List<Step> value = new List<Step>();
                        for (int j = 0; j < itemChildren.Count; j++)
                        {
                            Step step = new Step();
                            step.AssemblyName = itemChildren[j].Attribute("name").Value;
                            step.Type = itemChildren[j].Attribute("type").Value;
                            step.IsIgnore = itemChildren[j].Attribute("ignore").Value.ToLower() == "true" ? true : false;
                            step.Order = int.Parse(itemChildren[j].Attribute("order").Value);
                            value.Add(step);
                        }

                        key.Steps = value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        void Init()
        {
            //combobox

            this.FlowName.ItemsSource = currentFlow;
            this.FlowName.DisplayMemberPath = "FlowName";

        }

        List<Step> ReOrder(List<Step> steps)
        {
            StepCompare compare = new StepCompare();
            steps.Sort(compare);
            return steps;
        }

        class StepCompare : IComparer<Step>
        {

            public int Compare(Step x, Step y)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (y == null)
                    {
                        return 1;
                    }
                    else
                    {
                        int r = x.Order.CompareTo(y.Order);

                        return r;

                    }
                }
            }
        }

        void SaveXML(List<Flow> file, string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //创建一个根节点（一级）
            try
            {
                XmlElement root = doc.CreateElement("flowroot");
                doc.AppendChild(root);

                foreach (Flow item in file)
                {
                    XmlElement flow = doc.CreateElement("flow");
                    flow.SetAttribute("name", item.FlowName);
                    root.AppendChild(flow);
                    List<Step> steps = item.Steps;
                    if (steps == null)
                        continue;
                    for (int i = 0; i < steps.Count; i++)
                    {
                        XmlElement step = doc.CreateElement("step");
                        step.SetAttribute("name", steps[i].AssemblyName);
                        step.SetAttribute("ignore", steps[i].IsIgnore.ToString());
                        step.SetAttribute("order", steps[i].Order.ToString());
                        step.SetAttribute("type", steps[i].Type);
                        step.SetAttribute("isdownloaded", "false");
                        step.SetAttribute("ispage", "false");
                        flow.AppendChild(step);
                    }

                }

                doc.Save(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存出错" + Environment.NewLine + ex.Message);
            }
        }

        private void btnOpen_Click_1(object sender, RoutedEventArgs e)
        {
            ReadFlows();
        }

        private void btnSave_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "流程|*.xml;*.XML";
            saveDialog.Title = "保存流程配置文件";
            if (saveDialog.ShowDialog() == true)
            {
                string path = saveDialog.FileName;
                try
                {
                    SaveXML(currentFlow, path);
                    MessageBox.Show("保存成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存错误" + ex.Message);
                }
            }
        }



        private void btnNew_Click_1(object sender, RoutedEventArgs e)
        {
            NewStep frmNewStep = new NewStep("流程");
            if (true == frmNewStep.ShowDialog())
            {
                if (!string.IsNullOrEmpty(frmNewStep.Result))
                {
                    Flow _flow = new Flow();
                    _flow.FlowName = frmNewStep.Result;
                    if (currentFlow == null)
                    {
                        currentFlow = new List<Flow>();
                    }
                    currentFlow.Add(_flow);
                    FlowName.ItemsSource = null;
                    Init();

                }
            }
        }

        private void btnDelete_Click_1(object sender, RoutedEventArgs e)
        {
            Step selectedStep = this.FlowContent.SelectedItem as Step;
            if (selectedStep == null)
            {
                return;
            }
            List<Step> _temp = (this.FlowContent.ItemsSource as List<Step>);
            if (_temp == null) return;
           
            for (int i = selectedStep.Order; i < _temp.Count; i++)
            {
                _temp[i].Order--;
            }
            _temp.RemoveAt(selectedStep.Order - 1);
            this.FlowContent.ItemsSource = null;
            this.FlowContent.ItemsSource = _temp;
        }

        private void btnTop_Click_1(object sender, RoutedEventArgs e)
        {
            Step selectedStep = this.FlowContent.SelectedItem as Step;
            if (selectedStep == null)
            {
                return;
            }
            if (selectedStep.Order > 1)
            {
                int selectIndex = selectedStep.Order;
                List<Step> _temp = (this.FlowContent.ItemsSource as List<Step>);

                _temp[selectIndex - 2].Order++;
                _temp[selectIndex - 1].Order--;

                this.FlowContent.ItemsSource = null;
                _temp = ReOrder(_temp);
                this.FlowContent.ItemsSource = _temp;
                this.FlowContent.SelectedIndex = selectIndex - 2;

            }
        }

        private void btnAdd_Click_1(object sender, RoutedEventArgs e)
        {
            NewStep frmNewStep = new NewStep("程序集");
            if (true == frmNewStep.ShowDialog())
            {
                if (!string.IsNullOrEmpty(frmNewStep.Result))
                {
                    Step newStep = new Step();
                    newStep.AssemblyName = frmNewStep.Result;
                    newStep.Order = this.FlowContent.Items.Count + 1;
                    List<Step> _temp = (this.FlowContent.ItemsSource as List<Step>);
                    if (_temp == null)
                        _temp = new List<Step>();
                    _temp.Add(newStep);
                    _temp = ReOrder(_temp);
                    this.FlowContent.ItemsSource = null;
                    this.FlowContent.ItemsSource = _temp;
                    this.FlowContent.SelectedIndex = newStep.Order - 1;

                }
            }
        }

        private void btnDown_Click_1(object sender, RoutedEventArgs e)
        {
            Step selectedStep = this.FlowContent.SelectedItem as Step;
            if (selectedStep == null)
            {
                return;
            }
            List<Step> _temp = (this.FlowContent.ItemsSource as List<Step>);
            if (selectedStep.Order < _temp.Count)
            {
                int selectIndex = selectedStep.Order;


                _temp[selectIndex - 1].Order++;
                _temp[selectIndex].Order--;

                this.FlowContent.ItemsSource = null;
                _temp = ReOrder(_temp);
                this.FlowContent.ItemsSource = _temp;
                this.FlowContent.SelectedIndex = selectIndex;

            }
        }

        private void btnSaveItem_Click_1(object sender, RoutedEventArgs e)
        {
            Step selectedStep = this.FlowContent.SelectedItem as Step;
            if (selectedStep == null)
            {
                return;
            }
            List<Step> _temp = (this.FlowContent.ItemsSource as List<Step>);
            _temp[selectedStep.Order - 1].AssemblyName = txtAssemblyName.Text;
            _temp[selectedStep.Order - 1].Type = txtType.Text;
            if (chkIgnore.IsChecked != null)
                _temp[selectedStep.Order - 1].IsIgnore = chkIgnore.IsChecked == true ? true : false;


            SelectedFlow.Steps = null;
            SelectedFlow.Steps = _temp;
            currentFlow.Where((p) => { return p.FlowName == SelectedFlow.FlowName; }).ToList()[0] = SelectedFlow;
            Init();
            MessageBox.Show("保存成功");
        }
    }
}
