// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.1
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Avro.SchemaGeneration.Sample.Model
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.1")]
	public partial class StorageAccount : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""StorageAccount"",""namespace"":""Avro.SchemaGeneration.Sample.Model"",""fields"":[{""name"":""StorageAccountName"",""type"":""string""},{""name"":""AccountType"",""type"":""string""},{""name"":""AccessTier"",""type"":""string""},{""name"":""IsSecureTransferEnabled"",""type"":""boolean""}]}");
		private string _StorageAccountName;
		private string _AccountType;
		private string _AccessTier;
		private bool _IsSecureTransferEnabled;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return StorageAccount._SCHEMA;
			}
		}
		public string StorageAccountName
		{
			get
			{
				return this._StorageAccountName;
			}
			set
			{
				this._StorageAccountName = value;
			}
		}
		public string AccountType
		{
			get
			{
				return this._AccountType;
			}
			set
			{
				this._AccountType = value;
			}
		}
		public string AccessTier
		{
			get
			{
				return this._AccessTier;
			}
			set
			{
				this._AccessTier = value;
			}
		}
		public bool IsSecureTransferEnabled
		{
			get
			{
				return this._IsSecureTransferEnabled;
			}
			set
			{
				this._IsSecureTransferEnabled = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.StorageAccountName;
			case 1: return this.AccountType;
			case 2: return this.AccessTier;
			case 3: return this.IsSecureTransferEnabled;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.StorageAccountName = (System.String)fieldValue; break;
			case 1: this.AccountType = (System.String)fieldValue; break;
			case 2: this.AccessTier = (System.String)fieldValue; break;
			case 3: this.IsSecureTransferEnabled = (System.Boolean)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}