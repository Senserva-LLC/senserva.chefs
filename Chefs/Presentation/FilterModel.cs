namespace Chefs.Presentation;

public partial record FilterModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;

	public FilterModel(SearchFilter filters, INavigator navigator, IRecipeService recipeService)
	{
		_navigator = navigator;
		_recipeService = recipeService;
		Filter = State.Value(this, () => filters);
	}
	public IState<SearchFilter> Filter { get; }
	public IEnumerable<FilterGroup> FilterGroups => Enum.GetValues(typeof(FilterGroup)).Cast<FilterGroup>();
	public IEnumerable<Time> Times => Enum.GetValues(typeof(Time)).Cast<Time>();
	public IEnumerable<Difficulty> Difficulties => Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>();
	public IEnumerable<int> Serves => new int[] { 1, 2, 3, 4, 5 };
	public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);

	public async ValueTask ApplySearchFilter(SearchFilter filter) =>
		await _navigator.NavigateBackWithResultAsync(this, data: filter);

	public async ValueTask Reset(CancellationToken ct) =>
		await Filter.UpdateAsync(current => new SearchFilter(), ct);
}
