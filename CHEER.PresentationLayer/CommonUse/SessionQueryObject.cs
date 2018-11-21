using CHEER.CommonLayer.ePersonnel.Data;
using CHEER.Platform.DAL.SQLCenter;
using System;
//using CHEER.Interface;
namespace CHEER.PresentationLayer.CommonUse
{
	/// <summary>
	/// 查询session信息对象对象的基类，主要有查询条件，面板设置信息和页码
	/// </summary>
	[Serializable]
	public abstract class SessionQueryObject
	{
		private int _pageIndex;
		private PersonQueryData _personQueryData;
		private SQLSelectEntity _selectEntity;
		/// <summary>
		/// 页码
		/// </summary>
		public int PageIndex
		{
			set
			{
				this._pageIndex = value;
			}
			get
			{
				return this._pageIndex;
			}
		}
		/// <summary>
		/// 查询面板设置信息
		/// </summary>
		public PersonQueryData PersonQueryInfo
		{
			set
			{
				this._personQueryData = value;
			}
			get
			{
				return this._personQueryData;
			}
		}
		/// <summary>
		/// 查询条件
		/// </summary>
		public SQLSelectEntity SelectEntity
		{
			set
			{
				this._selectEntity = value;
			}
			get
			{
				return this._selectEntity;
			}
		}
		/// <summary>
		/// 使用页码，查询面板设置信息和查询条件初始化查询session对象
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="queryData"></param>
		/// <param name="selectEntity"></param>
		public SessionQueryObject(int pageIndex,PersonQueryData queryData,SQLSelectEntity selectEntity)
		{
			this.PageIndex = pageIndex;
			this.PersonQueryInfo = queryData;
			this.SelectEntity = selectEntity;
		}
	}
}


