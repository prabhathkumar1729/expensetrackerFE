using BlazorWalletTrackerFE.Shared.Models;
using BlazorWalletTrackerFE.Shared.Services;
using BlazorWalletTrackerFE.Store.TransactionStore;
using Fluxor;
using Fluxor.DependencyInjection;

namespace BlazorWalletTrackerFE.Store.UserStore
{
    public class UserState
    {
        public User User { get; set; } = new User();
        public bool IsLoading { get; set; } = false;
        public bool IsLoggedIn { get; set; } = false;
        public string Error { get; set; } = null;
    }

    public class UserFeature : Feature<UserState>
    {
        public override string GetName() => "User";
        protected override UserState GetInitialState()
        {
            return new UserState();
        }
    }

    public class FetchUserAction { }

    public class FetchUserSuccessAction
    {
        public User User { get; set; }
    }

    public class SaveToUserStateAction
    {
        public User User { get; set; }
    }

    //write action and reducer to change the user state when the user logs out
    public class LogOutAction { }

    public class UserReducers
    {
        [ReducerMethod]
        public static UserState ReduceFetchUser(UserState state, FetchUserAction action)
        {
            return new UserState { IsLoading = true };
        }
        [ReducerMethod]
        public static UserState ReduceFetchUserSuccess(UserState state, FetchUserSuccessAction action)
        {
            return new UserState { User = action.User, IsLoading = false, IsLoggedIn=true };
        }

        [ReducerMethod]
        public static UserState ReduceSaveToUserState(UserState state, SaveToUserStateAction action)
        {
            return new UserState { User = action.User, IsLoading = false, IsLoggedIn = true };
        }

        [ReducerMethod]
        public static UserState ReduceLogOut(UserState state, LogOutAction action)
        {
            return new UserState { User = new User(), IsLoading = false, IsLoggedIn = false };
        }
    }

    public class UserEffects
    {
        private readonly IUserService _userService;
        public UserEffects(IUserService userService)
        {
            _userService = userService;
        }
        [EffectMethod]
        public async Task HandleFetchUser(FetchUserAction action, IDispatcher dispatcher)
        {
            var user = await _userService.GetUser();
            dispatcher.Dispatch(new FetchUserSuccessAction { User = user });
        }
    }
}
