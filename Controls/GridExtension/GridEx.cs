using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PDA.COP.FW.GridExtension
{
    /*Grid容器处理类
     * 取数据
     * ExampleData result= GridEx.GetVale<ExampleData>(GridName);
     * 置数据
     * GridEx.SetValue(GridName,new ExampleData{...});
     *
     */
    public static class GridEx
    {

        //public static object GetGridSource(DependencyObject d)
        //{
        //    return (object)d.GetValue(GridSourceProperty);
        //}

        //public static void SetGridSource(DependencyObject d, object value)
        //{
        //    d.SetValue(GridSourceProperty, value);
        //}

        static  string prefix = "TKS_";
        public static readonly DependencyProperty GridSourceProperty =
        DependencyProperty.RegisterAttached("GridSource", typeof(object), typeof(System.Windows.Controls.Grid),
        new PropertyMetadata(OnGridSourceChanged));

        private static void OnGridSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.Grid gridContainer = d as System.Windows.Controls.Grid;
            var source = e.NewValue as object;

            List<Control> children = GetChildObjects<Control>(gridContainer, null);
            Dictionary<string, Control> controls = new Dictionary<string, Control>();
            foreach (var item in children)
            {
                if (!string.IsNullOrEmpty(item.Name))
                    //throw new Exception(item.GetType().Name + "控件未命名");
                    if (!controls.ContainsKey(  item.Name))
                        controls.Add( item.Name, item);
            }



            PropertyInfo[] properties = source.GetType().GetProperties();


            foreach (var item in properties)
            {
                if (controls.ContainsKey(prefix + item.Name))
                {
                    SetControlValue(controls[prefix+ item.Name], item.GetValue(source, null));
                }
            }


        }

        /// <summary>
        /// 获取容器内的值到实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="grid">Grid容器</param>
        /// <returns></returns>
        public static T GetValue<T>(Grid grid) where T : class ,new()
        {
            error = 0;
            string currentProperty = string.Empty;
            string currentPropertyType = string.Empty;
            object currentVal=null ;
            T result = new T();
            Type tt = typeof(T);
            List<Control> children = GetChildObjects<Control>(grid, null);
            Dictionary<string, Control> controls = new Dictionary<string, Control>();
            foreach (var item in children)
            {

                if (!controls.ContainsKey(item.Name))
                    controls.Add(item.Name, item);
            }

            PropertyInfo[] properties = tt.GetProperties();
            try
            {
                foreach (var item in properties)
                {
                    if (controls.ContainsKey(prefix+item.Name))
                    {
                        currentProperty = item.Name;
                        Control con = controls[prefix + item.Name];
                    

                        object val = GetControlValue(con);
                        bool flag=  ValdateItem(con, item, val);
                        if (flag)
                        {
                            currentVal=val;
                            currentPropertyType = item.PropertyType.ToString();
                            val = Convert.ChangeType(val, item.PropertyType, null);
                            item.SetValue(result, val, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取控件值["+currentVal+"]后对实体属性[" + currentProperty + "]赋值异常，目标属性类型为" 
                   +currentPropertyType +"。错误信息："+ ex.Message);
            }

            return result;
        }
        static int error = 0;

        /// <summary>
        /// 是否验证通过
        /// </summary>
        public static bool ResultOK
        {
            get { return error ==0; }
        }
        static bool ValdateItem(Control con, PropertyInfo pro, object value)
        {
            bool flag = true;
            object[] attrs = pro.GetCustomAttributes(false);

            for (int i = 0; i < attrs.Length; i++)
            {
                switch (attrs[i].GetType().Name)
                {
                    case ValdationType._Required:
                        {
                            RequiredAttribute attr = attrs[i] as RequiredAttribute;
                            IList list = value as IList;
                            if (list == null)
                            {
                                if (value == null || string.IsNullOrEmpty(value.ToString()))
                                {

                                    flag = false;
                                    con.SetValidation(attr.ErrorMessage);
                                    con.RaiseValidationError();
                                }
                            }
                            else if (list.Count == 0)
                            {
                                flag = false;
                                con.SetValidation(attr.ErrorMessage);
                                con.RaiseValidationError();
                            }
                            
                            

                        }
                        break;
                    case ValdationType._Range:
                        {
                            RangeAttribute attr = attrs[i] as RangeAttribute;
                            double max = double.Parse(attr.Maximum.ToString());
                            double min = double.Parse(attr.Minimum.ToString());
                            double val;
                            try
                            {
                                if (value != null || !string.IsNullOrEmpty(value.ToString()))
                                {
                                    val = double.Parse(value.ToString());
                                    if (val < min || val > max)
                                    {
                                        flag = false;
                                        con.SetValidation(attr.ErrorMessage);
                                        con.RaiseValidationError();
                                    }
                                }

                            }
                            catch (FormatException)
                            {
                                 
                                flag = false;
                                con.SetValidation("请输入正确的数字");
                                con.RaiseValidationError();
                            }

                        }
                        break;
                    case ValdationType._StringLength:
                        {
                            StringLengthAttribute attr = attrs[i] as StringLengthAttribute;
                            int max = attr.MaximumLength;
                            int min = attr.MinimumLength;
                            if (value != null || !string.IsNullOrEmpty(value.ToString()))
                            {
                                int length = value.ToString().Length;
                                if (length < min || length > max)
                                {
                                    
                                    flag = false;
                                    con.SetValidation(attr.ErrorMessage);
                                    con.RaiseValidationError();
                                }
                            }

                        }
                        break;
                    case ValdationType._RegularExpression:
                        {
                            RegularExpressionAttribute attr = attrs[i] as RegularExpressionAttribute;
                            string expression = attr.Pattern;

                            Regex r = new Regex(expression);
                            bool ismatch = r.IsMatch(value.ToString(), 0);
                            if (!ismatch)
                            {
                                
                                flag = false;
                                con.SetValidation(attr.ErrorMessage);
                                con.RaiseValidationError();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            if (flag)
                con.ClearValidationError();
            else {
                error++;
            }
             
            return flag;

        }
        /// <summary>
        /// 将实体绑定到容器
        /// </summary>
        /// <param name="grid">Grid容器</param>
        /// <param name="value">实体</param>
        public static void SetValue(Grid grid, object value)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    grid.SetValue(GridSourceProperty, value);
                });
            }
            else
            {
                grid.SetValue(GridSourceProperty, value);
            }
        }

        static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, ""));
            }
            return childList;
        }

        /// <summary>
        /// 对控件进行赋值
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        static void SetControlValue(Control control, object value)
        {
            ISetControlVal controlset = ControlFactory(control);

            controlset.SetValue(control, value);
        }

        static object GetControlValue(Control control)
        {
            ISetControlVal controlget = ControlFactory(control);
            return controlget.GetValue(control);
        }

        private static ISetControlVal ControlFactory(Control control)
        {
            ISetControlVal controlset = new TextBoxValOP();
            switch (control.GetType().Name)
            {
                case ControlType._TextBox:
                    controlset = new TextBoxValOP();
                    break;
                case ControlType._ComboBox:
                    controlset = new ComboBoxValOP();
                    break;
                case ControlType._CheckBox:
                    controlset = new CheckBoxValOP();
                    break;
                case ControlType._RadioButton:
                    controlset = new RadioButtonValOP();
                    break;
                case ControlType._ItemsControl:
                    controlset = new ItemsControlValOP();
                    break;
                case ControlType._DatePicker :
                    controlset = new DatePickerValOP();
                    break;
                case ControlType._ListBox:
                    controlset = new ListBoxValOP();
                    break;
                default:
                    throw new Exception(control.GetType().Name + "未在指定类型中");
            }
            return controlset;
        }



        static class ControlType
        {
            public const string _TextBox = "TextBox";
            public const string _ComboBox = "ComboBox";
            public const string _RadioButton = "RadioButton";
            public const string _CheckBox = "CheckBox";
            public const string _ItemsControl = "ItemsControl";
            public const string _DatePicker = "DatePicker";
            public const string _ListBox = "ListBox";
        }

        static class ValdationType
        {
            public const string _Range = "RangeAttribute";
            public const string _Required = "RequiredAttribute";
            public const string _StringLength = "StringLengthAttribute";
            public const string _RegularExpression = "RegularExpressionAttribute";

        }


        interface ISetControlVal
        {
            bool SetValue(object obj, object val);
            object GetValue(object obj);
        }



        class TextBoxValOP : ISetControlVal
        {

            public bool SetValue(object obj, object val)
            {
                bool flag = false;
                TextBox textbox = obj as TextBox;
                string textval = val.ToString();

                if (textbox == null)
                {
                    throw new Exception(obj.GetType().Name + "转换TextBox失败，导致赋值异常");
                }

                textbox.Text = textval;

                flag = true;
                return flag;
            }


            public object GetValue(object obj)
            {
                TextBox textbox = obj as TextBox;
                return textbox.Text;
            }
        }

        class ComboBoxValOP : ISetControlVal
        {

            public bool SetValue(object obj, object val)
            {
                bool flag = false;
                ComboBox comboBox = obj as ComboBox;
                IEnumerable comboval = val as IEnumerable;

                if (comboBox == null)
                {
                    throw new Exception(obj.GetType().Name + "转换TextBox失败，导致赋值异常");
                }

                if (comboval == null)
                {
                    throw new Exception(obj.GetType().Name + "ComboBox的赋值数据源非集合类型");
                }

                comboBox.ItemsSource = comboval;

                flag = true;
                return flag;
            }


            public object GetValue(object obj)
            {
                ComboBox comboBox = obj as ComboBox;

                return comboBox.SelectedItem;
            }
        }

        class CheckBoxValOP : ISetControlVal
        {

            public bool SetValue(object obj, object val)
            {
                bool flag = false;

                CheckBox checkBox = obj as CheckBox;
                bool checkVal = (bool)val;
                if (checkBox == null)
                    throw new Exception(obj.GetType().Name + "转换CheckBox失败，导致赋值异常");

                checkBox.IsChecked = checkVal;

                flag = true;
                return flag;
            }


            public object GetValue(object obj)
            {
                CheckBox checkBox = obj as CheckBox;
                return checkBox.IsChecked;
            }
        }

        class RadioButtonValOP : ISetControlVal
        {

            public bool SetValue(object obj, object val)
            {
                bool flag = false;
                RadioButton radioButton = obj as RadioButton;
                bool radioVal = (bool)val;
                if (radioButton == null)
                {
                    throw new Exception(obj.GetType().Name + "转换RadioButton失败，导致赋值异常");
                }

                radioButton.IsChecked = radioVal;
                flag = true;
                return flag;
            }


            public object GetValue(object obj)
            {
                RadioButton radioButton = obj as RadioButton;
                return radioButton.IsChecked;
            }
        }

        class ItemsControlValOP : ISetControlVal
        {

            public bool SetValue(object obj, object val)
            {
                bool flag = false;
                ItemsControl itemsControl = obj as ItemsControl;
                IEnumerable itemsVal = val as IEnumerable;

                if (itemsControl == null)
                {
                    throw new Exception(obj.GetType().Name + "转换ItemsControl失败，导致赋值异常");
                }

                itemsControl.ItemsSource = itemsVal;
                flag = true ;
                return flag;
            }


            public object GetValue(object obj)
            {
                ItemsControl itemsControl = obj as ItemsControl;
                return itemsControl.ItemsSource;
            }
        }

        class DatePickerValOP : ISetControlVal
        {

            public bool SetValue(object obj, object val)
            {
                bool flag = false;
                DatePicker datePick = obj as DatePicker;
                string date = val as string;
                if (datePick == null)
                {
                    throw new Exception(obj.GetType().Name +"转换DatePicker失败，导致赋值异常");
                }

                datePick.Text = date;
                flag = true;
                return flag;
                
            }

            public object GetValue(object obj)
            {
                DatePicker datePick = obj as DatePicker;
                return datePick.Text;
            }
        }

        class ListBoxValOP : ISetControlVal
        {

            public bool SetValue(object obj, object val)
            {
                bool flag = false;
                ListBox listbox = obj as ListBox;
                IEnumerable itemsVal = val as IEnumerable;

                if (listbox == null)
                {
                    throw new Exception(obj.GetType().Name + "转换Listbox失败，导致赋值异常");
                }

                listbox.ItemsSource = itemsVal;
                flag = true;
                return flag;
            }

            public object GetValue(object obj)
            {
                ListBox listbox = obj as ListBox;
                if (listbox.SelectionMode == SelectionMode.Single)
                {
                    return listbox.SelectedValue;
                }
                else
                {
                    IList values = listbox.SelectedItems;
                    List<string> results = new List<string>();
                    foreach (var item in values)
                    {
                        results.Add(item.ToString());
                    }
                    return results;
                }
                
            }
        }
    }
}
