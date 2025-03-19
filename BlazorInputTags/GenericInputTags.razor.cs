using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorInputTags
{
    public partial class GenericInputTags<TValue>
    {
        private readonly Guid _id = Guid.NewGuid();

        private ElementReference? _reference;
        private DotNetObjectReference<GenericInputTags<TValue>>? _dotNetHelper = null;
        private IJSObjectReference Module { get; set; } = default!;
        private bool _wasSetToEmpty;
        private string _input = string.Empty;

        private bool _showSearchResults;

        private TValue? SelectedItem { get; set; }
        public string Input
        {
            get => _input;
            set
            {
                _wasSetToEmpty = value == string.Empty;
                _input = value;
            }
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
            if (SelectedItem is null)
            {
                SelectedItem = _items.FirstOrDefault();
            }
            else
            {
                int currentIndex = _items.IndexOf(SelectedItem);

                if (currentIndex is -1)
                {
                    SelectedItem = _items.FirstOrDefault();
                }
                else if (currentIndex + 1 < _items.Count)
                {
                    SelectedItem = _items[currentIndex + 1];
                }
            }


            await InvokeAsync(StateHasChanged);
        }
        [JSInvokable]
        public async Task SelectPreviousItemAsync()
        {
            if (SelectedItem is null)
            {
                SelectedItem = _items.FirstOrDefault();
            }
            else
            {
                int currentIndex = _items.IndexOf(SelectedItem);

                if (currentIndex is -1)
                {
                    SelectedItem = _items.FirstOrDefault();
                }
                else if (currentIndex - 1 >= 0)
                {
                    SelectedItem = _items[currentIndex - 1];
                }
            }

            await InvokeAsync(StateHasChanged);
        }
        [Parameter] public string Placeholder { get; set; } = string.Empty;
        [Parameter] public List<TValue> Value { get; set; } = new List<TValue>();
        [Parameter] public EventCallback<OptionsSearchEventArgs<TValue>> OnOptionsSearch { get; set; }
        [Parameter] public RenderFragment<TValue>? ItemTemplate { get; set; }

        private List<TValue> _items = [];

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

            if (_wasSetToEmpty)
            {
                _items = [];
                return;
            }

            var args = new OptionsSearchEventArgs<TValue>()
            {
                Items = Array.Empty<TValue>(),
                Text = Input,
            };

            await OnOptionsSearch.InvokeAsync(args);
            _items = [.. args.Items];

            SelectedItem = _items.FirstOrDefault();
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
    }
}