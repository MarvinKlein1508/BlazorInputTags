namespace BlazorInputTags
{
    public class InputTagOptions
    {
        /// <summary>
        /// Gets or sets the name of the class for the tag wrapper
        /// </summary>
        public string WrapperClass { get; set; } = "blazor-tag-wrapper";
        /// <summary>
        /// Gets or sets the name of the class for the tag list
        /// </summary>
        public string TagListClass { get; set; } = "blazor-tag-list";
        /// <summary>
        /// Gets or sets the name of the class for a tag
        /// </summary>
        public string TagClass { get; set; } = "blazor-tag";
        /// <summary>
        /// Gets or sets the name of the class for the input field.
        /// </summary>
        public string InputClass { get; set; } = "blazor-tag-input";
        /// <summary>
        /// Gets or sets the name of the class for the label
        /// </summary>
        public string LabelClass { get; set; } = "blazor-tag-label";
        /// <summary>
        /// Gets or sets the text for the tooltip from the remove button
        /// </summary>
        public string RemoveButtonTooltip { get; set; } = "Remove";
        /// <summary>
        /// Gets or sets the placeholder text for the input field.
        /// </summary>
        public string InputPlaceholder { get; set; } = "Enter tag, add with Enter";
        /// <summary>
        /// Defines whether the component should show a label in above the input field.
        /// </summary>
        public bool DisplayLabel { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum length of a tag in order to be able to be added to the list.
        /// <para>0 = No minimum length</para>
        /// </summary>
        public int MinLength { get; set; }
        /// <summary>
        /// Gets or sets the maximum length of a tag in order to be able to be added to the list.
        /// <para>0 = No maximum length</para>
        /// </summary>
        public int MaxLength { get; set; }
    }
}
