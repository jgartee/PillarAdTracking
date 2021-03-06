﻿using System;
using System.ComponentModel;
using FluentAssertions;
using Granite.Testing;
using Models;
using NSubstitute;
using ViewModels;
using Xunit;

namespace ViewModelTests.UnitTests
{
    public class NewspaperViewModelUnitTests
    {
        #region Instance fields

        private string NEW_PAPER_NAME = "New paper name";
        private string TEST_PAPER_NAME = "Test paper name";

        #endregion

        #region Class Members

        [Fact]
        public void DeleteCommand_WhenInvoked_InvokesDeleteOnRepositoryWithModel()
        {
            //	Arrange
            var paper = GetPopulatedNewspaper();
            var mockPaperRepository = Substitute.For<IRepository<Newspaper, Guid>>();
            mockPaperRepository.Delete(paper);
            var vm = GetNewspaperViewModel(paper, mockPaperRepository);

            //	Act
            vm.DeleteCommand.Execute(paper);

            //	Assert
            mockPaperRepository.Received().Delete(paper);
        }

        [Fact]
        public void HasErrors_WhenModified_PerformsErrorsChangedCallbackOnNameAndHasErrors()
        {
            //	Arrange
            var paper = GetPopulatedNewspaper();
            var vm = GetNewspaperViewModel(paper);
            paper.HasErrors.Should().Be(false, "Name should be valid");
            var errorChangedCallbackOccurred = false;
            paper.ErrorsChanged += delegate(object sender, DataErrorsChangedEventArgs e)
                                   {
                                       errorChangedCallbackOccurred = true;
                                   };

            //	Act
            vm.Name = null;

            //	Assert
            errorChangedCallbackOccurred.Should().Be(true, "Callback occurred");
        }

        [Fact(Skip = "Possible invalid test.")]
        public void HasErrors_WhenInvalidNameChangedToValidName_PerformsErrorsChangedCallbackOnNameProperty()
        {
            //	Arrange
            var paper = GetEmptyNewspaper();
            var vm = GetNewspaperViewModel(paper);
            paper.HasErrors.Should().Be(false, "Name should be invalid");
            var callbackOccurred = false;
            var callbackPropertyName = "";

            paper.ErrorsChanged += delegate(object sender, DataErrorsChangedEventArgs e)
                                   {
                                       callbackOccurred = true;
                                       callbackPropertyName = e.PropertyName;
                                   };
            //	Act
            vm.Name = NEW_PAPER_NAME;

            //	Assert
            vm.HasErrors.Should().Be(false, "Name should now be valid.");

        }
        [Fact]
        public void HasErrors_WhenValidNameIsSetToNull_IsFalse()
        {
            //	Arrange
            var paper = GetPopulatedNewspaper();
            var vm = GetNewspaperViewModel(paper);

            //	Act
            vm.Name = null;

            //	Assert
            vm.HasErrors.Should().Be(false, "Empty name is invalid");
        }

        [Fact]
        public void Name_WhenModified_PerformsPropertyChangedCallback()
        {
            //  Arrange
            var paper = GetPopulatedNewspaper();
            var vm = GetNewspaperViewModel(paper);
            paper.Name.Should().Be(TEST_PAPER_NAME, "Name should be set.");

            //  Act
            var eventAssert = new PropertyChangedEventAssert(paper);
            eventAssert.ExpectNothing();
            vm.Name = NEW_PAPER_NAME;

            //  Assert

            paper.Name.Should().Be(NEW_PAPER_NAME, "ViewModel should set new name in the model");
            eventAssert.Expect("Name");
        }

        [Fact]
        public void SaveCommand_WhenInvoked_InvokesSaveOnRepositoryWithModel()
        {
            //	Arrange
            var paper = GetPopulatedNewspaper();
            var mockPaperRepository = Substitute.For<IRepository<Newspaper, Guid>>();
            mockPaperRepository.Save(paper);
            var vm = GetNewspaperViewModel(paper, mockPaperRepository);

            //	Act
            vm.SaveCommand.Execute(paper);

            //	Assert
            mockPaperRepository.Received().Save(paper);
        }
        #endregion

        #region Utility Object Creation Routines

        private Newspaper GetEmptyNewspaper()
        {
            return new Newspaper();
        }

        private static NewspaperViewModel GetNewspaperViewModel(Newspaper paper, IRepository<Newspaper, Guid> repository = null)
        {
            var repo = repository ?? Substitute.For<IRepository<Newspaper, Guid>>();
            return new NewspaperViewModel(paper, repo);
        }

        private Newspaper GetPopulatedNewspaper()
        {
            return new Newspaper {Name = TEST_PAPER_NAME};
        }

        #endregion Utility Object Creation Routines
    }
}