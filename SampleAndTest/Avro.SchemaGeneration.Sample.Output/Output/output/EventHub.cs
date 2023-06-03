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
	public partial class EventHub : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"EventHub\",\"namespace\":\"Avro.SchemaGeneration.Sample.Mode" +
				"l\",\"fields\":[{\"name\":\"EventHubName\",\"type\":\"string\"},{\"name\":\"PartitionCount\",\"t" +
				"ype\":\"int\"},{\"name\":\"MessageRetention\",\"type\":\"long\"}]}");
		private string _EventHubName;
		private int _PartitionCount;
		private long _MessageRetention;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return EventHub._SCHEMA;
			}
		}
		public string EventHubName
		{
			get
			{
				return this._EventHubName;
			}
			set
			{
				this._EventHubName = value;
			}
		}
		public int PartitionCount
		{
			get
			{
				return this._PartitionCount;
			}
			set
			{
				this._PartitionCount = value;
			}
		}
		public long MessageRetention
		{
			get
			{
				return this._MessageRetention;
			}
			set
			{
				this._MessageRetention = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.EventHubName;
			case 1: return this.PartitionCount;
			case 2: return this.MessageRetention;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.EventHubName = (System.String)fieldValue; break;
			case 1: this.PartitionCount = (System.Int32)fieldValue; break;
			case 2: this.MessageRetention = (System.Int64)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
