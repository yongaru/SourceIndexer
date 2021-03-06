﻿namespace SourceIndexer
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	internal class Program
	{
		private static void Main(string[] args)
		{
			var sourceControlPath = string.Empty;
			var rootPath = string.Empty;
			var files = new List<string>();
			var options = new Mono.Options.OptionSet
			{
				{ "s|src:", "The path to the source control system.", x => sourceControlPath = x },
				{ "r|root:", "The root path under which all source code files belong.", x => rootPath = x },
				{ "p|pdb:", "The fully qualified path to the one or more debug (PDB) files.", files.Add }
			};
			options.Parse(args);

			var symbols = files.Select(x => x.Standardize())
				.Where(File.Exists)
				.Select(x => new DebugSymbol(x, rootPath))
				.ToArray();

			var indexer = new NetworkShareIndexer(sourceControlPath);
			foreach (var symbol in symbols)
				indexer.Index(symbol);
		}
	}
}