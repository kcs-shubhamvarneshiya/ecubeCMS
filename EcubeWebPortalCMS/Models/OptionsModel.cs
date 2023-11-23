
namespace EcubeWebPortalCMS.Models
{

    /// <summary>
    /// Class CLS Options.
    /// </summary>
    public class OptionsModel
    {
        /// <summary>
        /// Gets or sets the in position.
        /// </summary>
        /// <value>The in position.</value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the name of the string.
        /// </summary>
        /// <value>The name of the string.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL default value].
        /// </summary>
        /// <value><c>true</c> if [BL default value]; otherwise, <c>false</c>.</value>
        public bool DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [BL na value].
        /// </summary>
        /// <value><c>true</c> if [BL na value]; otherwise, <c>false</c>.</value>
        public bool NaValue { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>The weight.</value>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        /// <value>The point.</value>
        public decimal Point { get; set; }

        /// <summary>
        /// Gets or sets the LG identifier.
        /// </summary>
        /// <value>The LG identifier.</value>
        public long Id { get; set; }
    }
}