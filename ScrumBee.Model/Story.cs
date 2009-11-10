using System.ComponentModel.DataAnnotations;

namespace ScrumBee.Model
{
	/// <summary>
	/// This is just a test class to verify that RIA is working.
	/// </summary>
	/// <remarks>
	/// 2009-11-10  POMU: Class created
	/// </remarks>
	public class Story
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}
}