using System;

namespace Markdown
{
	class Program
	{
		static void Main()
		{
			var md = new Md();
			Console.WriteLine(md.Render("__a _b_ c__"));
		}
	}
}