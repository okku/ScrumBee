
using ScrumBee.Model;

namespace ScrumBee.DomainServices.Web
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Web.Ria;
	using System.Web.Ria.Data;
	using System.Web.DomainServices;


	// TODO: Create methods containing your application logic.
	[EnableClientAccess()]
	public class ScrumBeeService : DomainService
	{
		public IQueryable<Story> GetStories()
		{
			// Create some test data
			Story[] stories = {
								new Story {Id = 1, Name = "Story 1", Description = "This is story 1"},
								new Story {Id = 2, Name = "Story 2", Description = "This is story 2"},
								new Story {Id = 3, Name = "Story 3", Description = "This is story 3"},
								new Story {Id = 4, Name = "Story 4", Description = "This is story 4"}
							  };
			return stories.AsQueryable();
		}

		public void InsertStory(Story story)
		{
			// Not yet implemented...
		}

		public void UpdateStory(Story story)
		{
			// Not yet implemented...
		}

		public void DeleteStory(Story story)
		{
			// Not yet implemented...
		}
	}
}


