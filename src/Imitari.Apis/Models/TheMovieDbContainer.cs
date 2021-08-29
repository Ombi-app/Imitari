using System;
using System.Collections.Generic;
using System.Text;

namespace Imitari.Apis.Models
{
    public class TheMovieDbContainer<T>
    {
        public List<T> results { get; set; }
    }
}
