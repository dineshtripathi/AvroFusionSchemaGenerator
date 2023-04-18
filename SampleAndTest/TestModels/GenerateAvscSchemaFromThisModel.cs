
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.Implementation.AvroTypeHandlers;

namespace TestModels
{
    public class GenerateAvscSchemaFromThisModelByClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string TypeDescription { get; set; }
        public DateTime GenerateDateTime { get; set; }
        public ValueClass ValueClass { get; set; }
        public DictionaryModel DictionaryModel { get; set; }
        public ListModel ListModel { get; set; }
        public Dictionary<string, ValueClass> DictionaryValueClasses { get; set; }
        public ValueType enumValueType { get; set; }
        [AvroUnionType(typeof(float), typeof(decimal))]
        public object FloatDecimalValue { get; set; }
    }


    public class DictionaryModel
    {
        public Dictionary<string, string> DictionaryKeyString { get; set; }
        public Dictionary<int, string> DictionaryKeyInt { get; set; }
        public Dictionary<int, ValueClass> DictionaryIntValueClasses { get; set; }
        public Dictionary<string, ValueClass> DictionaryStringValueClasses { get; set; }
        [AvroUnionType(typeof(int), typeof(double))]
        public object NumericValue { get; set; }
    }

    public class ListModel
    {
        public List<string> ListKeyString { get; set; }
        public List<int> ListKeyInt { get; set; }
    }

    public class ValueClass
    {
        public string Value { get; set; }
        public string ValueString { get; set; }
        public int ValueInt { get; set; }
        public double ValueDouble { get; set; }
        public float ValueFloat { get; set; }
        public DateTime ValueDateTime { get; set; }
        public decimal ValueDecimal { get; set; }
    }

    public enum ValueType
    {
        single,
        collection,
        list,
        tupple,
        primitive
    }


}