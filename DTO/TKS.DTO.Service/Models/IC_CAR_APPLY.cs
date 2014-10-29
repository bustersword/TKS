// ***********************************************************************
// Author           : Peter.Zhao
// Created          : 2014/8/20 11:11:58
//
// ***********************************************************************
// file="IC_CAR_APPLY.cs" 
//     Copyright (c) . All rights reserved.
// 
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using TKS.DTO.Core;
#if SILVERLIGHT
#else
using Zlib.DbUtilities.Interface;
#endif
namespace TKS.DTO.Models
{
    ///<summary>
    ///用车申请
    ///</summary>
    public class IC_CAR_APPLY : BaseModel
    {


#if !SILVERLIGHT
        [PrimaryKey]
#endif
        /// <summary>
        /// 记录编号
        /// </summary>		
        public virtual string DOC_NO
        {
            get { return GetDataField<string>("DOC_NO"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "DOC_NO" });
                SetDataField<string>("DOC_NO", value);
                RaisePropertyChanged(() => this.DOC_NO);
            }
        }
        /// <summary>
        /// 状态代码
        /// </summary>		
        public virtual string STATUS_CODE
        {
            get { return GetDataField<string>("STATUS_CODE"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "STATUS_CODE" });
                SetDataField<string>("STATUS_CODE", value);
                RaisePropertyChanged(() => this.STATUS_CODE);
            }
        }
        /// <summary>
        /// 状态名称
        /// </summary>		
        public virtual string STATUS_NAME
        {
            get { return GetDataField<string>("STATUS_NAME"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "STATUS_NAME" });
                SetDataField<string>("STATUS_NAME", value);
                RaisePropertyChanged(() => this.STATUS_NAME);
            }
        }
        /// <summary>
        /// 申请时间
        /// </summary>		
        public virtual DateTime DECL_DATE
        {
            get { return GetDataField<DateTime>("DECL_DATE"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "DECL_DATE" });
                SetDataField<DateTime>("DECL_DATE", value);
                RaisePropertyChanged(() => this.DECL_DATE);
            }
        }
        /// <summary>
        /// 申请人编码
        /// </summary>		
        public virtual string DECL_MAN_NO
        {
            get { return GetDataField<string>("DECL_MAN_NO"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "DECL_MAN_NO" });
                SetDataField<string>("DECL_MAN_NO", value);
                RaisePropertyChanged(() => this.DECL_MAN_NO);
            }
        }
        /// <summary>
        /// 申请人名称
        /// </summary>		
        public virtual string DECL_MAN_NAME
        {
            get { return GetDataField<string>("DECL_MAN_NAME"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "DECL_MAN_NAME" });
                SetDataField<string>("DECL_MAN_NAME", value);
                RaisePropertyChanged(() => this.DECL_MAN_NAME);
            }
        }
        /// <summary>
        /// 下级审批人编号
        /// </summary>		
        public virtual string NEXT_APP_MAN_NO
        {
            get { return GetDataField<string>("NEXT_APP_MAN_NO"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "NEXT_APP_MAN_NO" });
                SetDataField<string>("NEXT_APP_MAN_NO", value);
                RaisePropertyChanged(() => this.NEXT_APP_MAN_NO);
            }
        }
        /// <summary>
        /// 下级审批人名称
        /// </summary>		
        public virtual string NEXT_APP_MAN_NAME
        {
            get { return GetDataField<string>("NEXT_APP_MAN_NAME"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "NEXT_APP_MAN_NAME" });
                SetDataField<string>("NEXT_APP_MAN_NAME", value);
                RaisePropertyChanged(() => this.NEXT_APP_MAN_NAME);
            }
        }
        /// <summary>
        /// 备注
        /// </summary>		
        public virtual string REMARK
        {
            get { return GetDataField<string>("REMARK"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "REMARK" });
                SetDataField<string>("REMARK", value);
                RaisePropertyChanged(() => this.REMARK);
            }
        }
        /// <summary>
        /// 处理意见
        /// </summary>		
        public virtual string PROC_OPINION
        {
            get { return GetDataField<string>("PROC_OPINION"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "PROC_OPINION" });
                SetDataField<string>("PROC_OPINION", value);
                RaisePropertyChanged(() => this.PROC_OPINION);
            }
        }
        /// <summary>
        /// 申请科室编码
        /// </summary>		
        public virtual string DECL_DEPT_NO
        {
            get { return GetDataField<string>("DECL_DEPT_NO"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "DECL_DEPT_NO" });
                SetDataField<string>("DECL_DEPT_NO", value);
                RaisePropertyChanged(() => this.DECL_DEPT_NO);
            }
        }
        /// <summary>
        /// 申请科室名称
        /// </summary>		
        public virtual string DECL_DEPT_NAME
        {
            get { return GetDataField<string>("DECL_DEPT_NAME"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "DECL_DEPT_NAME" });
                SetDataField<string>("DECL_DEPT_NAME", value);
                RaisePropertyChanged(() => this.DECL_DEPT_NAME);
            }
        }
        /// <summary>
        /// 短途/长途
        /// </summary>		
        public virtual decimal LONG_OR_SHORT
        {
            get { return GetDataField<decimal>("LONG_OR_SHORT"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "LONG_OR_SHORT" });
                SetDataField<decimal>("LONG_OR_SHORT", value);
                RaisePropertyChanged(() => this.LONG_OR_SHORT);
            }
        }
        /// <summary>
        /// 人数
        /// </summary>		
        public virtual decimal MAN_NO
        {
            get { return GetDataField<decimal>("MAN_NO"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "MAN_NO" });
                SetDataField<decimal>("MAN_NO", value);
                RaisePropertyChanged(() => this.MAN_NO);
            }
        }
        /// <summary>
        /// 车型
        /// </summary>		
        public virtual string CAR_TYPE
        {
            get { return GetDataField<string>("CAR_TYPE"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "CAR_TYPE" });
                SetDataField<string>("CAR_TYPE", value);
                RaisePropertyChanged(() => this.CAR_TYPE);
            }
        }
        /// <summary>
        /// 计划出车时间
        /// </summary>		
        public virtual DateTime PLAN_START_TIME
        {
            get { return GetDataField<DateTime>("PLAN_START_TIME"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "PLAN_START_TIME" });
                SetDataField<DateTime>("PLAN_START_TIME", value);
                RaisePropertyChanged(() => this.PLAN_START_TIME);
            }
        }
        /// <summary>
        /// 计划返回时间
        /// </summary>		
        public virtual DateTime PLAN_END_TIME
        {
            get { return GetDataField<DateTime>("PLAN_END_TIME"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "PLAN_END_TIME" });
                SetDataField<DateTime>("PLAN_END_TIME", value);
                RaisePropertyChanged(() => this.PLAN_END_TIME);
            }
        }
        /// <summary>
        /// 起始地
        /// </summary>		
        public virtual string START_ADD
        {
            get { return GetDataField<string>("START_ADD"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "START_ADD" });
                SetDataField<string>("START_ADD", value);
                RaisePropertyChanged(() => this.START_ADD);
            }
        }
        /// <summary>
        /// 目的地
        /// </summary>		
        public virtual string DEST_ADD
        {
            get { return GetDataField<string>("DEST_ADD"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "DEST_ADD" });
                SetDataField<string>("DEST_ADD", value);
                RaisePropertyChanged(() => this.DEST_ADD);
            }
        }
        /// <summary>
        /// 用车事由
        /// </summary>		
        public virtual string USE_REASONS
        {
            get { return GetDataField<string>("USE_REASONS"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "USE_REASONS" });
                SetDataField<string>("USE_REASONS", value);
                RaisePropertyChanged(() => this.USE_REASONS);
            }
        }
        /// <summary>
        /// 安排车牌号码
        /// </summary>		
        public virtual string CAR_NO
        {
            get { return GetDataField<string>("CAR_NO"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "CAR_NO" });
                SetDataField<string>("CAR_NO", value);
                RaisePropertyChanged(() => this.CAR_NO);
            }
        }
        /// <summary>
        /// 工作ID
        /// </summary>		
        public virtual string WID
        {
            get { return GetDataField<string>("WID"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "WID" });
                SetDataField<string>("WID", value);
                RaisePropertyChanged(() => this.WID);
            }
        }
        /// <summary>
        /// 流程ID
        /// </summary>		
        public virtual string FID
        {
            get { return GetDataField<string>("FID"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "FID" });
                SetDataField<string>("FID", value);
                RaisePropertyChanged(() => this.FID);
            }
        }
        /// <summary>
        /// 当前节点
        /// </summary>		
        public virtual string CURNODE
        {
            get { return GetDataField<string>("CURNODE"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "CURNODE" });
                SetDataField<string>("CURNODE", value);
                RaisePropertyChanged(() => this.CURNODE);
            }
        }
        /// <summary>
        /// 下级审批人
        /// </summary>		
        public virtual string POINTTOEMP
        {
            get { return GetDataField<string>("POINTTOEMP"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "POINTTOEMP" });
                SetDataField<string>("POINTTOEMP", value);
                RaisePropertyChanged(() => this.POINTTOEMP);
            }
        }
        /// <summary>
        /// 下级审批岗位
        /// </summary>		
        public virtual string POINTTOSTAT
        {
            get { return GetDataField<string>("POINTTOSTAT"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "POINTTOSTAT" });
                SetDataField<string>("POINTTOSTAT", value);
                RaisePropertyChanged(() => this.POINTTOSTAT);
            }
        }
        /// <summary>
        /// 下级审批部门
        /// </summary>		
        public virtual string POINTTODEPT
        {
            get { return GetDataField<string>("POINTTODEPT"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "POINTTODEPT" });
                SetDataField<string>("POINTTODEPT", value);
                RaisePropertyChanged(() => this.POINTTODEPT);
            }
        }
        /// <summary>
        /// 经办人
        /// </summary>		
        public virtual string GESTOR
        {
            get { return GetDataField<string>("GESTOR"); }
            set
            {
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "GESTOR" });
                SetDataField<string>("GESTOR", value);
                RaisePropertyChanged(() => this.GESTOR);
            }
        }

    }
}

