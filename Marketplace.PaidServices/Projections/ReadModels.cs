namespace Marketplace.PaidServices.Projections;

public static class ReadModels
{
    public class OrderDraft
    {
        public string Id { get; set; }
        public string ClassifiedAdId { get; set; }
        public string CustomerId { get; set; }
        public double Total { get; set; }
        public List<Service> Services { get; set; }

        public static string GetDatabaseId(Guid id)
        {
            return $"OrderDraft/{id}";
        }

        public class Service
        {
            public string Type { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
        }
    }

    public class CompletedOrder
    {
        public string Id { get; set; }
        public string ClassifiedAdId { get; set; }
        public string CustomerId { get; set; }
        public double Total { get; set; }
        public DateTimeOffset FulfilledAt { get; set; }
        public Service[] OrderedServices { get; set; }

        public static string GetDatabaseId(Guid id)
        {
            return $"CompletedOrder/{id}";
        }

        public class Service
        {
            public string Description { get; set; }
            public double Price { get; set; }
        }
    }

    public class ClassifiedAdOrders
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public List<Order> Orders { get; set; }

        public static string GetDatabaseId(Guid id)
        {
            return $"ClassifiedAdOrders/{id}";
        }

        public class Order
        {
            public string OrderId { get; set; }
            public double Total { get; set; }
            public DateTimeOffset FulfilledAt { get; set; }
        }
    }
}