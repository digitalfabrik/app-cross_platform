using System;

namespace Integreat.Shared.Models
{
	public class UpdateTime
	{
		public string Date { get; set; }

		private const int Offset = 1000;
		// we need this offset to exclude Pages we already have

		public UpdateTime (long time)
		{
			Date = new DateTime (time + Offset).ToRestAcceptableString ();
		}

		public override string ToString ()
		{
			return Date;
		}
	}
}

