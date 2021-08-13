using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Imitari.Apis.Models;
using Refit;

namespace Imitari.Apis
{
    public interface IFanartTvApi
    {
        [Get("/tv/{tvdbId}")]
        Task<TvImageResults> GetTvImages(int tvdbId);
        [Get("/movies/{id}")]
        Task<MovieResult> GetMovieImages([AliasAs("id")] string movieOrImdbId);
    }
}
