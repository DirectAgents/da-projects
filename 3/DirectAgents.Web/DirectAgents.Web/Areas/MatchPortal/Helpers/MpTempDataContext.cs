using System.Collections.Generic;

using DirectAgents.Web.Areas.MatchPortal.Models;

namespace DirectAgents.Web.Areas.MatchPortal.Helpers
{
    public static class MpTempDataContext
    {
        public static IReadOnlyCollection<string> Categories { get; } = new[] { "handbags1", "handbags2", "handbags3", "handbags4", "handbags5" };

        public static IReadOnlyCollection<string> Brands { get; } = new[] { "GUCCI", "BOTTEGA VENETA", "Saint Laurent", "ZARA", "PRADA" };

        public static IReadOnlyCollection<SearchResultVM> SearchResults { get; } =
            new[]
            {
                new SearchResultVM
                {
                    Id = 1,
                    ImageLink = "https://static-buyma-com.akamaized.net/imgdata/item/191122/0049220739/228872974/org.jpg",
                    ResultLink = "https://www.buyma.us/items/fe7591d8-c3e8-47c5-ba7d-29c9e2675e7d/",
                    Headline = "Shop BOTTEGA VENETA 2019-20AW Blended Fabrics Home ...",
                },
                new SearchResultVM
                {
                    Id = 2,
                    ImageLink = "https://static-buyma-com.akamaized.net/imgdata/item/191128/0049390134/229556430/org.jpg",
                    ResultLink = "https://www.buyma.com/r/_BOTTEGA-VENETA-%E3%83%9C%E3%83%83%E3%83%86%E3%82%AC%E3%83%B4%E3%82%A7%E3%83%8D%E3%82%BF/-C2114/?tag_ids=502",
                    Headline = "BOTTEGA VENETA 2019-20AW Lambskin Long Wallets ...",
                },
                new SearchResultVM
                {
                    Id = 3,
                    ImageLink = "https://i.ebayimg.com/images/g/qKQAAOSwcH5cMBCH/s-l1600.jpg",
                    ResultLink = "https://www.ebay.ca/p/20014583804?iid=112648433067",
                    Headline = "Coach F25519 Men Compact ID 3 in 1 Wallet Signature PVC ...",
                },
                new SearchResultVM
                {
                    Id = 4,
                    ImageLink = "https://img.shopstyle-cdn.com/sim/e8/1b/e81be96c352ed4f40cfd6fdfb25fd77e/kate-spade-new-york-tiny-cabana-dot-elephant-leather-crossbody-bag.jpg",
                    ResultLink = "https://www.shopstyle.com/browse?fts=white+handbag+charmes",
                    Headline = "New Authentic Men Coach F25519 3-In-1 Signature Wallet ...",
                },
                new SearchResultVM
                {
                    Id = 5,
                    ImageLink = "https://img.shopstyle-cdn.com/sim/e8/1b/e81be96c352ed4f40cfd6fdfb25fd77e/tiny-cabana-dot-elephant-leather-crossbody-bag.jpg",
                    ResultLink = "https://www.shape.com/shop/womens-accessories-bags-crossbodies-kate-spade-3d-cabana-dot-heart-crossbody-parchment-pe69e174e80f361186ae6d52caf853459.html",
                    Headline = "Buy Coach F25519 Men Compact ID 3 in 1 Wallet Signature ...",
                },
            };

        public static IReadOnlyCollection<MatchProductVM> Procucts { get; } =
            new[]
            {
                new MatchProductVM
                {
                    Id = 1,
                    OriginalProductTitle = "GUCCI GG Marmont Mens Folding Wallets Black (inventory check required)",
                    ProductImageLink = "https://img.shopstyle-cdn.com/sim/e8/1b/e81be96c352ed4f40cfd6fdfb25fd77e/kate-spade-new-york-tiny-cabana-dot-elephant-leather-crossbody-bag.jpg",
                    ProductPageLink = "https://adaxshop.com/gb/men/wallets/",
                },
                new MatchProductVM
                {
                    Id = 2,
                    OriginalProductTitle = "kate spade new york Womens Handbags White",
                    ProductImageLink = "https://images.prod.meredith.com/product/144d1c3ef3d37f7a556c9ae7eb29b0f8/1581976800779/m/tiny-cabana-dot-elephant-leather-crossbody-bag-blue-kate-spade-shoulder-bags",
                    ProductPageLink = "https://www.shopstyle.com/browse?fts=kate+spade+new+york+handbag",
                },
                new MatchProductVM
                {
                    Id = 3,
                    OriginalProductTitle = "BOTTEGA VENETA Womens Long Wallets Concrete 2020 SS",
                    ProductImageLink = "https://static-buyma-com.akamaized.net/imgdata/item/180705/0037045659/153163171/428.jpg",
                    ProductPageLink = "https://www.shopstyle.com/browse?fts=white+handbag+charmes",
                },
                new MatchProductVM
                {
                    Id = 4,
                    OriginalProductTitle = "Saint Laurent Womens Long Wallets DENIM 2018-19AW",
                    ProductImageLink = "https://static-buyma-jp.akamaized.net/imgdata/item/191202/0049491272/230162023/428.jpg",
                    ProductPageLink = "https://www.shopstyle.com/browse?fts=kate+spade+eye+handbag",
                },
                new MatchProductVM
                {
                    Id = 5,
                    OriginalProductTitle = "BOTTEGA VENETA Womens Long Wallets CIPRIA/BRIGHT RED 2020 SS",
                    ProductImageLink = "https://cdn-images.farfetch-contents.com/13/81/29/92/13812992_17292404_300.jpg",
                    ProductPageLink = "https://www.shopstyle.com/browse/handbags?fts=elephant+leather+bag",
                },
            };
    }
}