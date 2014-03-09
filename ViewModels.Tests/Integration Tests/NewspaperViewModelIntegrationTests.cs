using System;
using FluentAssertions;
using Granite.Testing;
using Models;
using NSubstitute;
using ViewModels;
using Xunit;

namespace ViewModelTests.IntegrationTests
{
    public class HewspaperViewModelIntegrationTests
    {
//        private string NEW_PAPER_NAME = "New paper name";
        private string TEST_PAPER_NAME = "Test paper name";


        #region Utility Object Creation Routines

        private Newspaper GetNewEmptyNewspaper()
        {
            return new Newspaper();
        }

        private static NewspaperViewModel GetNewspaperViewModel(Newspaper paper, IRepository<Newspaper, Guid> repository = null)
        {
            var repo = repository ?? Substitute.For<IRepository<Newspaper, Guid>>();
            return new NewspaperViewModel(paper, repo);
        }

        private Newspaper GetNewPopulatedNewspaper()
        {
            return new Newspaper { Name = TEST_PAPER_NAME };
        }

        #endregion Utility Object Creation Routines
 
    }
}
