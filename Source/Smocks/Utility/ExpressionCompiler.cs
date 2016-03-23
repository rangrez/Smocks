#region License
//// The MIT License (MIT)
//// 
//// Copyright (c) 2015 Tom van der Kleij
//// 
//// Permission is hereby granted, free of charge, to any person obtaining a copy of
//// this software and associated documentation files (the "Software"), to deal in
//// the Software without restriction, including without limitation the rights to
//// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//// the Software, and to permit persons to whom the Software is furnished to do so,
//// subject to the following conditions:
//// 
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Linq.Expressions;

namespace Smocks.Utility
{
    /// <summary>
    /// A compiler for <see cref="Expression"/>s.
    /// </summary>
    public class ExpressionCompiler : IExpressionCompiler
    {
        /// <summary>
        /// Compiles an <see cref="LambdaExpression"/> to a <see cref="Delegate"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The compiled expression as a <see cref="Delegate"/>.</returns>
        public Delegate CompileExpression(LambdaExpression expression)
        {
            ArgumentChecker.NotNull(expression, () => expression);

            var type = expression.GetType();
            bool isCompilableExpression = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Expression<>);
            ArgumentChecker.Assert<ArgumentException>(isCompilableExpression, "Unexpected expression type, cannot compile", "expression");

            Delegate compiledDelegate = (Delegate)type.GetMethod("Compile", new Type[0]).Invoke(expression, new object[0]);
            return compiledDelegate;
        }
    }
}