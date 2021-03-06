using System;
using System.Collections.Generic;

namespace Marketplace.Ads.Projections;

public static class ReadModels
{
    public class ClassifiedAdDetails
    {
        public string Id { get; set; }
        public Guid SellerId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }

        public List<string> PhotoUrls { get; set; } = new();

        public static string GetDatabaseId(Guid id)
        {
            return $"ClassifiedAdDetails/{id}";
        }
    }

    public class MyClassifiedAds
    {
        public string Id { get; set; }
        public List<MyAd> MyAds { get; set; }

        public static string GetDatabaseId(Guid id)
        {
            return $"MyClassifiedAds/{id}";
        }

        public class MyAd
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }

            public List<string> PhotoUrls { get; set; } = new();
        }
    }
}