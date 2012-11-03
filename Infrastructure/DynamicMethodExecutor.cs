using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Diagnostics;

namespace sharp_net {

    //http://blog.zhaojie.me/2008/11/invoke-method-by-lambda-expression.html
    //方法的直接调用，反射调用与……Lambda表达式调用
    public class DynamicMethodExecutor {
        private Func<object, object[], object> m_execute;

        public DynamicMethodExecutor(MethodInfo methodInfo) {
            this.m_execute = this.GetExecuteDelegate(methodInfo);
        }

        public object Execute(object instance, object[] parameters) {
            return this.m_execute(instance, parameters);
        }

        private Func<object, object[], object> GetExecuteDelegate(MethodInfo methodInfo) {
            // parameters to execute
            ParameterExpression instanceParameter =
                Expression.Parameter(typeof(object), "instance");
            ParameterExpression parametersParameter =
                Expression.Parameter(typeof(object[]), "parameters");

            // build parameter list
            List<Expression> parameterExpressions = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++) {
                // (Ti)parameters[i]
                BinaryExpression valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(
                    valueObj, paramInfos[i].ParameterType);
                parameterExpressions.Add(valueCast);
            }

            // non-instance for static method, or ((TInstance)instance)
            Expression instanceCast = methodInfo.IsStatic ? null :
                Expression.Convert(instanceParameter, methodInfo.ReflectedType);

            // static invoke or ((TInstance)instance).Method
            MethodCallExpression methodCall = Expression.Call(
                instanceCast, methodInfo, parameterExpressions);

            // ((TInstance)instance).Method((T0)parameters[0], (T1)parameters[1], ...)
            if (methodCall.Type == typeof(void)) {
                Expression<Action<object, object[]>> lambda =
                    Expression.Lambda<Action<object, object[]>>(
                        methodCall, instanceParameter, parametersParameter);

                Action<object, object[]> execute = lambda.Compile();
                return (instance, parameters) => {
                    execute(instance, parameters);
                    return null;
                };
            } else {
                UnaryExpression castMethodCall = Expression.Convert(
                    methodCall, typeof(object));
                Expression<Func<object, object[], object>> lambda =
                    Expression.Lambda<Func<object, object[], object>>(
                        castMethodCall, instanceParameter, parametersParameter);

                return lambda.Compile();
            }
        }

    }

    //Simple
    class WatchDynamicMethod {
        private void Watch() {
            int times = 1000000;
            Program program = new Program();
            object[] parameters = new object[] { new object(), new object(), new object() };
            program.Call(null, null, null); // force JIT-compile
            MethodInfo methodInfo = typeof(Program).GetMethod("Call");

            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            for (int i = 0; i < times; i++) {
                program.Call(parameters[0], parameters[1], parameters[2]);
            }
            watch1.Stop();
            Console.WriteLine(watch1.Elapsed + " (Directly invoke)");

            Stopwatch watch2 = new Stopwatch();
            watch2.Start();
            for (int i = 0; i < times; i++) {
                methodInfo.Invoke(program, parameters);
            }
            watch2.Stop();
            Console.WriteLine(watch2.Elapsed + " (Reflection invoke)");

            DynamicMethodExecutor executor = new DynamicMethodExecutor(methodInfo);
            Stopwatch watch3 = new Stopwatch();
            watch3.Start();
            for (int i = 0; i < times; i++) {
                executor.Execute(program, parameters);
            }
            watch3.Stop();
            Console.WriteLine(watch3.Elapsed + " (Dynamic executor)");
        }
    }

    class Program {
        static void Main(string[] args) {

        }
        internal void Call(object one, object two, object three) { }
    }

}
