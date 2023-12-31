﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="ClubO7_Dev")]
	public partial class BannerDataDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertBanner(Banner instance);
    partial void UpdateBanner(Banner instance);
    partial void DeleteBanner(Banner instance);
    #endregion
		
		public BannerDataDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["ECubeCMSConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public BannerDataDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BannerDataDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BannerDataDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BannerDataDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Banner> Banners
		{
			get
			{
				return this.GetTable<Banner>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SearchBanner")]
		public ISingleResult<SearchBannerResult> SearchBanner([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Rows", DbType="Int")] System.Nullable<int> rows, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Page", DbType="Int")] System.Nullable<int> page, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Search", DbType="VarChar(500)")] string search, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Sort", DbType="VarChar(50)")] string sort)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), rows, page, search, sort);
			return ((ISingleResult<SearchBannerResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.DeleteBanner")]
		public ISingleResult<DeleteBannerResult> DeleteBanner([global::System.Data.Linq.Mapping.ParameterAttribute(Name="IdList", DbType="VarChar(50)")] string idList, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="DeletedBy", DbType="Int")] System.Nullable<int> deletedBy, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PageId", DbType="Int")] System.Nullable<int> pageId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), idList, deletedBy, pageId);
			return ((ISingleResult<DeleteBannerResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetBannerById")]
		public ISingleResult<GetBannerByIdResult> GetBannerById([global::System.Data.Linq.Mapping.ParameterAttribute(Name="BannerId", DbType="Int")] System.Nullable<int> bannerId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), bannerId);
			return ((ISingleResult<GetBannerByIdResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.InsertOrUpdateBanner")]
		public ISingleResult<InsertOrUpdateBannerResult> InsertOrUpdateBanner([global::System.Data.Linq.Mapping.ParameterAttribute(Name="BannerId", DbType="Int")] System.Nullable<int> bannerId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="BannerTitle", DbType="VarChar(300)")] string bannerTitle, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="BannerDescription", DbType="NVarChar(MAX)")] string bannerDescription, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="FromDate", DbType="DateTime")] System.Nullable<System.DateTime> fromDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ToDate", DbType="DateTime")] System.Nullable<System.DateTime> toDate, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Image1", DbType="VarChar(50)")] string image1, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Image2", DbType="VarChar(50)")] string image2, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Image3", DbType="VarChar(50)")] string image3, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Image4", DbType="VarChar(50)")] string image4, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Image5", DbType="VarChar(50)")] string image5, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IsActive", DbType="Bit")] System.Nullable<bool> isActive, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserId", DbType="Int")] System.Nullable<int> userId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="PageId", DbType="Int")] System.Nullable<int> pageId)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), bannerId, bannerTitle, bannerDescription, fromDate, toDate, image1, image2, image3, image4, image5, isActive, userId, pageId);
			return ((ISingleResult<InsertOrUpdateBannerResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Banner")]
	public partial class Banner : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _BannerId;
		
		private string _BannerTitle;
		
		private string _BannerDescription;
		
		private System.Nullable<System.DateTime> _FromDate;
		
		private System.Nullable<System.DateTime> _ToDate;
		
		private string _Image1;
		
		private string _Image2;
		
		private string _Image3;
		
		private string _Image4;
		
		private string _Image5;
		
		private bool _IsDeleted;
		
		private int _CreatedBy;
		
		private System.DateTime _CreatedOn;
		
		private System.Nullable<int> _UpdatedBy;
		
		private System.Nullable<System.DateTime> _UpdatedOn;
		
		private System.Nullable<int> _DeletedBy;
		
		private System.Nullable<System.DateTime> _DeletedOn;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnBannerIdChanging(int value);
    partial void OnBannerIdChanged();
    partial void OnBannerTitleChanging(string value);
    partial void OnBannerTitleChanged();
    partial void OnBannerDescriptionChanging(string value);
    partial void OnBannerDescriptionChanged();
    partial void OnFromDateChanging(System.Nullable<System.DateTime> value);
    partial void OnFromDateChanged();
    partial void OnToDateChanging(System.Nullable<System.DateTime> value);
    partial void OnToDateChanged();
    partial void OnImage1Changing(string value);
    partial void OnImage1Changed();
    partial void OnImage2Changing(string value);
    partial void OnImage2Changed();
    partial void OnImage3Changing(string value);
    partial void OnImage3Changed();
    partial void OnImage4Changing(string value);
    partial void OnImage4Changed();
    partial void OnImage5Changing(string value);
    partial void OnImage5Changed();
    partial void OnIsDeletedChanging(bool value);
    partial void OnIsDeletedChanged();
    partial void OnCreatedByChanging(int value);
    partial void OnCreatedByChanged();
    partial void OnCreatedOnChanging(System.DateTime value);
    partial void OnCreatedOnChanged();
    partial void OnUpdatedByChanging(System.Nullable<int> value);
    partial void OnUpdatedByChanged();
    partial void OnUpdatedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnUpdatedOnChanged();
    partial void OnDeletedByChanging(System.Nullable<int> value);
    partial void OnDeletedByChanged();
    partial void OnDeletedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnDeletedOnChanged();
    #endregion
		
		public Banner()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BannerId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int BannerId
		{
			get
			{
				return this._BannerId;
			}
			set
			{
				if ((this._BannerId != value))
				{
					this.OnBannerIdChanging(value);
					this.SendPropertyChanging();
					this._BannerId = value;
					this.SendPropertyChanged("BannerId");
					this.OnBannerIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BannerTitle", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string BannerTitle
		{
			get
			{
				return this._BannerTitle;
			}
			set
			{
				if ((this._BannerTitle != value))
				{
					this.OnBannerTitleChanging(value);
					this.SendPropertyChanging();
					this._BannerTitle = value;
					this.SendPropertyChanged("BannerTitle");
					this.OnBannerTitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BannerDescription", DbType="NVarChar(MAX)")]
		public string BannerDescription
		{
			get
			{
				return this._BannerDescription;
			}
			set
			{
				if ((this._BannerDescription != value))
				{
					this.OnBannerDescriptionChanging(value);
					this.SendPropertyChanging();
					this._BannerDescription = value;
					this.SendPropertyChanged("BannerDescription");
					this.OnBannerDescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FromDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> FromDate
		{
			get
			{
				return this._FromDate;
			}
			set
			{
				if ((this._FromDate != value))
				{
					this.OnFromDateChanging(value);
					this.SendPropertyChanging();
					this._FromDate = value;
					this.SendPropertyChanged("FromDate");
					this.OnFromDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ToDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> ToDate
		{
			get
			{
				return this._ToDate;
			}
			set
			{
				if ((this._ToDate != value))
				{
					this.OnToDateChanging(value);
					this.SendPropertyChanging();
					this._ToDate = value;
					this.SendPropertyChanged("ToDate");
					this.OnToDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image1", DbType="VarChar(50)")]
		public string Image1
		{
			get
			{
				return this._Image1;
			}
			set
			{
				if ((this._Image1 != value))
				{
					this.OnImage1Changing(value);
					this.SendPropertyChanging();
					this._Image1 = value;
					this.SendPropertyChanged("Image1");
					this.OnImage1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image2", DbType="VarChar(50)")]
		public string Image2
		{
			get
			{
				return this._Image2;
			}
			set
			{
				if ((this._Image2 != value))
				{
					this.OnImage2Changing(value);
					this.SendPropertyChanging();
					this._Image2 = value;
					this.SendPropertyChanged("Image2");
					this.OnImage2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image3", DbType="VarChar(50)")]
		public string Image3
		{
			get
			{
				return this._Image3;
			}
			set
			{
				if ((this._Image3 != value))
				{
					this.OnImage3Changing(value);
					this.SendPropertyChanging();
					this._Image3 = value;
					this.SendPropertyChanged("Image3");
					this.OnImage3Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image4", DbType="VarChar(50)")]
		public string Image4
		{
			get
			{
				return this._Image4;
			}
			set
			{
				if ((this._Image4 != value))
				{
					this.OnImage4Changing(value);
					this.SendPropertyChanging();
					this._Image4 = value;
					this.SendPropertyChanged("Image4");
					this.OnImage4Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image5", DbType="VarChar(50)")]
		public string Image5
		{
			get
			{
				return this._Image5;
			}
			set
			{
				if ((this._Image5 != value))
				{
					this.OnImage5Changing(value);
					this.SendPropertyChanging();
					this._Image5 = value;
					this.SendPropertyChanged("Image5");
					this.OnImage5Changed();
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedBy", DbType="Int NOT NULL")]
		public int CreatedBy
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdatedBy", DbType="Int")]
		public System.Nullable<int> UpdatedBy
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeletedBy", DbType="Int")]
		public System.Nullable<int> DeletedBy
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
	
	public partial class SearchBannerResult
	{
		
		private System.Nullable<long> _RowNum;
		
		private System.Nullable<int> _Total;
		
		private System.Nullable<int> _BannerId;
		
		private string _BannerTitle;
		
		private System.Nullable<System.DateTime> _FromDate;
		
		private System.Nullable<System.DateTime> _ToDate;
		
		public SearchBannerResult()
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BannerId", DbType="Int")]
		public System.Nullable<int> BannerId
		{
			get
			{
				return this._BannerId;
			}
			set
			{
				if ((this._BannerId != value))
				{
					this._BannerId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BannerTitle", DbType="VarChar(MAX)")]
		public string BannerTitle
		{
			get
			{
				return this._BannerTitle;
			}
			set
			{
				if ((this._BannerTitle != value))
				{
					this._BannerTitle = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FromDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> FromDate
		{
			get
			{
				return this._FromDate;
			}
			set
			{
				if ((this._FromDate != value))
				{
					this._FromDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ToDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> ToDate
		{
			get
			{
				return this._ToDate;
			}
			set
			{
				if ((this._ToDate != value))
				{
					this._ToDate = value;
				}
			}
		}
	}
	
	public partial class DeleteBannerResult
	{
		
		private int _TotalReference;
		
		private string _Name;
		
		public DeleteBannerResult()
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(250)")]
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
	
	public partial class GetBannerByIdResult
	{
		
		private int _BannerId;
		
		private string _BannerTitle;
		
		private string _BannerDescription;
		
		private string _FromDate;
		
		private string _ToDate;
		
		private string _Image1;
		
		private string _Image2;
		
		private string _Image3;
		
		private string _Image4;
		
		private string _Image5;
		
		private bool _IsActive;
		
		public GetBannerByIdResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BannerId", DbType="Int NOT NULL")]
		public int BannerId
		{
			get
			{
				return this._BannerId;
			}
			set
			{
				if ((this._BannerId != value))
				{
					this._BannerId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BannerTitle", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string BannerTitle
		{
			get
			{
				return this._BannerTitle;
			}
			set
			{
				if ((this._BannerTitle != value))
				{
					this._BannerTitle = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BannerDescription", DbType="NVarChar(MAX)")]
		public string BannerDescription
		{
			get
			{
				return this._BannerDescription;
			}
			set
			{
				if ((this._BannerDescription != value))
				{
					this._BannerDescription = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FromDate", DbType="VarChar(50)")]
		public string FromDate
		{
			get
			{
				return this._FromDate;
			}
			set
			{
				if ((this._FromDate != value))
				{
					this._FromDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ToDate", DbType="VarChar(50)")]
		public string ToDate
		{
			get
			{
				return this._ToDate;
			}
			set
			{
				if ((this._ToDate != value))
				{
					this._ToDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image1", DbType="VarChar(50)")]
		public string Image1
		{
			get
			{
				return this._Image1;
			}
			set
			{
				if ((this._Image1 != value))
				{
					this._Image1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image2", DbType="VarChar(50)")]
		public string Image2
		{
			get
			{
				return this._Image2;
			}
			set
			{
				if ((this._Image2 != value))
				{
					this._Image2 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image3", DbType="VarChar(50)")]
		public string Image3
		{
			get
			{
				return this._Image3;
			}
			set
			{
				if ((this._Image3 != value))
				{
					this._Image3 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image4", DbType="VarChar(50)")]
		public string Image4
		{
			get
			{
				return this._Image4;
			}
			set
			{
				if ((this._Image4 != value))
				{
					this._Image4 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Image5", DbType="VarChar(50)")]
		public string Image5
		{
			get
			{
				return this._Image5;
			}
			set
			{
				if ((this._Image5 != value))
				{
					this._Image5 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsActive", DbType="Bit NOT NULL")]
		public bool IsActive
		{
			get
			{
				return this._IsActive;
			}
			set
			{
				if ((this._IsActive != value))
				{
					this._IsActive = value;
				}
			}
		}
	}
	
	public partial class InsertOrUpdateBannerResult
	{
		
		private int _InsertedId;
		
		public InsertOrUpdateBannerResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InsertedId", DbType="Int NOT NULL")]
		public int InsertedId
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
}
#pragma warning restore 1591
