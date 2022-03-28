﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
	public class Speler
	{
		public Guid Id { get; set; }
		public string Naam { get; set; }
		public string Email { get; set; }
		public int AantalGewonnen { get; set; }
		public int AantalVerloren { get; set; }
		public int AantalGelijk { get; set; }
	}
}
