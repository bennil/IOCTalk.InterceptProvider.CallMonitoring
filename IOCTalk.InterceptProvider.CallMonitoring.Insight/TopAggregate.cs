using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCTalk.InterceptProvider.CallMonitoring.Insight
{
    internal class TopAggregate<ValueType> 
        where ValueType : struct
    {
        public Type InterfaceType { get; init; }

        public string ServiceMethod { get; init; }

        public ValueType Value { get; init; }

        static readonly TopAggregateComparer<ValueType> valueComparer = new TopAggregateComparer<ValueType>();

        public static TopAggregateComparer<ValueType> ValueComparer => valueComparer;
    }

    internal class TopAggregateComparer<ValueType> : IComparer<TopAggregate<ValueType>>
        where ValueType : struct
    {
        public int Compare(TopAggregate<ValueType> x, TopAggregate<ValueType> y)
        {
            int result = Comparer<ValueType>.Default.Compare(x.Value, y.Value);
            return result;
        }
    }
}
