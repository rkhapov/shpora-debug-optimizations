using System;
using System.Numerics;

namespace DotTraceExamples.Programs
{
	public class ComplexOperationTestProgram : IProgram
	{
		private const int elementsNumber = 13000000;

		public void Run()
		{
			var data = new Complex[elementsNumber];
			data.DivideByNumber(Math.PI).SumModules();
		}
	}
}