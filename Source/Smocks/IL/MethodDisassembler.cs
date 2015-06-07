﻿#region License
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
using System.Reflection;
using Mono.Cecil;
using Smocks.Utility;

namespace Smocks.IL
{
    internal class MethodDisassembler : IMethodDisassembler
    {
        public DisassembleResult Disassemble(MethodBase method)
        {
            ArgumentChecker.NotNull(method, () => method);
            ArgumentChecker.Assert<ArgumentException>(method.DeclaringType != null,
                "Method must be in a type", "method");

            string location = method.DeclaringType.Assembly.Location;
            ModuleDefinition module = ModuleDefinition.ReadModule(location);

            var foundMethod = module.Import(method).Resolve();

            if (foundMethod == null)
            {
                throw new ArgumentException("Could not find method in assembly", "method");
            }

            if (foundMethod.Body == null)
            {
                throw new ArgumentException("Method has no body", "method");
            }

            return new DisassembleResult(module, foundMethod.Body);
        }
    }
}