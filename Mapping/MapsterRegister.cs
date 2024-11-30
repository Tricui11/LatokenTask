using LatokenTask.ExternalApis.Coinmarketcap;
using LatokenTask.ExternalApis.GnewsIo;
using LatokenTask.ExternalApis.NewsapiOrg;
using LatokenTask.Models;
using Mapster;

namespace LatokenTask.Mapping
{
    public partial class MapsterRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<NewsapiOrgArticle, NewsArticle>()
                .Map(d => d.Source, s => "newsapi.org");

            config.NewConfig<GnewsIoArticle, NewsArticle>()
                .Map(d => d.Source, s => "gnews.io");

            config.NewConfig<CoinmarketcapCryptoData, CryptoPriceInfo>()
                .Map(d => d.Source, s => "coinmarketcap")
                .Map(d => d.Price, s => s.Quote.USD.Price)
                .Map(d => d.PercentChange7d, s => s.Quote.USD.PercentChange7d);
        }
    }
}