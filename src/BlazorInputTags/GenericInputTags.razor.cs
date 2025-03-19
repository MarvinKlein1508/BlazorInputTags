using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorInputTags
{
    public partial class GenericInputTags<TValue> : IAsyncDisposable
    {
        private readonly Guid _id = Guid.NewGuid();
        private bool _showSearchResults;
        private ElementReference? _reference;
        private DotNetObjectReference<GenericInputTags<TValue>>? _dotNetHelper = null;
        private IJSObjectReference Module { get; set; } = default!;
        private TValue? SelectedItem { get; set; }
        public string Input { get; set; } = string.Empty;
        private async Task OnInputClick()
        {
            await SearchAsync();
        }

        private async Task OnInputFocusOutAsync()
        {
            // Delay to let the UI refresh in case the user wants to select an item
            await Task.Delay(150);
            _showSearchResults = false;
        }


        public async Task OnItemSelectedAsync(TValue item)
        {
            if (!Value.Remove(item))
            {
                Value.Add(item);
            }

            _showSearchResults = false;
            Input = string.Empty;

            await _reference!.Value.FocusAsync();
        }

        [JSInvokable]
        public async Task OnItemSelectedAsync()
        {
            if (SelectedItem is null || !_showSearchResults)
            {
                return;
            }

            await OnItemSelectedAsync(SelectedItem);
            await InvokeAsync(StateHasChanged);
        }
        [JSInvokable]
        public async Task HideSearchResultsAsync()
        {
            _showSearchResults = false;
            await InvokeAsync(StateHasChanged);
        }
        [JSInvokable]
        public async Task SelectNextItemAsync()
        {
            _showSearchResults = true;
            await InvokeAsync(StateHasChanged);
            if (SelectedItem is null)
            {
                SelectedItem = _searchResults.FirstOrDefault();
            }
            else
            {
                int currentIndex = _searchResults.IndexOf(SelectedItem);

                if (currentIndex is -1)
                {
                    SelectedItem = _searchResults.FirstOrDefault();
                }
                else if (currentIndex + 1 < _searchResults.Count)
                {
                    SelectedItem = _searchResults[currentIndex + 1];
                }
            }


            await InvokeAsync(StateHasChanged);
        }
        [JSInvokable]
        public async Task SelectPreviousItemAsync()
        {
            _showSearchResults = true;
            await InvokeAsync(StateHasChanged);

            if (SelectedItem is null)
            {
                SelectedItem = _searchResults.FirstOrDefault();
            }
            else
            {
                int currentIndex = _searchResults.IndexOf(SelectedItem);

                if (currentIndex is -1)
                {
                    SelectedItem = _searchResults.FirstOrDefault();
                }
                else if (currentIndex - 1 >= 0)
                {
                    SelectedItem = _searchResults[currentIndex - 1];
                }
            }

            await InvokeAsync(StateHasChanged);
        }

        [JSInvokable]
        public async Task OnBackspaceAsync()
        {
            if (Input == string.Empty)
            {
                Value.RemoveAt(Value.Count - 1);
            }
            else
            {
                Input = Input[..^1];
                await SearchAsync();
            }

            await InvokeAsync(StateHasChanged);
        }
        [Parameter] public string Placeholder { get; set; } = string.Empty;
        [Parameter] public string? Label { get; set; }
        [Parameter] public List<TValue> Value { get; set; } = new List<TValue>();
        [Parameter] public EventCallback<OptionsSearchEventArgs<TValue>> OnOptionsSearch { get; set; }
        [Parameter] public RenderFragment<TValue>? ItemTemplate { get; set; }

        private List<TValue> _searchResults = [];

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorInputTags/GenericInputTags.razor.js");
                _dotNetHelper = DotNetObjectReference.Create(this);
                await Module.InvokeVoidAsync("initialize", _id, _dotNetHelper);
            }
        }

        private async Task InputHandlerAsync(ChangeEventArgs e)
        {
            Input = e.Value?.ToString() ?? string.Empty;
            await SearchAsync();
        }

        private async Task SearchAsync()
        {
            var args = new OptionsSearchEventArgs<TValue>()
            {
                Items = Array.Empty<TValue>(),
                Text = Input,
            };

            await OnOptionsSearch.InvokeAsync(args);
            _searchResults = [.. args.Items];

            SelectedItem = _searchResults.FirstOrDefault();
            _showSearchResults = true;
        }

        private string GetSearchResultClass(TValue item)
        {
            bool valueContainsItem = Value.Contains(item);
            if (valueContainsItem && item!.Equals(SelectedItem))
            {
                return "blazor-tag-active blazor-tag-selected";
            }
            else if (valueContainsItem)
            {
                return "blazor-tag-active";
            }
            else if (item!.Equals(SelectedItem))
            {
                return "blazor-tag-selected";
            }

            return string.Empty;
        }

        private string GetResultListClass()
        {
            return _showSearchResults ? "blazor-tag-results" : "blazor-tag-results hidden";
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (Module != null)
                {
                    await Module.DisposeAsync();
                }
            }
            catch (Exception ex) when (ex is JSDisconnectedException ||
                                       ex is OperationCanceledException)
            {
                // The JSRuntime side may routinely be gone already if the reason we're disposing is that
                // the client disconnected. This is not an error.
            }
        }
    }
}