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
using TKS.Common.MVVMLite;
using System.Collections.Generic;
using TKS.DTO.Models;
using TKS.Common;
using TKS.Common.Navigator;
using System.Collections;
using TKS.Common.MVVMLite.Command;
using TKS.Common.MVVMLite.Messaging;
using TKS.Controls;
using System.Linq;
namespace DataGrid_Sample_MVVM
{
    public class DViewVM : ViewModelBase
    {
        public DViewVM()
        {
            try
            {
                currentCarApply = new IC_CAR_APPLY_D
                {
                    STATUS_CODE = "20",
                    USE_REASONS = "坐灰机",
                    PROC_OPINION = "",
                    PLAN_START_TIME = DateTime.Now.AddMonths(-20)
                };

                NavCommand = new RelayCommand(NavTo);
                QueryCommand = new RelayCommand<Grid>(QueryData);
                PageIndexChangedCommand = new RelayCommand<PageIndexArgs>(IndexChanged);
                DeleteCommand = new RelayCommand<IEnumerable<IC_CAR_APPLY_E>>(DeleteSelectedItem);
                NewAddCommand = new RelayCommand(NewAdd);
                EditCommand = new RelayCommand<IC_CAR_APPLY_E>(EditSelectedItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void NavTo()
        {
            Navigator.Default.NavigateTo(new MVVM_ViewListTemplate1());
        }

        private void EditSelectedItem(IC_CAR_APPLY_E obj)
        {
            Messaging.Default.Send<IC_CAR_APPLY_E, EViewVM>(obj, "edit");
            EView_Sample frm = new EView_Sample();
            frm.Show();
        }

        private void NewAdd()
        {
            EView_Sample frm = new EView_Sample();
            frm.Show();
        }

        private void DeleteSelectedItem(IEnumerable<IC_CAR_APPLY_E> obj)
        {
            List<IC_CAR_APPLY_E> list = obj as List<IC_CAR_APPLY_E>;
            IEnumerable<IC_CAR_APPLY_E> res = obj.Where((p) => p.IsChecked == true);
            if (res.Count() >= 2)
            {
                Message.WarnMessage("请不要选择多余1项");
            }
            else if (res.Count() <= 0)
            {
                Message.WarnMessage("请选择1项");
            }
            else
            {
                CommonBusy.IsBusy("请稍等...");
                RESTFulClient.SubmitRequest<int>("RESTfulS.BAL.IC_CAR_APPLY_D001", "Execute", (o) =>
                {
                    CommonBusy.IsNotBusy();
                    Message.InfoMessage("成功删除了" + o.ToString() + "行数据");
                }, res.ToList()[0]);
            }
        }

        private void IndexChanged(PageIndexArgs obj)
        {
            Query2(obj);
        }

        private void Query(PageIndexArgs obj)
        {
            CommonBusy.IsBusy("加载中...");
            string _selectSql = @"select  *
            from IC_CAR_APPLY   where  USE_REASONS like '%'||@USE_REASONS||'%' and PROC_OPINION like '%'||@PROC_OPINION||'%' and
            status_code=@STATUS_CODE  and plan_start_time>to_date(@PLAN_START_TIME,'yyyy/mm/dd hh24:mi:ss')";
            RESTFulClient.SubmitRequestOnlyForQuery<IEnumerable<IC_CAR_APPLY_E>>(_selectSql, obj.ViewIndex, 10, (P, total) =>
            {
                PageCount = total;
                PageSize = 10;
                CarApply = P as IEnumerable<IC_CAR_APPLY_E>;
                CommonBusy.IsNotBusy();
            }, queryWhere);
        }

        void Query2(PageIndexArgs obj)
        {
            CommonBusy.IsBusy("加载中...");
            RESTFulClient.SubmitRequest < IEnumerable<IC_CAR_APPLY_E>>("RESTfulS.BAL.IC_CAR_APPLY_S001", "Execute", (P) =>
            {
                PageCount = P.Count();
                PageSize = P.Count();
                CarApply = P as IEnumerable<IC_CAR_APPLY_E>;
                CommonBusy.IsNotBusy();
            }, queryWhere);
        }

        IC_CAR_APPLY_D queryWhere;
        private void QueryData(Grid grid)
        {
            GridEx.ValidateItems(grid);
            if (GridEx.ResultOK)
            {
                queryWhere = currentCarApply;
                Query2(new PageIndexArgs(0));
            }
        }

        //DataGrid数据源集合
        private IEnumerable<IC_CAR_APPLY_E> _carApply;
        public IEnumerable<IC_CAR_APPLY_E> CarApply
        {
            get { return _carApply; }
            set
            {
                _carApply = value;
                RaisePropertyChanged(() => this.CarApply);
            }
        }

        //总数
        private int _pageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
                RaisePropertyChanged(() => this.PageCount);
            }
        }
        //页大小
        private int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                RaisePropertyChanged(() => this.PageSize);
            }
        }

        //绑定到查询条件框的实体
        private IC_CAR_APPLY_D _currentCarApply;
        public IC_CAR_APPLY_D currentCarApply
        {
            get { return _currentCarApply; }
            set
            {
                _currentCarApply = value;
                RaisePropertyChanged(() => this.currentCarApply);
            }
        }

        public RelayCommand NavCommand
        {
            get;
            set;
        }

        //查询Command
        public RelayCommand<Grid> QueryCommand
        {
            get;
            set;
        }
        //翻页Command
        public RelayCommand<PageIndexArgs> PageIndexChangedCommand
        {
            get;
            set;
        }
        //删除Command
        public RelayCommand<IEnumerable<IC_CAR_APPLY_E>> DeleteCommand
        {
            get;
            set;
        }

        //新增Command
        public RelayCommand NewAddCommand
        {
            get;
            set;
        }

        //编辑Command
        public RelayCommand<IC_CAR_APPLY_E> EditCommand
        {
            get;
            set;
        }

       
    }
}
