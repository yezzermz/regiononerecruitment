using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using RegionOneRecruitment.DBContext;
using RegionOneRecruitment.Models;
using System.Diagnostics;

namespace RegionOneRecruitment.Components.Pages
{
    public partial class Users
    {
        public DataContext? Context { get; set; } = default;
        public List<User>? AllUsers { get; set; }
        public bool EditRecord { get; set; }
        public User? UserToUpdate { get; set; }
        public User? OriginalUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ShowAllUsers();
        }

        public async Task ShowAllUsers()
        {
            Context ??= await UserDataContextFactory.CreateDbContextAsync();

            if (Context is not null)
            {
                AllUsers = await Context.Users.ToListAsync();
            }
        }

        public async Task ShowEditForm(User user)
        {
            Context ??= await UserDataContextFactory.CreateDbContextAsync();
            UserToUpdate = Context.Users.FirstOrDefault(x => x.UserId == user.UserId);
            OriginalUser = UserToUpdate;
            EditRecord = true;
        }

        public async Task UpdateUser()
        {
            EditRecord = false;
            Context ??= await UserDataContextFactory.CreateDbContextAsync();

            if (Context is not null)
            {
                if (UserToUpdate is not null)
                {
                    Context.Users.Update(UserToUpdate);
                }
                await Context.SaveChangesAsync();
            }
        }

        public async Task CancelEdit()
        {
            UserToUpdate = null;
            EditRecord = false;
            await ShowAllUsers();
        }

        public async Task DeleteUser(int userId)
        {
            Context ??= await UserDataContextFactory.CreateDbContextAsync();

            if (Context is not null)
            {
                var userToDelete = await Context.Users.FindAsync(userId);
                if (userToDelete != null)
                {
                    Context.Users.Remove(userToDelete);
                    await Context.SaveChangesAsync();
                    await ShowAllUsers();
                }
            }
        }

        public async Task HandleResumeUpload(InputFileChangeEventArgs e)
        {
            try
            {
                if (UserToUpdate is not null)
                {
                    var file = e.File;
                    if (file != null)
                    {
                        if (file.Size > 5 * 1024 * 1024)
                        {
                            return;
                        }

                        if (file.ContentType != "application/pdf")
                        {
                            return;
                        }

                        using var ms = new MemoryStream();
                        await file.OpenReadStream().CopyToAsync(ms);
                        UserToUpdate.ResumeBytes = ms.ToArray();
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error: {ex.Message}");
            }
        }
    }
}
