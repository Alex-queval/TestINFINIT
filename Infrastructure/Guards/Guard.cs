namespace Infrastructure.Guards
{
    /// <summary>
    /// Represents a static class for testsing parameters values.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Checks if the given string <paramref name="param"/> is null.
        /// Throws <see cref="ArgumentNullException"/> if param is null.
        /// </summary>
        /// <param name="param">Value to check.</param>
        /// <param name="paramName">Name of the value.</param>
        public static void ArgumentNotNull(object param, string paramName)
        {
            if(param is null) 
                throw new ArgumentNullException($"parameter {paramName} cannot be null.");
        }

        /// <summary>
        /// Checks if the given string <paramref name="param"/> is null or empty.
        /// Throws <see cref="ArgumentNullException"/> if param is null.
        /// Throws <see cref="ArgumentException"/> if param is empty.
        /// </summary>
        /// <param name="param">Value to check.</param>
        /// <param name="paramName">Name of the value.</param>
        public static void ArgumentNotNullOrEmpty(string param, string paramName)
        {
            if (param is null)
                throw new ArgumentNullException($"parameter {paramName} cannot be null.");
            
            if (string.IsNullOrEmpty(param))
                throw new ArgumentException($"parameter {paramName} cannot be empty");
            
        }


        /// <summary>
        /// Checks if the given <paramref name="collection"/> is null, 
        /// and checks if item are null.
        /// Throws <see cref="ArgumentNullException"/> if param is null.
        /// Throws <see cref="ArgumentNullException"/> if item is null.
        /// </summary>
        /// <param name="collection">Value to check.</param>
        /// <param name="paramName">Name of the value.</param>
        public static void ArgumentItemsNotNull(IEnumerable<object> collection, string paramName)
        {
            ArgumentNotNull(collection, paramName);

            if(!collection.Any())
                throw new ArgumentException($"collection {paramName} cannot be empty");

            foreach (var item in collection)
            {
                ArgumentNotNull(item, paramName);
            }
        }
    }
}
