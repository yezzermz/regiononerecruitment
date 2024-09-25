using Microsoft.AspNetCore.Components.Forms;
using RegionOneRecruitment.DBContext;
using RegionOneRecruitment.Models;
using System.Diagnostics;

namespace RegionOneRecruitment.Components.Pages
{
    public partial class Register
    {
        public DataContext? Context { get; set; } = default;
        public User NewUser { get; set; } = new User();
        public List<string> JobInterests { get; set; } = [];
        public string? NewJobInterest { get; set; } = string.Empty;
        public bool showSuccessMessage = false;
        public string? errorMessage;
        public string? resumeFileName;

        public void AddJobInterest()
        {
            if (!string.IsNullOrWhiteSpace(NewJobInterest) && !JobInterests.Contains(NewJobInterest))
            {
                JobInterests.Add(NewJobInterest);
                Debug.WriteLine($"Job interest added: {NewJobInterest}");
                NewJobInterest = string.Empty;
                StateHasChanged();
            }
            else
            {
                Debug.WriteLine("Job interest not added.");
            }
        }

        public void RemoveJobInterest(string interest)
        {
            JobInterests.Remove(interest);
        }

        public async Task HandleResumeUpload(InputFileChangeEventArgs e)
        {
            try
            {
                var file = e.File;
                if (file != null)
                {
                    // max file size of 5mb
                    if (file.Size > 5 * 1024 * 1024)
                    {
                        errorMessage = "File size exceeds 5MB limit.";
                        return;
                    }

                    // make sure only pdf uploaded
                    if (file.ContentType != "application/pdf")
                    {
                        errorMessage = "Only PDF files are allowed.";
                        return;
                    }

                    using var ms = new MemoryStream();
                    await file.OpenReadStream().CopyToAsync(ms);
                    NewUser.ResumeBytes = ms.ToArray();
                    resumeFileName = file.Name;
                    errorMessage = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error: {ex.Message}");
            }
        }

        public async Task CreateNewUserAsync()
        {
            showSuccessMessage = false;
            errorMessage = null;

            if (NewUser is not null)
            {
                try
                {
                    Context ??= await UserDataContextFactory.CreateDbContextAsync();
                    NewUser.JobInterests = string.Join(",", JobInterests);

                    Context.Users.Add(NewUser);
                    await Context.SaveChangesAsync();
                    showSuccessMessage = true;

                    // Reset the form after successful submission
                    NewUser = new User();
                    JobInterests.Clear();
                    resumeFileName = null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"error: {ex.Message}");
                }
            }
        }
    }
}
