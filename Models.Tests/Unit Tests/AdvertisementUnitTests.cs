using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Granite.Testing;
using Models;
using Xunit;

namespace Model.Tests.UnitTests
{
    public class AdvertisementUnitTests
    {
        #region Class Members

        [Fact]
        public void AddNewspaper_MultipleTimesWithValidNewspaperObjects_CreatesAReferenceToEachPaper()
        {
            //  Arrange
            var paper1 = new Newspaper() {Name = "Paper 1 name"};
            var paper2 = new Newspaper() {Name = "Paper 2 name"};
            var ad = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 text."};

            //  Act

            ad.AddNewspaper(paper1);
            ad.AddNewspaper(paper2);

            //  Assert
            ad.Newspapers.ShouldBeEquivalentTo(new List<Newspaper> {paper2, paper1});
        }

        [Fact]
        public void AddNewspaper_WithNull_DoesNotChangeTheNewspapersCollection()
        {
            //	Arrange
            var advertisement = new Advertisement {Name = "Test Ad 1", Text = "Test ad 1 text."};

            //	Act

            advertisement.AddNewspaper(null);

            //	Assert

            advertisement.Newspapers.ShouldAllBeEquivalentTo(new List<Newspaper>());
        }

        [Fact]
        public void AddNewspaper_WithValidNewspaperObject_CreatesAReferenceToThatNewspaper()
        {
            //  Arrange
            var paper = new Newspaper() {Name = "Test Paper 1"};
            var advertisement = new Advertisement {Name = "Test Ad 1", Text = "Test ad 1 text."};

            //  Act
            advertisement.AddNewspaper(paper);

            //Assert
            advertisement.Newspapers.ShouldAllBeEquivalentTo(new List<Newspaper> {paper},
                                                             "A paper added to an advertisement adds a reference to that paper.");
        }

        [Fact]
        public void AddNewspapers_WithListOfValidNewspaperstestAddsAllItemsToNewspapersCollection()
        {
            //  Arrange
            var paper1 = new Newspaper() {Name = "Paper 1 name"};
            var paper2 = new Newspaper() {Name = "Paper 2 name"};
            var paper3 = new Newspaper() {Name = "Paper 3 name"};
            var paperList = new List<Newspaper> {paper1, paper2, paper3};

            var ad = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 text."};

            //  Act
            ad.AddNewspapers(paperList);

            //Assert

            ad.Newspapers.ShouldAllBeEquivalentTo(paperList);
        }

        [Fact]
        public void AddNewspapers_WithNullNewspaperList_ReturnsTheOriginalNewspapersCollection()
        {
            //	Arrange
            var paper1 = new Newspaper() {Name = "Paper 1 name"};
            var paper2 = new Newspaper() {Name = "Paper 2 name"};
            var paper3 = new Newspaper() {Name = "Paper 3 name"};
            var ad = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 text."};
            var paperList = new List<Newspaper> {paper1, paper3, paper2};

            ad.AddNewspaper(paper1);
            ad.AddNewspaper(paper2);
            ad.AddNewspaper(paper3);

            //  Act

            ad.AddNewspapers(null);

            //	Assert

            ad.Newspapers.ShouldAllBeEquivalentTo(paperList);
        }

        [Fact]
        public void DbStatus_OnNewObject_IsSetToAdded()
        {
            var testAd = new Advertisement {Name = "Test Ad 1 Name", Text = "Test Ad 1 Text"};

            testAd.IsAdded.Should().Be(true, "A new object should be in the Added state");
        }

        [Fact]
        public void IsNameValid_WhenFalseAndSetToANonNullValue_IsTrue()
        {
            //	Arrange
            var testAd = new Advertisement {Name = null, Text = "test text"};
            testAd.IsNameValid.Should().Be(false, "Creating an object without a name sets name status to invalid");

            //	Act
            var testName = "New Test Name";
            testAd.Name = testName;
            testAd.Name.Should().Be(testName, "The name change was successful");

            //	Assert
            testAd.IsNameValid.Should().Be(true, "The status of Name is true when a non-null assignment is made.");
        }

        [Fact]
        public void IsNameValid_WhenNameIsSetToNull_RetunsFalse()
        {
            //	Arrange

            var testAd = new Advertisement {Name = "Test ad name", Text = "Test ad text"};
            testAd.IsNameValid.Should().Be(true, "Verify that Name is valid when object is created.");

            //	Act
            testAd.Name = null;

            //	Assert
            testAd.IsNameValid.Should().Be(false, "Setting the name of an Advertisement makes the Name invalid.");
        }

        [Fact]
        public void IsNameValid_WhenObjectCreatedWithNonEmptyNameSpecified_IsTrue()
        {
            //  Arrange
            var testAd = new Advertisement {Name = "Test Ad", Text = "Test ad text"};

            //  Act
            //  Assert
            testAd.IsNameValid.Should().Be(true, "Any name for an ad is valid");
        }

        [Fact]
        public void IsNameValid_WhenObjectCreatedWithoutNameSpecified_IsFalse()
        {
            //  Arrange
            var testAd = new Advertisement {Name = null, Text = "Test ad text"};

            //  Act
            //  Assert
            testAd.IsNameValid.Should().Be(false, "Advertisments must have a name.");
        }

        [Fact]
        public void IsNameValid_WhenTrueAndSetToANonNullValue_IsTrue()
        {
            //	Arrange

            var testAd = new Advertisement {Name = "Test ad name", Text = "Test ad text."};
            testAd.IsNameValid.Should().Be(true, "Verify that the flag is already valid.");

            //	Act

            var newName = "New Test Ad Name";
            testAd.Name = newName;

            //	Assert
            testAd.Name.Should().Be(newName, "Verify the name was changed.");
            testAd.IsNameValid.Should().Be(true, "Any new text value assigned to Name is valid.");
        }

        [Fact]
        public void IsTextValid_WhenObjectCreatedWithNonNullTextSpecified_IsTrue()
        {
            //  Arrange
            var testAd = new Advertisement {Name = "Test ad name", Text = "Test ad text."};

            //  Act
            //  Assert
            testAd.IsTextValid.Should().Be(true, "Advertisements must have text");
        }

        [Fact]
        public void IsTextValid_WhenObjectCreatedWithTextSetToNull_IsFalse()
        {
            //  Arrange
            var testAd = new Advertisement {Name = "Test ad name", Text = null};

            //  Act
            //  Assert
            testAd.IsTextValid.Should().Be(false, "Advertisements must have text");
        }

        [Fact]
        public void IsTextValid_WhenTextIsSetToNull_ReturnsFalse()
        {
            //	Arrange

            var testAd = new Advertisement {Name = "Test Ad", Text = "Test ad text"};
            testAd.IsTextValid.Should().Be(true, "Confirm status is valid for text after creation.");

            //	Act

            testAd.Text = null;

            //	Assert

            testAd.IsTextValid.Should().Be(false, "Null text is not valid in an ad.");
        }

        [Fact]
        public void IsTextValid_WhenTextSetToAnyNonEmptyString_IsTrue()
        {
            //  Arrange
            var testAd = new Advertisement {Name = "Test Ad", Text = null};
            testAd.IsTextValid.Should().Be(false, "Verify IsTextValid is false with null value.");

            //  Act
            //  Assert
            testAd.Text = "Ad text body.";
            testAd.IsTextValid.Should().Be(true, "Any text for an ad is valid.");
        }

        [Fact]
        public void Name_WhenModified_PerformsPropertyChangedCallback()
        {
            //	Arrange
            const string AD_NAME = "Test ad 1 name";
            const string NEW_AD_NAME = "New ad name";

            var testAd = new Advertisement {Name = AD_NAME};
            var eventAssert = new PropertyChangedEventAssert(testAd);

            testAd.Name.Should().Be(AD_NAME, "Ad name set properly");
            eventAssert.ExpectNothing();
            testAd.Name = NEW_AD_NAME;
            eventAssert.Expect("Name");
        }

        [Fact]
        public void Newspapers_SetToNull_SetsNewspapersToAnEmptyCollection()
        {
            //	Arrange
            var testAd = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 text."};

            var testPaper1 = new Newspaper() {Name = "Test Paper 1 Name"};
            var testPaper2 = new Newspaper() {Name = "Test Paper 2 Name"};
            var testPaper3 = new Newspaper() {Name = "Test Paper 3 Name"};

            testAd.Newspapers = new ObservableCollection<Newspaper>() {testPaper1, testPaper2, testPaper3};
            testAd.Newspapers.Count.Should().Be(3, "All papers were added to the ad");

            //	Act
            testAd.Newspapers = null;

            //	Assert
            testAd.Newspapers.ShouldBeEquivalentTo(new List<Newspaper>(),
                                                   "Newspaper collection is set to empty list when null is assigned to it.");
        }

        #endregion
    }
}