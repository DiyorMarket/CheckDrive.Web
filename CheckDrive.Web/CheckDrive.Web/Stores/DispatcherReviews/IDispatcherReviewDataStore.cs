﻿using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Web.Responses;

namespace CheckDrive.Web.Stores.DispatcherReviews
{
    public interface IDispatcherReviewDataStore
    {
        Task<GetDispatcherReviewResponse> GetDispatcherReviews(int? pageNumber, string? searchString, DateTime? date, int? roleId);
        Task<DispatcherReviewDto> GetDispatcherReview(int id);
        Task<DispatcherReviewDto> CreateDispatcherReview(DispatcherReviewForCreateDto review);
        Task<DispatcherReviewDto> UpdateDispatcherReview(int id, DispatcherReviewForUpdateDto review);
        Task DeleteDispatcherReview(int id);
    }
}
