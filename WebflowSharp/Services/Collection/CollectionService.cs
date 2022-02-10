using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WebflowSharp.Entities;
using WebflowSharp.Extensions;
using WebflowSharp.Infrastructure;

namespace WebflowSharp.Services.Collection
{
    public class CollectionService : WebflowService
    {
        public CollectionService(string shopAccessToken) : base(shopAccessToken)
        {
        }

        /// <summary>
        /// Returns List of collection
        /// </summary>
        /// <param name="siteId">	Unique identifier for the site</param>
        public virtual async Task<List<SiteCollection>> GetCollections(string siteId)
        {
            var req = PrepareRequest($"sites/{siteId}/collections");
            return await ExecuteRequestAsync<List<SiteCollection>>(req, HttpMethod.Get);
        }

        /// <summary>
        /// Get Collection with Full Schema
        /// </summary>
        /// <param name="collectionId">Unique identifier for the Collection</param>
        /// <returns>The <see cref="Order"/>.</returns>
        public virtual async Task<SiteCollection> GetCollectionById(string collectionId)
        {
            var req = PrepareRequest($"collections/{collectionId}");
            return await ExecuteRequestAsync<SiteCollection>(req, HttpMethod.Get);
        }
        
        /// <summary>
        /// 	Unique identifier for the Collection you are querying
        /// </summary>
        public virtual async Task<ProductQueryResponse> GetCollectionProducts(
            string collectionId, CollectionQueryParameters queryParameters)
        {
            var req = PrepareRequest($"collections/{collectionId}/items");
            if (queryParameters != null) req.QueryParams.AddRange(queryParameters.ToParameters());
            return await ExecuteRequestAsync<ProductQueryResponse>(req, HttpMethod.Get);
        }

        /// <summary>
        /// 	Unique identifier for the Collection you are querying
        /// </summary>
        public virtual async Task<VariantQueryResponse> GetCollectionVariants(
            string collectionId, CollectionQueryParameters queryParameters)
        {
            var req = PrepareRequest($"collections/{collectionId}/items");
            if (queryParameters != null) req.QueryParams.AddRange(queryParameters.ToParameters());
            return await ExecuteRequestAsync<VariantQueryResponse>(req, HttpMethod.Get);
        }

        /// <summary>
        /// Get Single Item
        /// </summary>
        /// <param name="collectionId">Unique identifier for the Collection you are querying</param>
        /// <param name="itemId">Unique identifier for the Item you are querying</param>
        /// <returns>The <see cref="Order"/>.</returns>
        public virtual async Task<CreateItem> GetItemById(
            string collectionId, string itemId)
        {
            var req = PrepareRequest($"collections/{collectionId}/items/{itemId}");
            return await ExecuteRequestAsync<CreateItem>(req, HttpMethod.Get);
        }
        
        public virtual async Task<CreateItem> CreateOrUpdate(
            string collectionId,
            string itemId,
            CreateItem item)
        {
            try
            {
                var remoteItem = await GetItemById(collectionId, itemId);
                return await UpdateCollectionItem(collectionId, itemId, item);
            }
            catch
            {
                return await CreateNewCollectionItem(collectionId, item);
            }
        }

        /// <summary>
        /// An “update item” request" replaces the fields of an existent item with the fields specified in the payload.
        /// </summary>
        /// <param name="collectionId">Unique identifier for the Collection you are querying</param>
        /// <param name="itemId">Unique identifier for the Item you are querying</param>
        /// <param name="collectionItemQueryParameters"></param>
        /// <returns>The <see cref="Order"/>.</returns>
        public virtual async Task<CreateItem> UpdateCollectionItem(
            string collectionId,
            string itemId,
            CreateItem item)
        {
            var req = PrepareRequest($"collections/{collectionId}/items/{itemId}");
            HttpContent content = null;

            if (item != null)
            {
                var body = item.ToDictionary();
                content = new JsonContent(body);
            }

            var result = await ExecuteRequestAsync<JObject>(req, HttpMethod.Put, content);

            return item;
        }

        /// <summary>
        /// An “create item” request" creates a new item.
        /// </summary>
        /// <param name="collectionId">Unique identifier for the Collection you are querying</param>
        /// <param name="item"></param>
        /// <returns>The <see cref="Item"/>.</returns>
        public virtual async Task<CreateItem> CreateNewCollectionItem(
            string collectionId,
            CreateItem item)
        {
            var req = PrepareRequest($"collections/{collectionId}/items");
            HttpContent content = null;

            if (item != null)
            {
                var body = item.ToDictionary();
                content = new JsonContent(body);
            }

            var result = await ExecuteRequestAsync<JObject>(req, HttpMethod.Post, content);

            return item;
        }

        /// <summary>
        /// An “update item” request" replaces the fields of an existent item 
        /// with the fields specified in the payload.
        /// </summary>
        /// <param name="collectionId">Unique identifier for the Collection you are querying</param>
        /// <param name="itemId">Unique identifier for the Item you are querying</param>
        /// <param name="collectionItemQueryParameters"></param>
        /// <returns>The <see cref="Order"/>.</returns>
        public virtual async Task<JObject> RemoveCollectionItem(
            string collectionId,
            string itemId)
        {
            var req = PrepareRequest($"collections/{collectionId}/items/{itemId}");
            return await ExecuteRequestAsync<JObject>(req, HttpMethod.Delete);
        }
    }
}
