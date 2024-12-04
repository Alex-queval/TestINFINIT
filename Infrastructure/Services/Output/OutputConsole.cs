using Models.Data;

namespace Infrastructure.Services.Output
{
    /// <summary>
    /// Represents a service that output statistics of <see cref="RepositoryInfo"/> 
    /// into the <see cref="Console"/>.
    /// </summary>
    public sealed class OutputConsole : IOutputServices
    {
        /// <summary>
        /// Output statistics
        /// </summary>
        /// <param name="repositoryInfo">Repo's info.</param>
        public void Output(RepositoryInfo repositoryInfo)
        {
            Console.WriteLine("---------Output info---------");
            Console.WriteLine($"Repository {repositoryInfo.Owner}/{repositoryInfo.RepositoryName}");
            Console.WriteLine($"Number of Js/Ts files : {repositoryInfo.TotalJsFile}");
            Console.WriteLine($"Number of chars in total : {repositoryInfo.TotalChars}");
            Console.WriteLine($"Repartition : ");

            foreach (var item in repositoryInfo.TotalCharsCount)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }

            foreach (var file in repositoryInfo.JsFiles) 
            {
                Console.WriteLine($"---------File : {file.Name}---------");
                Console.WriteLine($"Number of chars in total : {file.TotalChars}");
                Console.WriteLine($"Repartition : ");

                foreach (var item in file.CharsCount)
                {
                    Console.WriteLine($"{item.Key} : {item.Value}");
                }
            }
        }
    }
}
