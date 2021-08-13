using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imitari.Apis;
using Imitari.Apis.Models;
using Imitari.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Imitari.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IFanartTvApi _api;
        private readonly ICacheService _cache;
        private readonly ITheMovieDbApi _movieApi;

        public ImagesController(IFanartTvApi api, ICacheService cache, ITheMovieDbApi movieApi)
        {
            _api = api;
            _cache = cache;
            _movieApi = movieApi;
        }

        [HttpGet("tv/{tvdbid}")]
        public async Task<string> GetTvBanner(int tvdbid)
        {
            if (tvdbid <= 0)
            {
                return string.Empty;
            }

            var images = await _cache.GetOrAddAsync($"tv{tvdbid}", () => _api.GetTvImages(tvdbid), DateTimeOffset.Now.AddDays(1));
            if (images == null)
            {
                return string.Empty;
            }
            if (images.tvbanner != null)
            {
                var enImage = images.tvbanner.Where(x => x.lang == "en").OrderByDescending(x => x.likes).Select(x => x.url).FirstOrDefault();
                if (enImage == null)
                {
                    return images.tvbanner.OrderByDescending(x => x.likes).Select(x => x.url).FirstOrDefault();
                }
            }
            if (images.seasonposter != null)
            {
                return images.seasonposter.FirstOrDefault()?.url ?? string.Empty;
            }
            return string.Empty;
        }

        [HttpGet("poster")]
        public async Task<string> GetRandomPoster()
        {
            var rand = new Random();
            var val = rand.Next(1, 3);
            if (val == 1)
            {
                var movies = (await _movieApi.PopularMovies()).ToArray();
                var selectedMovieIndex = rand.Next(movies.Count());
                var movie = movies[selectedMovieIndex];

                var images = await _cache.GetOrAddAsync($"movie{movie.Id}", () => _api.GetMovieImages(movie.Id.ToString()), DateTimeOffset.Now.AddDays(1));
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
            }
            else
            {
                var tv = (await _movieApi.PopularTv()).ToArray();
                var selectedMovieIndex = rand.Next(tv.Count());
                var selected = tv[selectedMovieIndex];

                return $"https://image.tmdb.org/t/p/original{selected.BackdropPath}";
            }
            return "";
        }
    }
}
