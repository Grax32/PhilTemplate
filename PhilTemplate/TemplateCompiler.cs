using System.Linq.Expressions;
using System.Reflection;

namespace VorteonTemplateEngine
{
    public class TemplateCompiler
    {
        private static readonly MethodInfo _stringConcatMethod = ((Func<string[], string>)string.Concat).Method;
        public static Func<Func<string, string>, string> CompileFuncStringTemplate(string template, string keyStartMarker = "{{", string keyEndMarker = "}}")
        {
            var functionParameter = Expression.Parameter(typeof(Func<string, string>));
            var stringConcatMethod = _stringConcatMethod;

            var stringExpressions = new List<Expression>();

            foreach (var value in template.Split(keyStartMarker))
            {
                var parts = value.Split(keyEndMarker, 2);

                if (parts.Length == 1)
                {
                    stringExpressions.Add(Expression.Constant(value));
                }
                else
                {
                    var key = parts[0];

                    // if key is blank or equals key start marker, insert the keyStartMarker instead
                    if (key == keyStartMarker || key == "")
                    {
                        stringExpressions.Add(Expression.Constant(keyStartMarker));
                    }
                    else
                    {
                        stringExpressions.Add(Expression.Invoke(functionParameter, Expression.Constant(key)));

                    }

                    stringExpressions.Add(Expression.Constant(parts[1]));
                }
            }

            var values = Expression.NewArrayInit(typeof(string), stringExpressions);

            var expr = Expression.Lambda<Func<Func<string, string>, string>>(Expression.Call(stringConcatMethod, values), functionParameter);

            return expr.Compile();
        }
    }
}
