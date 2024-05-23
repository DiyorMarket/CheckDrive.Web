﻿using CheckDrive.Web.Models;
using CheckDrive.Web.Responses;
using CheckDrive.Web.Service;
using Newtonsoft.Json;

namespace CheckDrive.Web.Stores.Mechanics
{
    public class MechanicDataStore : IMechanicDataStore
    {
        private readonly ApiClient _api;

        public MechanicDataStore(ApiClient apiClient)
        {
            _api = apiClient;
        }
        public async Task<GetMechanicResponse> GetMechanicsAsync()
        {
            var response = await _api.GetAsync("mechanics");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Could not fetch drivers.");
            }

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<GetMechanicResponse>(json);

            return result;
        }
        public Task<Mechanic> CreateMechanicAsync(Mechanic mechanic)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMechanicAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Mechanic> GetMechanicAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Mechanic> UpdateMechanicAsync(int id, Mechanic mechanic)
        {
            throw new NotImplementedException();
        }
    }
}