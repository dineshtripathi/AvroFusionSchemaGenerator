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
	public partial class AzureResource : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse(@"{""type"":""record"",""name"":""AzureResource"",""namespace"":""Avro.SchemaGeneration.Sample.Model"",""fields"":[{""name"":""Name"",""type"":""string""},{""name"":""ResourceType"",""type"":""string""},{""name"":""Location"",""type"":""string""},{""name"":""ResourceGroup"",""type"":""string""},{""name"":""SubscriptionId"",""type"":""string"",""aliases"":[""AzureResourceSubscriptionId""]},{""name"":""ETag"",""type"":""string""},{""name"":""AzureResourceTags"",""type"":{""type"":""map"",""values"":""string""}},{""name"":""ProvisioningState"",""type"":[""int"",""double"",""null""]}]}");
		private string _Name;
		private string _ResourceType;
		private string _Location;
		private string _ResourceGroup;
		private string _SubscriptionId;
		private string _ETag;
		private IDictionary<string,System.String> _AzureResourceTags;
		private object _ProvisioningState;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return AzureResource._SCHEMA;
			}
		}
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}
		public string ResourceType
		{
			get
			{
				return this._ResourceType;
			}
			set
			{
				this._ResourceType = value;
			}
		}
		public string Location
		{
			get
			{
				return this._Location;
			}
			set
			{
				this._Location = value;
			}
		}
		public string ResourceGroup
		{
			get
			{
				return this._ResourceGroup;
			}
			set
			{
				this._ResourceGroup = value;
			}
		}
		public string SubscriptionId
		{
			get
			{
				return this._SubscriptionId;
			}
			set
			{
				this._SubscriptionId = value;
			}
		}
		public string ETag
		{
			get
			{
				return this._ETag;
			}
			set
			{
				this._ETag = value;
			}
		}
		public IDictionary<string,System.String> AzureResourceTags
		{
			get
			{
				return this._AzureResourceTags;
			}
			set
			{
				this._AzureResourceTags = value;
			}
		}
		public object ProvisioningState
		{
			get
			{
				return this._ProvisioningState;
			}
			set
			{
				this._ProvisioningState = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.Name;
			case 1: return this.ResourceType;
			case 2: return this.Location;
			case 3: return this.ResourceGroup;
			case 4: return this.SubscriptionId;
			case 5: return this.ETag;
			case 6: return this.AzureResourceTags;
			case 7: return this.ProvisioningState;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.Name = (System.String)fieldValue; break;
			case 1: this.ResourceType = (System.String)fieldValue; break;
			case 2: this.Location = (System.String)fieldValue; break;
			case 3: this.ResourceGroup = (System.String)fieldValue; break;
			case 4: this.SubscriptionId = (System.String)fieldValue; break;
			case 5: this.ETag = (System.String)fieldValue; break;
			case 6: this.AzureResourceTags = (IDictionary<string,System.String>)fieldValue; break;
			case 7: this.ProvisioningState = (System.Object)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
