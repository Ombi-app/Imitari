using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Imitari.Apis;
using System.Linq;

namespace Imitari.Func
{
    public class ImagesFunction
    {
        private readonly IFanartTvApi _api;
        private readonly ITheMovieDbApi _movieApi;

        public ImagesFunction(IFanartTvApi api, ITheMovieDbApi movieApi)
        {
            _api = api;
            _movieApi = movieApi;
        }

        [FunctionName("GetPoster")]
        public async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            return await GetRandomPoster();
        }

        private async Task<string> GetRandomPoster()
        {
            var rand = new Random();
            var val = rand.Next(1, 3);
            if (val == 1)
            {
                var movies = (await _movieApi.PopularMovies()).results.ToArray();
                var selectedMovieIndex = rand.Next(movies.Count());
                var movie = movies[selectedMovieIndex];

                var images = await _api.GetMovieImages(movie.Id.ToString());
                if (images == null)
                {
                    return string.Empty;
                }

                if (images.movieposter?.Any() ?? false)
                {
                    var enImage = images.movieposter.Where(x => x.lang == "en").OrderByDescending(x => x.likes).Select(x => x.url).FirstOrDefault();
                    if (enImage == null)
                    {
                        return images.movieposter.OrderByDescending(x => x.likes).Select(x => x.url).FirstOrDefault();
                    }
                    return enImage;
                }

                if (images.moviethumb?.Any() ?? false)
                {
                    return images.moviethumb.OrderBy(x => x.likes).Select(x => x.url).FirstOrDefault();
                }

                return await GetRandomPoster();
            }
            else
            {
                var tv = (await _movieApi.PopularTv()).results.ToArray();
                var selectedMovieIndex = rand.Next(tv.Count());
                var selected = tv[selectedMovieIndex];

                return $"https://image.tmdb.org/t/p/original{selected.backdrop_path}";
            }
        }
    }
}
