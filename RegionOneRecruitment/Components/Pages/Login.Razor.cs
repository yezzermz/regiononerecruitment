using RegionOneRecruitment.DBContext;
using RegionOneRecruitment.Models;

namespace RegionOneRecruitment.Components.Pages
{
    public partial class Login
    {
        public DataContext? Context { get; set; } = default;
        private readonly LoginModel LoginModel = new();

        private async Task HandleLogin()
        {
            Context ??= await UserDataContextFactory.CreateDbContextAsync();
            var user = Context.Users.FirstOrDefault(u => u.Email == LoginModel.Email && u.Password == LoginModel.Password);

            if (user != null)
            {
                SessionService.CreateSession(user.UserId, user.IsAdmin);
                StateHasChanged();
                Navigation.NavigateTo("/");
            }
            else
            {
                Navigation.NavigateTo("/register");
            }
        }
    }
}
