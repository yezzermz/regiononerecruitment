using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RegionOneRecruitment.DBContext;
using RegionOneRecruitment.Models;

namespace RegionOneRecruitment.Components.Pages
{
    public partial class Job_Details
    {
        [Parameter]
        public int Id { get; set; }
        public Opening? JobOpening { get; set; }
        public DataContext? Context { get; set; } = default;
        public bool showSuccessMessage = false;
        public string? errorMessage;

        protected override void OnParametersSet()
        {
            if (Id <= 0)
            {
                NavigationManager.NavigateTo("/");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await ShowJobListing();
        }

        public async Task ShowJobListing()
        {
            Context ??= await OpeningsDataContextFactory.CreateDbContextAsync();

            if (Context is not null)
            {
                JobOpening = await Context.Openings.FirstOrDefaultAsync(x => x.OpeningId == Id);
            }
        }
        public void NavigateToLogin()
        {
            NavigationManager.NavigateTo("/login");
        }

        public async Task JobApply(Opening jobOpening)
        {
            showSuccessMessage = false;
            errorMessage = null;

            Context ??= await OpeningsDataContextFactory.CreateDbContextAsync();

            if (jobOpening is not null)
            {
                var userId = SessionService.GetUserId();

                if (userId != -1)
                {
                    Application JobApplication = new()
                    {
                        OpeningId = jobOpening.OpeningId,
                        UserId = userId
                    };

                    Context.Applications.Add(JobApplication);
                    await Context.SaveChangesAsync();
                    showSuccessMessage = true;
                }
                else
                {
                    errorMessage = "Error: Unable to retrieve your information.";
                }
            }
            else
            {
                errorMessage = "Error: Please try again later.";
            }
        }
    }
}
