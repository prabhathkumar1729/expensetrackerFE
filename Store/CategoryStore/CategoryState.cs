using Fluxor;
using BlazorWalletTrackerFE.Shared.Services;
using BlazorWalletTrackerFE.Store.TransactionStore;
using MudBlazor.Interfaces;

namespace BlazorWalletTrackerFE.Store.CategoryStore
{
    public class CategoryState
    {
        public List<string> CategoryList { get; set; } = new List<string> { "Food", "Shopping", "Loans", "Entertainment", "Bills", "Others" };
        public CategoryState() { }

    }

    public class CategoryFeature : Feature<CategoryState>
    {
        public override string GetName() => "Category";
        protected override CategoryState GetInitialState()
        {
            return new CategoryState();
        }
    }
    public class FetchCategoriesAction
    {
        public int UserId { get; set; }
    }
    public class FetchCategoriesSuccessAction
    {
        public List<string> CategoryList { get; set; }
    }
    public class AddCategoryAction
    {
        public string Category { get; set; }
    }
    //the categories can be created, updated, and deleted by the user. So if the user deletes a category, a pop is shown to the user whether the delete category should delete all the transactions with that category or just delete the category and keep the transactions with "Other" category
    public class UpdateCategoryAction
    {
        public int UserId { get; set; }
        public string newCategory { get; set; }
        public string oldCategory { get; set; }
    }
    public class UpdateCategoryInStateAction
    { 
        public string newCategory { get; set; }
        public string oldCategory { get; set; }
    }

    public class DeleteCategoryAction
    {
        public int UserId { get; set; }
        public string Category { get; set; }
    }

    public class DeleteAllCategoryTransactionsAction
    {
        public int UserId { get; set; }
        public string Category { get; set; }
    }

    public class DeletCategoryFromStateAction
    {
        public string Category { get; set; }
    }

    //write the actions, reducers to clear the category state storage
    public class ClearCategoryStateAction
    {
    }


    public class CategoryReducers
    {
        [ReducerMethod]
        public static CategoryState ReduceFetchCategories(CategoryState state, FetchCategoriesAction action)
        {
            return new CategoryState { };
        }
        [ReducerMethod]
        public static CategoryState ReduceFetchCategoriesSuccess(CategoryState state, FetchCategoriesSuccessAction action)
        {
            //var CategoryList = action.CategoryList;
            //CategoryList = CategoryList.Concat(new List<string> { "Food", "Shopping", "Loans", "Entertainment", "Bills", "Others" }).Distinct().OrderDescending().ToList();
            //foreach(string category in CategoryList)
            //{
            //   Console.WriteLine(category);
            //}
            var CategoryList = action.CategoryList;
            //also orderby in ascending
            CategoryList = CategoryList.Concat(new List<string> { "Food", "Shopping", "Loans", "Entertainment", "Bills", "Others" }).Distinct().OrderBy(category => category).ToList();
            return new CategoryState { CategoryList = CategoryList };
        }
        [ReducerMethod]
        public static CategoryState ReduceAddCategory(CategoryState state, AddCategoryAction action)
        {
            var CategoryList = state.CategoryList;
            CategoryList = CategoryList.Concat(new List<string> { action.Category }).Distinct().OrderBy(category => category).ToList();
            return new CategoryState { CategoryList = CategoryList };
        }
        [ReducerMethod]
        public static CategoryState ReduceUpdateCategoryInState(CategoryState state, UpdateCategoryInStateAction action)
        {
            //return new CategoryState
            //{
            //CategoryList = state.CategoryList
            //    .Select(category => category == action.oldCategory ? action.newCategory : category)
            //    .ToList().Distinct()

            //make sure the category is distinct and always have "Food", "Shopping", "Loans","Entertainment", "Bills" and "others" category
            //CategoryList = state.CategoryList
            //    .Select(category => category == action.oldCategory ? action.newCategory : category)
            //    .ToList().Concat(new List<string> { "Food", "Shopping", "Loans", "Entertainment", "Bills", "Others" }).Distinct().OrderBy(category => category).ToList()

            //return new category state by updating the old category with new category from the categorylist
            var CategoryList = state.CategoryList;
            CategoryList = CategoryList.Select(category => category == action.oldCategory ? action.newCategory : category).ToList();
            CategoryList = CategoryList.Concat(new List<string> { "Food", "Shopping", "Loans", "Entertainment", "Bills", "Others" }).Distinct().OrderBy(category => category).ToList();
            return new CategoryState { CategoryList = CategoryList };



        }
        [ReducerMethod]
        public static CategoryState ReduceDeletCategoryFromState(CategoryState state, DeletCategoryFromStateAction action)
        {
            return new CategoryState
            {
                CategoryList = state.CategoryList.Where(c => c != action.Category)
                .ToList().Concat(new List<string> { "Food", "Shopping", "Loans", "Entertainment", "Bills", "Others" }).Distinct().ToList()
            };
        }

        [ReducerMethod]
        public static CategoryState ReduceClearCategoryState(CategoryState state, ClearCategoryStateAction action)
        {
            return new CategoryState { };
        }
        
    }

    public class CategoryEffects
    {
        private readonly ICategoryService _categoryService;
        public CategoryEffects(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [EffectMethod]
        public async Task HandleFetchCategoriesAction(FetchCategoriesAction action, IDispatcher dispatcher)
        {
            var categoryList = await _categoryService.GetCategories(action.UserId);
            dispatcher.Dispatch(new FetchCategoriesSuccessAction { CategoryList = categoryList });
        }

        [EffectMethod]
        public async Task HandleUpdateCategoryAction(UpdateCategoryAction action, IDispatcher dispatcher)
        {
            bool success = await _categoryService.UpdateCategory(action.UserId, action.oldCategory, action.newCategory);
            //on success, replace the old category with the new category
            if (success)
            {
                dispatcher.Dispatch(new UpdateCategoryInStateAction { oldCategory = action.oldCategory, newCategory = action.newCategory });
                dispatcher.Dispatch(new UpdateCategoryInTransactionsAction { NewCategory = action.newCategory, OldCategory = action.oldCategory });
            }
        }
        [EffectMethod]
        public async Task HandleDeleteCategoryAction(DeleteCategoryAction action, IDispatcher dispatcher)
        {
            bool success = await _categoryService.UpdateCategory(action.UserId, action.Category, "Others");
            if (success)
            {
                dispatcher.Dispatch(new DeletCategoryFromStateAction { Category = action.Category });
                dispatcher.Dispatch(new UpdateCategoryInTransactionsAction { NewCategory = "Others", OldCategory = action.Category });
            }
        }

        [EffectMethod]
        public async Task HandleDeleteAllCategoryTransactionsAction(DeleteAllCategoryTransactionsAction action, IDispatcher dispatcher)
        {
            bool success = await _categoryService.DeleteAllCategoryTransactions(action.UserId, action.Category);
            if (success)
            {
                dispatcher.Dispatch(new DeletCategoryFromStateAction { Category = action.Category });
                dispatcher.Dispatch(new DeleteAllTransactionsHavingCategoryAction { Category = action.Category });
            }
        }
    }
}
