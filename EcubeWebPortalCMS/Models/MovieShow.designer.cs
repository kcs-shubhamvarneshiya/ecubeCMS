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


    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "EcubeWebPortalCMS")]
	public partial class MovieShowDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertMovieShow(MovieShow instance);
    partial void UpdateMovieShow(MovieShow instance);
    partial void DeleteMovieShow(MovieShow instance);
    #endregion
		
		public MovieShowDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["EcubeCMSConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public MovieShowDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MovieShowDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MovieShowDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MovieShowDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<MovieShow> MovieShow
		{
			get
			{
				return this.GetTable<MovieShow>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.InsertOrUpdateMovieShow")]
		public ISingleResult<InsertOrUpdateMovieShowResult> InsertOrUpdateMovieShow([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Id", DbType="BigInt")] System.Nullable<long> id, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="MovieShowName", DbType="NVarChar(50)")] string movieshowname, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ShowStartDate", DbType="DateTime")] System.Nullable<System.DateTime> showstartdate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ShowEndDate", DbType="DateTime")] System.Nullable<System.DateTime> showenddate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserId", DbType="BigInt")] System.Nullable<long> userid, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PageId", DbType="BigInt")] System.Nullable<long> pageid)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, movieshowname, showstartdate, showenddate, userid, pageid);
			return ((ISingleResult<InsertOrUpdateMovieShowResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetMovieShowAll")]
		public ISingleResult<GetMovieShowAllResult> GetMovieShowAll()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<GetMovieShowAllResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetMovieShowById")]
		public ISingleResult<GetMovieShowByIdResult> GetMovieShowById([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Id", DbType="BigInt")] System.Nullable<long> id)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), id);
			return ((ISingleResult<GetMovieShowByIdResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.DeleteMovieShow")]
		public ISingleResult<DeleteMovieShowResult> DeleteMovieShow([global::System.Data.Linq.Mapping.ParameterAttribute(Name="IdList", DbType="NVarChar(MAX)")] string idlist, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="DeletedBy", DbType="BigInt")] System.Nullable<long> deletedby, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PageId", DbType="BigInt")] System.Nullable<long> pageid)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), idlist, deletedby, pageid);
			return ((ISingleResult<DeleteMovieShowResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.CountMovieShow")]
		public ISingleResult<CountMovieShowResult> CountMovieShow()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<CountMovieShowResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchMovieShow")]
		public ISingleResult<SearchMovieShowResult> SearchMovieShow([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Rows", DbType="Int")] System.Nullable<int> rows, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Page", DbType="Int")] System.Nullable<int> page, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Search", DbType="NVarChar(500)")] string search, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Sort", DbType="NVarChar(50)")] string sort)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), rows, page, search, sort);
			return ((ISingleResult<SearchMovieShowResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MovieShow")]
	public partial class MovieShow : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _MovieShowName;
		
		private System.DateTime _ShowStartDate;
		
		private System.DateTime _ShowEndDate;
		
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
    partial void OnMovieShowNameChanging(string value);
    partial void OnMovieShowNameChanged();
    partial void OnShowStartDateChanging(System.DateTime value);
    partial void OnShowStartDateChanged();
    partial void OnShowEndDateChanging(System.DateTime value);
    partial void OnShowEndDateChanged();
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
		
		public MovieShow()
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MovieShowName", DbType="NVarChar(50)")]
		public string MovieShowName
		{
			get
			{
				return this._MovieShowName;
			}
			set
			{
				if ((this._MovieShowName != value))
				{
					this.OnMovieShowNameChanging(value);
					this.SendPropertyChanging();
					this._MovieShowName = value;
					this.SendPropertyChanged("MovieShowName");
					this.OnMovieShowNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ShowStartDate", DbType="DateTime NOT NULL")]
		public System.DateTime ShowStartDate
		{
			get
			{
				return this._ShowStartDate;
			}
			set
			{
				if ((this._ShowStartDate != value))
				{
					this.OnShowStartDateChanging(value);
					this.SendPropertyChanging();
					this._ShowStartDate = value;
					this.SendPropertyChanged("ShowStartDate");
					this.OnShowStartDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ShowEndDate", DbType="DateTime NOT NULL")]
		public System.DateTime ShowEndDate
		{
			get
			{
				return this._ShowEndDate;
			}
			set
			{
				if ((this._ShowEndDate != value))
				{
					this.OnShowEndDateChanging(value);
					this.SendPropertyChanging();
					this._ShowEndDate = value;
					this.SendPropertyChanged("ShowEndDate");
					this.OnShowEndDateChanged();
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
	
	public partial class InsertOrUpdateMovieShowResult
	{
		
		private long _InsertedId;
		
		public InsertOrUpdateMovieShowResult()
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
	
	public partial class GetMovieShowAllResult
	{
		
		private long _Id;
		
		private string _MovieShowName;
		
		private System.DateTime _ShowStartDate;
		
		private System.DateTime _ShowEndDate;
		
		public GetMovieShowAllResult()
		{
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MovieShowName", DbType="NVarChar(50)")]
		public string MovieShowName
		{
			get
			{
				return this._MovieShowName;
			}
			set
			{
				if ((this._MovieShowName != value))
				{
					this._MovieShowName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ShowStartDate", DbType="DateTime NOT NULL")]
		public System.DateTime ShowStartDate
		{
			get
			{
				return this._ShowStartDate;
			}
			set
			{
				if ((this._ShowStartDate != value))
				{
					this._ShowStartDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ShowEndDate", DbType="DateTime NOT NULL")]
		public System.DateTime ShowEndDate
		{
			get
			{
				return this._ShowEndDate;
			}
			set
			{
				if ((this._ShowEndDate != value))
				{
					this._ShowEndDate = value;
				}
			}
		}
	}
	
	public partial class GetMovieShowByIdResult
	{
		
		private long _Id;
		
		private string _MovieShowName;
		
		private System.DateTime _ShowStartDate;
		
		private System.DateTime _ShowEndDate;
		
		public GetMovieShowByIdResult()
		{
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MovieShowName", DbType="NVarChar(50)")]
		public string MovieShowName
		{
			get
			{
				return this._MovieShowName;
			}
			set
			{
				if ((this._MovieShowName != value))
				{
					this._MovieShowName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ShowStartDate", DbType="DateTime NOT NULL")]
		public System.DateTime ShowStartDate
		{
			get
			{
				return this._ShowStartDate;
			}
			set
			{
				if ((this._ShowStartDate != value))
				{
					this._ShowStartDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ShowEndDate", DbType="DateTime NOT NULL")]
		public System.DateTime ShowEndDate
		{
			get
			{
				return this._ShowEndDate;
			}
			set
			{
				if ((this._ShowEndDate != value))
				{
					this._ShowEndDate = value;
				}
			}
		}
	}
	
	public partial class DeleteMovieShowResult
	{
		
		private int _TotalReference;
		
		private string _Name;
		
		public DeleteMovieShowResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TotalReference", DbType="Int NOT NULL")]
		public int TotalReference
		{
			get
			{
				return this._TotalReference;
			}
			set
			{
				if ((this._TotalReference != value))
				{
					this._TotalReference = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
	}
	
	public partial class CountMovieShowResult
	{
		
		private int _Result;
		
		public CountMovieShowResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Result", DbType="Int NOT NULL")]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				if ((this._Result != value))
				{
					this._Result = value;
				}
			}
		}
	}
	
	public partial class SearchMovieShowResult
	{
		
		private System.Nullable<long> _RowNum;
		
		private System.Nullable<int> _Total;
		
		private long _Id;
		
		private string _MovieShowName;
		
		private System.DateTime _ShowStartDate;
		
		private System.DateTime _ShowEndDate;
		
		public SearchMovieShowResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RowNum", DbType="BigInt")]
		public System.Nullable<long> RowNum
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Total", DbType="Int")]
		public System.Nullable<int> Total
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MovieShowName", DbType="NVarChar(50)")]
		public string MovieShowName
		{
			get
			{
				return this._MovieShowName;
			}
			set
			{
				if ((this._MovieShowName != value))
				{
					this._MovieShowName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ShowStartDate", DbType="DateTime NOT NULL")]
		public System.DateTime ShowStartDate
		{
			get
			{
				return this._ShowStartDate;
			}
			set
			{
				if ((this._ShowStartDate != value))
				{
					this._ShowStartDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ShowEndDate", DbType="DateTime NOT NULL")]
		public System.DateTime ShowEndDate
		{
			get
			{
				return this._ShowEndDate;
			}
			set
			{
				if ((this._ShowEndDate != value))
				{
					this._ShowEndDate = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
