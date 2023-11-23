﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EcubeWebPortalCMS.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Karnavati_QC")]
	public partial class ActivityLogDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertActivityLog(ActivityLog instance);
    partial void UpdateActivityLog(ActivityLog instance);
    partial void DeleteActivityLog(ActivityLog instance);
    #endregion
		
		public ActivityLogDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["ECubeCMSConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ActivityLogDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ActivityLogDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ActivityLogDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ActivityLogDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ActivityLog> ActivityLog
		{
			get
			{
				return this.GetTable<ActivityLog>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.InsertOrUpdateActivityLog")]
		public ISingleResult<InsertOrUpdateActivityLogResult> InsertOrUpdateActivityLog([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Id", DbType="BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserId", DbType="BigInt")] System.Nullable<long> userid, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PageId", DbType="BigInt")] System.Nullable<long> pageid, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="AuditComments", DbType="NVarChar(MAX)")] string auditcomments, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="TableName", DbType="NVarChar(50)")] string tablename, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="RecordId", DbType="BigInt")] System.Nullable<long> recordid, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="LoginUserId", DbType="BigInt")] System.Nullable<long> loginuserid, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PPageId", DbType="BigInt")] System.Nullable<long> ppageid)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, userid, pageid, auditcomments, tablename, recordid, loginuserid, ppageid);
			return ((ISingleResult<InsertOrUpdateActivityLogResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchActivityLog")]
		public ISingleResult<SearchActivityLogResult> SearchActivityLog([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Rows", DbType="Int")] System.Nullable<int> rows, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Page", DbType="Int")] System.Nullable<int> page, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Search", DbType="NVarChar(500)")] string search, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Sort", DbType="NVarChar(50)")] string sort)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), rows, page, search, sort);
			return ((ISingleResult<SearchActivityLogResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ActivityLog")]
	public partial class ActivityLog : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private long _UserId;
		
		private long _PageId;
		
		private string _AuditComments;
		
		private string _TableName;
		
		private long _RecordId;
		
		private System.DateTime _CreatedOn;
		
		private long _CreatedBy;
		
		private System.Nullable<System.DateTime> _UpdatedOn;
		
		private System.Nullable<long> _UpdatedBy;
		
		private System.Nullable<System.DateTime> _DeletedOn;
		
		private System.Nullable<long> _DeletedBy;
		
		private bool _IsDeleted;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnUserIdChanging(long value);
    partial void OnUserIdChanged();
    partial void OnPageIdChanging(long value);
    partial void OnPageIdChanged();
    partial void OnAuditCommentsChanging(string value);
    partial void OnAuditCommentsChanged();
    partial void OnTableNameChanging(string value);
    partial void OnTableNameChanged();
    partial void OnRecordIdChanging(long value);
    partial void OnRecordIdChanged();
    partial void OnCreatedOnChanging(System.DateTime value);
    partial void OnCreatedOnChanged();
    partial void OnCreatedByChanging(long value);
    partial void OnCreatedByChanged();
    partial void OnUpdatedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnUpdatedOnChanged();
    partial void OnUpdatedByChanging(System.Nullable<long> value);
    partial void OnUpdatedByChanged();
    partial void OnDeletedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnDeletedOnChanged();
    partial void OnDeletedByChanging(System.Nullable<long> value);
    partial void OnDeletedByChanged();
    partial void OnIsDeletedChanging(bool value);
    partial void OnIsDeletedChanged();
    #endregion
		
		public ActivityLog()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="BigInt NOT NULL")]
		public long UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PageId", DbType="BigInt NOT NULL")]
		public long PageId
		{
			get
			{
				return this._PageId;
			}
			set
			{
				if ((this._PageId != value))
				{
					this.OnPageIdChanging(value);
					this.SendPropertyChanging();
					this._PageId = value;
					this.SendPropertyChanged("PageId");
					this.OnPageIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AuditComments", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string AuditComments
		{
			get
			{
				return this._AuditComments;
			}
			set
			{
				if ((this._AuditComments != value))
				{
					this.OnAuditCommentsChanging(value);
					this.SendPropertyChanging();
					this._AuditComments = value;
					this.SendPropertyChanged("AuditComments");
					this.OnAuditCommentsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TableName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string TableName
		{
			get
			{
				return this._TableName;
			}
			set
			{
				if ((this._TableName != value))
				{
					this.OnTableNameChanging(value);
					this.SendPropertyChanging();
					this._TableName = value;
					this.SendPropertyChanged("TableName");
					this.OnTableNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RecordId", DbType="BigInt NOT NULL")]
		public long RecordId
		{
			get
			{
				return this._RecordId;
			}
			set
			{
				if ((this._RecordId != value))
				{
					this.OnRecordIdChanging(value);
					this.SendPropertyChanging();
					this._RecordId = value;
					this.SendPropertyChanged("RecordId");
					this.OnRecordIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedOn", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedOn
		{
			get
			{
				return this._CreatedOn;
			}
			set
			{
				if ((this._CreatedOn != value))
				{
					this.OnCreatedOnChanging(value);
					this.SendPropertyChanging();
					this._CreatedOn = value;
					this.SendPropertyChanged("CreatedOn");
					this.OnCreatedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedBy", DbType="BigInt NOT NULL")]
		public long CreatedBy
		{
			get
			{
				return this._CreatedBy;
			}
			set
			{
				if ((this._CreatedBy != value))
				{
					this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdatedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> UpdatedOn
		{
			get
			{
				return this._UpdatedOn;
			}
			set
			{
				if ((this._UpdatedOn != value))
				{
					this.OnUpdatedOnChanging(value);
					this.SendPropertyChanging();
					this._UpdatedOn = value;
					this.SendPropertyChanged("UpdatedOn");
					this.OnUpdatedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdatedBy", DbType="BigInt")]
		public System.Nullable<long> UpdatedBy
		{
			get
			{
				return this._UpdatedBy;
			}
			set
			{
				if ((this._UpdatedBy != value))
				{
					this.OnUpdatedByChanging(value);
					this.SendPropertyChanging();
					this._UpdatedBy = value;
					this.SendPropertyChanged("UpdatedBy");
					this.OnUpdatedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeletedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> DeletedOn
		{
			get
			{
				return this._DeletedOn;
			}
			set
			{
				if ((this._DeletedOn != value))
				{
					this.OnDeletedOnChanging(value);
					this.SendPropertyChanging();
					this._DeletedOn = value;
					this.SendPropertyChanged("DeletedOn");
					this.OnDeletedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeletedBy", DbType="BigInt")]
		public System.Nullable<long> DeletedBy
		{
			get
			{
				return this._DeletedBy;
			}
			set
			{
				if ((this._DeletedBy != value))
				{
					this.OnDeletedByChanging(value);
					this.SendPropertyChanging();
					this._DeletedBy = value;
					this.SendPropertyChanged("DeletedBy");
					this.OnDeletedByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsDeleted", DbType="Bit NOT NULL")]
		public bool IsDeleted
		{
			get
			{
				return this._IsDeleted;
			}
			set
			{
				if ((this._IsDeleted != value))
				{
					this.OnIsDeletedChanging(value);
					this.SendPropertyChanging();
					this._IsDeleted = value;
					this.SendPropertyChanged("IsDeleted");
					this.OnIsDeletedChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	public partial class InsertOrUpdateActivityLogResult
	{
		
		private long _InsertedId;
		
		public InsertOrUpdateActivityLogResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InsertedId", DbType="BigInt NOT NULL")]
		public long InsertedId
		{
			get
			{
				return this._InsertedId;
			}
			set
			{
				if ((this._InsertedId != value))
				{
					this._InsertedId = value;
				}
			}
		}
	}
	
	public partial class SearchActivityLogResult
	{
		
		private System.Nullable<int> _RowNum;
		
		private int _Total;
		
		private long _Id;
		
		private long _UserId;
		
		private string _UserName;
		
		private long _PageId;
		
		private string _PageName;
		
		private string _AuditComments;
		
		private string _TableName;
		
		private long _RecordId;
		
		private System.DateTime _CreatedOn;
		
		public SearchActivityLogResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RowNum", DbType="Int")]
		public System.Nullable<int> RowNum
		{
			get
			{
				return this._RowNum;
			}
			set
			{
				if ((this._RowNum != value))
				{
					this._RowNum = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Total", DbType="Int NOT NULL")]
		public int Total
		{
			get
			{
				return this._Total;
			}
			set
			{
				if ((this._Total != value))
				{
					this._Total = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="BigInt NOT NULL")]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this._Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="BigInt NOT NULL")]
		public long UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this._UserId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(MAX)")]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this._UserName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PageId", DbType="BigInt NOT NULL")]
		public long PageId
		{
			get
			{
				return this._PageId;
			}
			set
			{
				if ((this._PageId != value))
				{
					this._PageId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PageName", DbType="NVarChar(MAX)")]
		public string PageName
		{
			get
			{
				return this._PageName;
			}
			set
			{
				if ((this._PageName != value))
				{
					this._PageName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AuditComments", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string AuditComments
		{
			get
			{
				return this._AuditComments;
			}
			set
			{
				if ((this._AuditComments != value))
				{
					this._AuditComments = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TableName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string TableName
		{
			get
			{
				return this._TableName;
			}
			set
			{
				if ((this._TableName != value))
				{
					this._TableName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RecordId", DbType="BigInt NOT NULL")]
		public long RecordId
		{
			get
			{
				return this._RecordId;
			}
			set
			{
				if ((this._RecordId != value))
				{
					this._RecordId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedOn", DbType="DateTime NOT NULL")]
		public System.DateTime CreatedOn
		{
			get
			{
				return this._CreatedOn;
			}
			set
			{
				if ((this._CreatedOn != value))
				{
					this._CreatedOn = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
