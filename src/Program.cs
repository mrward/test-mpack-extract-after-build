// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace MPackExtractor
{
	class Program
	{
		public static int Main (string[] args)
		{
			try {
				var program = new Program ();
				return program.Run (args);
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return -2;
			}
		}
		
		string mpackUrl;
		string mpackExtractionDirectory;
		string downloadFileName;
		
		int Run (string[] args)
		{
			if (!ParseArgs (args)) {
				ShowUsage ();
				return -1;
			}
			
			DownloadMPackFile ();
			ExtractMPackFile ();
			
			return 0;
		}
		
		bool ParseArgs (string[] args)
		{
			if (args.Length != 2)
				return false;
			
			mpackUrl = args [0];
			mpackExtractionDirectory = Path.GetFullPath (args [1]);
			
			return true;
		}
		
		void ShowUsage()
		{
			Console.WriteLine ("Usage: MPackExtractor mpack-file-url output-directory");
		}
		
		void ExtractMPackFile ()
		{
			ZipFile.ExtractToDirectory (downloadFileName, mpackExtractionDirectory);
		}
		
		void DownloadMPackFile()
		{
			downloadFileName = GetDownloadFileName ();
			RemoveExistingDownloadedFile ();
			DownloadFile (mpackUrl, downloadFileName);
		}
		
		string GetDownloadFileName()
		{
			string directory = Path.GetDirectoryName (typeof (Program).Assembly.Location);
			return Path.Combine (directory, "downloaded.mpack");
		}
		
		void RemoveExistingDownloadedFile()
		{
			if (File.Exists (downloadFileName)) {
				File.Delete (downloadFileName);
			}
		}
		
		static void DownloadFile (string url, string fileName)
		{
			using (var client = new WebClient ()) {
				client.DownloadFile (url, fileName);
			}
		}
	}
}