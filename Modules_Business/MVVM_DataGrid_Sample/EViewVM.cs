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
using System.Collections.Generic;
using TKS.Common.MVVMLite;
using TKS.Common.MVVMLite.Command;
using TKS.Common.MVVMLite.Messaging;
using TKS.Common;
using TKS.DTO.Models;
using TKS.Controls;

namespace DataGrid_Sample_MVVM
{
    public class EViewVM : ViewModelBase
    {
        //是否编辑
        bool isEdit = false;
        public EViewVM()
        {
            Codes = new List<string> { "20", "30", "40" };

            CurrentCarApply = Messaging.Default.Receive<IC_CAR_APPLY_E, EViewVM>("edit");
            if (CurrentCarApply == null)
            {
                WindowName = "新增记录";
                CurrentCarApply = new IC_CAR_APPLY_E() { PLAN_START_TIME = DateTime.Now, PLAN_END_TIME = DateTime.Now };
            }
            else
            {
                WindowName = "编辑记录";
                isEdit = true;
            }
            SaveCommand = new RelayCommand<Grid>(SaveHandler);
            CancelCommand = new RelayCommand<ChildWindow>(CancelHandler);
        }

        private void CancelHandler(ChildWindow obj)
        {
            obj.Close();
        }
       

        private void SaveHandler(Grid obj)
        {
            GridEx.ValidateItems(obj);

            if (GridEx.ResultOK)
            {
                if (!isEdit)
                {
                    IsBusy = true;
                    CurrentCarApply.DECL_DATE = DateTime.Now;
                    RESTFulClient.SubmitRequest<int>("RESTfulS.BAL.IC_CAR_APPLY_I001", "Execute", (res) =>
                    {
                        IsBusy = false;
                        Message.InfoMessage("成功增加了" + res.ToString() + "行数据");
                    }, CurrentCarApply);
                }
                else
                {
                    IsBusy = true;
                    RESTFulClient.SubmitRequest<int>("RESTfulS.BAL.IC_CAR_APPLY_U001", "Execute", (res) =>
                    {
                        IsBusy = false;
                        Message.InfoMessage("成功更新了" + res.ToString() + "行数据");
                    }, CurrentCarApply);
                }
            }
            else
            {
                Message.WarnMessage("验证未通过");
            }
        }

        IC_CAR_APPLY_E _currentCarApply;
        public IC_CAR_APPLY_E CurrentCarApply
        {
            get { return _currentCarApply; }
            set
            {
                _currentCarApply = value;
                RaisePropertyChanged(() => this.CurrentCarApply);
            }
        }

        public RelayCommand<Grid> SaveCommand
        {
            get;
            set;
        }
        public RelayCommand<ChildWindow> CancelCommand
        {
            get;
            set;
        }
        //状态代码数据源
        public List<string> Codes
        {
            get;
            set;
        }

        //窗体名称
        public string WindowName
        {
            get;
            set;
        }

        //Busy
        bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => this.IsBusy);
            }
        }

        //栏位是否只读（新增、编辑）
        public bool IsReadOnly
        {
            get { return isEdit; }
            set { isEdit = value; }
        }
    }
}
