using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RegionOneRecruitment.DBContext;
using RegionOneRecruitment.Models;
using System.Diagnostics;
namespace RegionOneRecruitment.Components.Pages
{
    public partial class Openings
    {
        public DataContext? Context { get; set; } = default;
        public List<Opening>? JobListings { get; set; }
        public bool EditRecord { get; set; }
        public Opening? OpeningToUpdate { get; set; }
        public string searchTerm = string.Empty;

        // Creating Job Post
        public bool showSuccessMessage = false;
        public string? errorMessage;
        public bool showSuccessMessage2 = false;
        public string? errorMessage2 = null;
        public Opening JobOpening { get; set; } = new Opening();
        public List<string> JobSkills { get; set; } = [];
        public string? NewJobSkill { get; set; } = string.Empty;
        public List<string> CertificationsRequired { get; set; } = [];
        public string? NewCertification { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await ShowJobListings();
        }

        private async Task FilterJobListings()
        {
            var context = await OpeningsDataContextFactory.CreateDbContextAsync();

            if (context != null)
            {
                JobListings = await context.Openings
                    .Where(o => (o.JobTitle != null && o.JobTitle.ToLower().Contains(searchTerm.ToLower())) ||
                                (o.JobDescription != null && o.JobDescription.ToLower().Contains(searchTerm.ToLower())))
                    .ToListAsync();
            }
            else
            {
                JobListings = [];
            }

            StateHasChanged();
        }

        public void NavigateToJob(Opening opening)
        {
            NavigationManager.NavigateTo($"/job-details/{opening.OpeningId}");
        }

        public async Task ShowJobListings()
        {
            Context ??= await OpeningsDataContextFactory.CreateDbContextAsync();

            if (Context is not null)
            {
                JobListings = await Context.Openings.ToListAsync();
            }
        }

        public async Task ShowEditForm(Opening jobListing)
        {
            Context ??= await OpeningsDataContextFactory.CreateDbContextAsync();
            OpeningToUpdate = Context.Openings.FirstOrDefault(x => x.OpeningId == jobListing.OpeningId);
            EditRecord = true;
        }

        public async Task CancelEdit()
        {
            OpeningToUpdate = null;  // Clear the edit user
            EditRecord = false;    // Set edit mode to false
            await ShowJobListings();
        }

        public async Task DeleteOpening(int openingId)
        {
            Context ??= await OpeningsDataContextFactory.CreateDbContextAsync();

            if (Context is not null)
            {
                var openingToDelete = await Context.Openings.FindAsync(openingId);
                if (openingToDelete != null)
                {
                    Context.Openings.Remove(openingToDelete);
                    await Context.SaveChangesAsync();
                    await ShowJobListings(); // Refresh the list after deletion
                }
            }
        }

        public async Task UpdateOpening()
        {
            EditRecord = false;
            Context ??= await OpeningsDataContextFactory.CreateDbContextAsync();

            if (Context is not null)
            {
                if (OpeningToUpdate is not null)
                {
                    Context.Openings.Update(OpeningToUpdate);
                }
                await Context.SaveChangesAsync();
            }
        }

        public async Task JobApply(Opening opening)
        {
            showSuccessMessage2 = false;
            errorMessage2 = null;

            Context ??= await OpeningsDataContextFactory.CreateDbContextAsync();

            if (opening is not null)
            {
                var userId = SessionService.GetUserId();

                if (userId != -1)
                {
                    Application JobApplication = new()
                    {
                        OpeningId = opening.OpeningId,
                        UserId = userId
                    };

                    Context.Applications.Add(JobApplication);
                    await Context.SaveChangesAsync();
                    showSuccessMessage2 = true;
                }
                else
                {
                    errorMessage2 = "Error: Unable to retrieve your information.";
                }
            }
            else
            {
                errorMessage2 = "Error: Please try again later.";
            }
        }

        public async Task CreateNewJobOpeningAsync()
        {
            showSuccessMessage = false;
            errorMessage = null;

            if (JobOpening is not null)
            {
                try
                {
                    Context ??= await OpeningsDataContextFactory.CreateDbContextAsync();
                    JobOpening.SkillsRequired = string.Join(",", JobSkills);
                    JobOpening.CertificationsRequired = string.Join(",", CertificationsRequired);
                    JobOpening.PositionFilled = true;
                    JobOpening.PostedDate = DateOnly.FromDateTime(DateTime.Now).ToString();
                    Context.Openings.Add(JobOpening);
                    await Context.SaveChangesAsync();
                    showSuccessMessage = true;

                    // Reset the form after successful submission
                    JobOpening = new Opening();
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"error: {ex.Message}");
                }
            }
        }

        public void AddJobSkill()
        {
            if (!string.IsNullOrWhiteSpace(NewJobSkill) && !JobSkills.Contains(NewJobSkill))
            {
                JobSkills.Add(NewJobSkill);
                Debug.WriteLine($"Job skill added: {NewJobSkill}");
                NewJobSkill = string.Empty;
                StateHasChanged();
            }
            else
            {
                Debug.WriteLine("Job skill not added.");
            }
        }

        public void RemoveJobSkill(string skill)
        {
            JobSkills.Remove(skill);
        }

        public void AddJobCertification()
        {
            if (!string.IsNullOrWhiteSpace(NewCertification) && !CertificationsRequired.Contains(NewCertification))
            {
                CertificationsRequired.Add(NewCertification);
                Debug.WriteLine($"Job certification added: {NewCertification}");
                NewCertification = string.Empty;
                StateHasChanged();
            }
            else
            {
                Debug.WriteLine("Job certification not added.");
            }
        }

        public void RemoveJobCertification(string certification)
        {
            JobSkills.Remove(certification);
        }
    }
}
