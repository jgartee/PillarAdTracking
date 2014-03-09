using System;
using System.Configuration;
using FluentAssertions;
using Granite.Testing;
using Models;
using NSubstitute;
using ViewModels;
using Xunit;

namespace ViewModelTests.IntegrationTests
{
    public class AdvertisementViewModelIntegrationTests
    {
        private string TEST_AD_NAME = "Test ad name";
        private string TEST_AD_TEXT = "Test ad text";

        //        private string NEW_AD_NAME = "New ad name";
        //        private string NEW_PAPER_NAME = "New Paper Name";
        //        private string TEST_PAPER_NAME = "Test Paper Name";

 
 
        #region Utility Object Creation

        private static AdvertisementViewModel GetAdvertisementViewModel(Advertisement adModel,
                                                                        IRepository<Advertisement, Guid> repository = null)
        {
            var repo = repository ?? Substitute.For<IRepository<Advertisement, Guid>>();
            return new AdvertisementViewModel(adModel, repo);
        }

        private static Advertisement GetNewEmptyAdvertisement()
        {
            return new Advertisement();
        }

        private Advertisement GetNewPopulatedAdvertisement()
        {
            return new Advertisement { Name = TEST_AD_NAME, Text = TEST_AD_TEXT };
        }

        #endregion Utility Object Creation
    }
}
