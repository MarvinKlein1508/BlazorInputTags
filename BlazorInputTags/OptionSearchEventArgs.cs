using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputTags;

public class OptionsSearchEventArgs<T>
{
    /// <summary>
    /// Gets or sets the text to search.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of items to display.
    /// </summary>
    public IEnumerable<T>? Items { get; set; }
}
