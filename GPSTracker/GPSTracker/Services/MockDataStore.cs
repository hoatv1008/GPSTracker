﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GPSTracker.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(GPSTracker.Services.MockDataStore))]
namespace GPSTracker.Services
{
	public class MockDataStore : IDataStore<Item>
	{
		bool isInitialized;
		List<Item> items;

		public async Task<bool> AddItemAsync(Item item)
		{
			await InitializeAsync();

			items.Insert(0, item);

            try {
				for (var i = items.Count; i > 100; i--)
					items.RemoveAt(i);
			}
			catch
			{
				// donothing
			}

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(Item item)
		{
			await InitializeAsync();

			var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
			items.Remove(_item);
			items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(Item item)
		{
			await InitializeAsync();

			var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
			items.Remove(_item);

			return await Task.FromResult(true);
		}

		public async Task<bool> ClearItemAsync()
		{
			await InitializeAsync();

            items.Clear();

			return await Task.FromResult(true);
		}

		public async Task<Item> GetItemAsync(string id)
		{
			await InitializeAsync();

			return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
		}

		public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
		{
			await InitializeAsync();

			return await Task.FromResult(items);
		}

		public Task<bool> PullLatestAsync()
		{
			return Task.FromResult(true);
		}


		public Task<bool> SyncAsync()
		{
			return Task.FromResult(true);
		}

		public async Task InitializeAsync()
		{
			if (isInitialized)
				return;

			items = new List<Item>();
            var _items = new List<Item>();

			foreach (Item item in _items)
			{
				items.Add(item);
			}

			isInitialized = true;

            await Task.FromResult(true);
		}
	}
}
