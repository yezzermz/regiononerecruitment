using Microsoft.EntityFrameworkCore;
using RegionOneRecruitment.DBContext;
using RegionOneRecruitment.Models;

namespace RegionOneRecruitment.Components.Pages
{
    public partial class Home
    {
        public DataContext? Context { get; set; } = default;
        public List<Opening>? JobListings { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ShowJobListings();
        }

        public async Task ShowJobListings()
        {
            Context ??= await OpeningsDataContextFactory.CreateDbContextAsync();

            if (Context is not null)
            {
                JobListings = await Context.Openings.ToListAsync();
            }
        }
    }
}
