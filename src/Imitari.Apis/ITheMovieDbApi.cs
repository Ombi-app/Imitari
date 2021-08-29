using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Imitari.Apis.Models;
using Refit;

namespace Imitari.Apis
{
    public interface ITheMovieDbApi
    {
        [Get("/discover/movie?sort_by=popularity.desc&api_key=b8eabaf5608b88d0298aa189dd90bf00")]
        Task<TheMovieDbContainer<MovieDbSearchResult>> PopularMovies();
        [Get("/discover/tv?sort_by=popularity.desc&api_key=b8eabaf5608b88d0298aa189dd90bf00")]
        Task<TheMovieDbContainer<MovieDbSearchResult>> PopularTv();
    }
}
