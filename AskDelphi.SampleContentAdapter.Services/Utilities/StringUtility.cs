using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.SampleContentAdapter.Services.Utilities
{
	public static class StringUtility
    {
		// <summary>
		/// Formats the string using the invariant culture.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The format arguments.</param>
		/// <returns>The formatted string.</returns>
		[StringFormatMethod("format")]
		public static string FormatInvariant(this string format, params object[] args) => string.Format(CultureInfo.InvariantCulture, format, args ?? null);
	}
}
