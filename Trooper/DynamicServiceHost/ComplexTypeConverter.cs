//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Trooper.DynamicServiceHost.Exceptions;

//namespace Trooper.DynamicServiceHost
//{
//    public class ComplexTypeConverter
//    {
//        public Type Input { get; private set; }

//        public Type Output { get; private set; }

//        public string Name { get; private set; }

//        private Func<object, object> converter;

//        private ComplexTypeConverter(Func<object, object> converter, string name)
//        {
//            this.Input = converter.Method.GetParameters().First().ParameterType;
//            this.Output = converter.Method.ReturnType;
//            this.converter = converter;
//            this.Name = name;
//        }

//        public object Convert(object input)
//        {
//            return this.converter(input);
//        }

//        public static ComplexTypeConverter Create<TInput, TOutput>(Func<TInput, TOutput> converter)
//            where TInput : class
//            where TOutput : class
//        {
//            var type = typeof(TOutput);
//            var name = HostInfoHelper.MakeClassName(type);

//            return new ComplexTypeConverter(converter as Func<object, object>, name);
//        }

//        public static ComplexTypeConverter Create<TInput, TOutput>(Func<TInput, TOutput> converter, string name)
//            where TInput : class
//            where TOutput : class
//        {
//            return new ComplexTypeConverter(converter as Func<object, object>, name);
//        }

//        public static TOutput Convert<TOutput, TInput>(ComplexTypeConverter ctc, TInput input) 
//            where TInput : class
//            where TOutput : class
//        {
//            if (ctc == null)
//            {
//                throw new ArgumentNullException("ctc");
//            }

//            if (!ctc.Input.IsEquivalentTo(typeof(TInput)))
//            {
//                throw new ComplexTypeConverterException(string.Format("TInput ({0}) is not equivalent to converter input {0}", typeof(TInput), ctc.Input));
//            }

//            if (!ctc.Output.IsEquivalentTo(typeof(TOutput)))
//            {
//                throw new ComplexTypeConverterException(string.Format("TOutput ({0}) is not equivalent to converter output {0}", typeof(TOutput), ctc.Output));
//            }

//            return ctc.Convert(input as TInput) as TOutput;
//        }
//    }
//}
