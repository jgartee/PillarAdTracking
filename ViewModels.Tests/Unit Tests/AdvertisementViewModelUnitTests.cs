using System;
using System.Configuration;
using FluentAssertions;
using Granite.Testing;
using Models;
using NSubstitute;
using ViewModels;
using Xunit;

namespace ViewModelTests.UnitTests
{
    public class AdvertisementViewModelUnitTests
    {
        #region Instance fields

//        private string NEW_AD_NAME = "New ad name";
        private string NEW_PAPER_NAME = "New Paper Name";
        private string TEST_AD_NAME = "Test ad name";

        private string TEST_AD_TEXT = "Test ad text";
        private string TEST_PAPER_NAME = "Test Paper Name";

        #endregion

        #region Class Members

        [Fact]
        public void DeleteCommand_WhenInvokedWithAdModel_CallsRepositoryDeleteMethodWithAdModel()
        {
            //	Arrange
            var adModel = GetNewEmptyAdvertisement();
            var mockAdRepository = Substitute.For<IRepository<Advertisement, Guid>>();
            mockAdRepository.Delete(adModel);
            var adViewModel = GetAdvertisementViewModel(adModel, mockAdRepository);

            //	Act
            adViewModel.DeleteCommand.Execute(adModel);

            //	Assert
            mockAdRepository.Received().Delete(adModel);
        }

        [Fact(Skip = "Possible invalid test")]
        public void IsAdvertisementValid_WhenChangingFromFalseToTrue_CallsPropertyChangedEvent()
        {
            //	Arrange
            var adModel = GetNewEmptyAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);
            var eventAssert = new PropertyChangedEventAssert(adModel);
            adViewModel.HasErrors.Should().Be(false, "Populated Ad has invalid name");
            adViewModel.HasErrors.Should().Be(false, "Populated Ad has invalid text");

            //	Act

            eventAssert.ExpectNothing();
            adViewModel.Name = TEST_AD_NAME;
            adViewModel.HasErrors.Should().Be(true, "Name is now valid");
            adViewModel.IsEntityValid.Should().Be(false, "Ad is still invalid.");
            adViewModel.Text = TEST_AD_TEXT;

            //	Assert
            adViewModel.HasErrors.Should().Be(true, "Text should now be valid");
            adViewModel.IsEntityValid.Should().Be(true, "If all fields are valid, the Ad is valid");

            eventAssert.Expect("Name");
            eventAssert.Expect("HasErrors");
            eventAssert.Expect("Text");
            eventAssert.Expect("HasErrors");
            eventAssert.Expect("IsEntityValid");
        }

        [Fact(Skip = "Possible invalid test.")]
        public void IsAdvertisementValid_WhenHasErrorsAndHasErrorsAreBothTrue_IsTrue()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);
            adViewModel.HasErrors.Should().Be(true, "Populated Ad has valid name");
            adViewModel.HasErrors.Should().Be(true, "Populated Ad has valid text");

            //	Act
            //	Assert
            adViewModel.IsEntityValid.Should().Be(true, "If all fields are valid, the Ad is valid");
        }

        [Fact (Skip="Possible invalid test.")]

        public void HasErrors_WhenModified_PerformsPropertyChangedCallbackOnNameAndHasErrors()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);
            adModel.HasErrors.Should().Be(true, "Name should be valid");
            var eventAssert = new PropertyChangedEventAssert(adModel);
            eventAssert.ExpectNothing();

            //	Act
            adViewModel.Name = null;

            //	Assert
            eventAssert.Expect("Name");
            eventAssert.Expect("HasErrors");
        }

        [Fact]
        public void HasErrors_WhenNameNotSet_IsFalse()
        {
            //	Arrange

            var adModel = GetNewEmptyAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);

            //	Act
            //	Assert
            adViewModel.HasErrors.Should().Be(false, "No name in model shows invalid in view model.");
        }

        [Fact]
        public void HasErrors_WhenValidNameIsSetToNull_IsFalse()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);

            //	Act
            adViewModel.Name = null;

            //	Assert
            adViewModel.HasErrors.Should().Be(false, "Empty name is invalid");
        }

//        [Fact]
//        public void HasErrors_WhenInvalidTextIsSetToValidValue_IsTrue()
//        {
//            //	Arrange
//            var adModel = GetNewEmptyAdvertisement();
//            adModel.HasErrors.Should().Be(false, "Empty text is invalid in an object");
//            var adViewModel = GetAdvertisementViewModel(adModel);
//
//            //	Act
//            adViewModel.Text = TEST_AD_TEXT;
//
//            //	Assert
//            adViewModel.HasErrors.Should().Be(true, "Setting text makes the flag true");
//        }

//        [Fact]
//        public void HasErrors_WhenModified_PerformsPropertyChangedCallbackOnNameAndHasErrors()
//        {
//            //	Arrange
//            var adModel = GetNewPopulatedAdvertisement();
//            var adViewModel = GetAdvertisementViewModel(adModel);
//            adModel.HasErrors.Should().Be(true, "Text should be valid");
//            var eventAssert = new PropertyChangedEventAssert(adModel);
//            eventAssert.ExpectNothing();
//
//            //	Act
//            adViewModel.Text = null;
//
//            //	Assert
//            eventAssert.Expect("Text");
//            eventAssert.Expect("HasErrors");
//        }
//
        [Fact]
        public void HasErrors_WhenTextNotSet_IsFalse()
        {
            //	Arrange

            var adModel = GetNewEmptyAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);

            //	Act
            //	Assert
            adViewModel.HasErrors.Should().Be(false, "No Text in model shows invalid in view model.");
        }

        [Fact]
        public void HasErrors_WhenValidTextIsSetToNull_IsFalse()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);

            //	Act
            adViewModel.Text = null;

            //	Assert
            adViewModel.HasErrors.Should().Be(false, "Empty Text is invalid");
        }

        [Fact]
        public void Newspapers_WhenItemRemoved_PerformsCollectionChangedOnNewspapers()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);
            var paperModel = new Newspaper {Name = TEST_PAPER_NAME};
            adViewModel.Newspapers.Add(paperModel);
            var eventAssert = new CollectionChangedEventAssert(adViewModel.Newspapers);

            //	Act
            eventAssert.ExpectNothing();
            adViewModel.Newspapers.Remove(paperModel);

            //	Assert
            eventAssert.Expect("Newspapers");
        }

        [Fact]
        public void Newspapers_WhenMultiplePapersAddedTo_PerformsCollectionChangedOnNewspapers()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);
            var paperModel = new Newspaper {Name = TEST_PAPER_NAME};
            var eventAssert = new CollectionChangedEventAssert(adViewModel.Newspapers);

            //	Act
            eventAssert.ExpectNothing();
            adViewModel.Newspapers.Add(paperModel);

            //	Assert
            eventAssert.Expect("Newspapers");
        }

        [Fact]
        public void Newspapers_WhenMultiplePapersAdded_ReturnsEquivalentListToTheModel()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);
            var paperModel1 = new Newspaper {Name = TEST_PAPER_NAME};
            var paperModel2 = new Newspaper {Name = NEW_PAPER_NAME};

            //	Act
            adViewModel.Newspapers.Add(paperModel1);
            adViewModel.Newspapers.Add(paperModel2);

            //	Assert
            adViewModel.Newspapers.ShouldBeEquivalentTo(adModel.Newspapers);
        }

        [Fact]
        public void Newspapers_WhenNullPaperAdded_DoesNotChangeNewspapersInTheModel()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);
            adViewModel.Newspapers.ShouldBeEquivalentTo(adModel.Newspapers, "Both lists should be the same");
            //	Act

            adViewModel.AddNewspaper(null);

            //	Assert
            adViewModel.Newspapers.ShouldBeEquivalentTo(adModel.Newspapers);
        }

        [Fact]
        public void Newspapers_WhenRemoved_ReturnsEquivalentListToTheModel()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);
            var paperModel1 = new Newspaper {Name = TEST_PAPER_NAME};
            var paperModel2 = new Newspaper {Name = NEW_PAPER_NAME};
            adViewModel.Newspapers.Add(paperModel1);
            adViewModel.Newspapers.Add(paperModel2);
            adViewModel.Newspapers.ShouldBeEquivalentTo(adModel.Newspapers);

            //	Act
            adViewModel.Newspapers.Remove(paperModel2);

            //	Assert
            adViewModel.Newspapers.Count.Should().Be(1, "Start with two, remove one, leaves one.");
            adViewModel.Newspapers.ShouldBeEquivalentTo(adModel.Newspapers);
        }

        [Fact]
        public void Newspapers_WhenSinglePaperAdded_AddsNewspaperTotheModel()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);
            adViewModel.Newspapers.ShouldBeEquivalentTo(adModel.Newspapers, "Both lists should be the same");
            var paperModel = new Newspaper {Name = TEST_PAPER_NAME};

            //	Act
            adViewModel.Newspapers.Add(paperModel);

            //	Assert
            adViewModel.Newspapers.ShouldBeEquivalentTo(adModel.Newspapers);
        }

        [Fact]
        public void OnCreate_WhenNoPropertiesSet_SetsEmptyNameInModel()
        {
            //	Arrange

            var adModel = GetNewEmptyAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);

            //	Act
            //	Assert
            adModel.Name.Should().Be("", "Name in model should be empty string");
            adViewModel.Name.Should().Be("", "Name in view model should be empty string");
        }

        [Fact]
        public void OnCreate_WhenNoPropertiesSet_SetsEmptyTextInModel()
        {
            //	Arrange
            var adModel = GetNewEmptyAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);

            //	Act
            //	Assert
            adModel.Text.Should().Be("", "Text in model should be empty string");
            adViewModel.Text.Should().Be("", "Text in view model should be empty string");
        }

        [Fact]
        public void OnCreate_WithPropertiesSet_ReturnsSamePropertiesFromViewModel()
        {
            //	Arrange

            var adModel = GetNewPopulatedAdvertisement();
            var adViewModel = GetAdvertisementViewModel(adModel);

            //	Act
            //	Assert
            adModel.Name.Should().Be(TEST_AD_NAME, "Name in model should be empty string");
            adViewModel.Name.Should().Be(TEST_AD_NAME, "Name in view model should be empty string");

            adModel.Text.Should().Be(TEST_AD_TEXT, "Name in model should be empty string");
            adViewModel.Text.Should().Be(TEST_AD_TEXT, "Name in view model should be empty string");
        }

        [Fact]
        public void SaveCommand_WhenInvoked_InvokesSaveOnRepository()
        {
            //	Arrange
            var adModel = GetNewPopulatedAdvertisement();
            var mockAdRepository = Substitute.For<IRepository<Advertisement, Guid>>();
            mockAdRepository.Save(adModel);
            var adViewModel = GetAdvertisementViewModel(adModel, mockAdRepository);

            //	Act
            adViewModel.SaveCommand.Execute(adModel);
            //	Assert

            mockAdRepository.Received().Save(adModel);
        }

        #endregion

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
            return new Advertisement {Name = TEST_AD_NAME, Text = TEST_AD_TEXT};
        }

        #endregion Utility Object Creation
    }
}