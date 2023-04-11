using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

namespace Uni.Sage.Infrastructures.Services
{
    public interface IInterfaceArticle
    {
        [Get("/GetAllArticles/{pConnexionName}")]
        Task<IEnumerable<ArticleResponse>> GetArticles();

    }
    class Program
    {
        public static async Task Main()
        {
            var API = RestService.For<IInterfaceArticle>("http://localhost:5050");
            var sugars = await API.GetArticles();
        }
    }

}
