﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Granite.Testing;
using Models;
using Xunit;
using Xunit.Extensions;

namespace Model.Tests.UnitTests
{
    public class NewspaperUnitTests
    {
        #region Constants and Fields

        private const string TEST_PAPER_NAME = "Test Newspaper Name";

        #endregion

        #region Class Members

        [Fact]
        public void AddAdvertisement_UsingANullAdvertisement_DoesNotChangeTheNewspaperAdvertisementsCollection()
        {
            //	Arrange
            var paper = GetNamedNewspaper();
            var ad1 = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 text"};
            var ad2 = new Advertisement {Name = "Test ad 2 name", Text = "Test ad 2 text"};
            var ad3 = new Advertisement {Name = "Test ad 3 name", Text = "Test ad 3 text"};
            var localAdList = new List<Advertisement> {ad1, ad2, ad3};

            paper.AddAdvertisements(localAdList);
            paper.Advertisements.Count.Should().Be(3, "All advertisements should be added to the local list");

            //	Act
            paper.AddAdvertisement(null);

            //	Assert

            paper.Advertisements.Count.Should().Be(3, "The collection should remain unchanged.");
            paper.Advertisements.ShouldAllBeEquivalentTo(localAdList, "The original list of Advertisements is still present.");
        }

        [Fact]
        public void AddAdvertisement_WithAValidAdvertisement_AddsTheAdvertisementToTheAdvertisementsCollection()
        {
            //	Arrange

            var paper = GetNamedNewspaper();
            var ad1 = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 text"};
            var ad2 = new Advertisement {Name = "Test ad 2 name", Text = "Test ad 2 text"};
            var ad3 = new Advertisement {Name = "Test ad 3 name", Text = "Test ad 3 text"};

            var localAdList = new ObservableCollection<Advertisement> {ad1, ad2, ad3};
            paper.Advertisements = localAdList;
            paper.Advertisements.ShouldAllBeEquivalentTo(localAdList, "All advertisements should be added");

            var newAd = new Advertisement {Name = "New test ad", Text = "New test ad text"};
            localAdList.Add(newAd);

            localAdList.Count.Should().Be(4, "The new ad should be added to the local list.");

            //	Act
            paper.AddAdvertisement(newAd);

            //	Assert
            paper.Advertisements.ShouldAllBeEquivalentTo(localAdList,
                                                         "The local and object Advertisement lists should have the same contents.");
        }

        [Theory]
        [PropertyData("EmptyAdListNoCount")]
        [PropertyData("SingleAdListNoCount")]
        [PropertyData("FiveAdListNoCount")]
        public void AddAdvertisement_WithValidAdvertisements_CreatesAReferenceToTheContainingNewspaperInEachAddedAdvertisement(
            List<Advertisement> advertisements)
        {
            //  Arrange
            var paper = GetEmptyNewspaper();
            var ocAds = new ObservableCollection<Advertisement>(advertisements);

            //  Act
            advertisements.ForEach(paper.AddAdvertisement);

            //  Assert
            foreach (var ad in paper.Advertisements)
                ad.Newspapers.Contains(paper).Should().Be(true, "The item is in the list.");
        }

        [Fact]
        public void AddAdvertisements_WithANullList_DoesNotChangeTheAdvertisementsCollection()
        {
            //	Arrange
            var paper = GetNamedNewspaper();
            var ad1 = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 text"};
            var ad2 = new Advertisement {Name = "Test ad 2 name", Text = "Test ad 2 text"};
            var ad3 = new Advertisement {Name = "Test ad 2 name", Text = "Test ad 3 text"};

            var localAdList = new List<Advertisement> {ad1, ad2, ad3};
            paper.AddAdvertisements(localAdList);

            paper.Advertisements.ShouldAllBeEquivalentTo(localAdList,
                                                         "All original advertisements were added to the Newspaper Advertisements collection");

            //	Act
            paper.AddAdvertisements(null);

            //	Assert
            paper.Advertisements.ShouldAllBeEquivalentTo(localAdList,
                                                         "Adding a null list does not impact the Newspaper Advertisements collection");
        }

        [Fact]
        public void AddAdvertisments_WithAListOfAdvertisements_PopulatesTheAdvertisementsCollection()
        {
            //	Arrange

            var paper = GetNamedNewspaper();
            var ad1 = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 text"};
            var ad2 = new Advertisement {Name = "Test ad 2 name", Text = "Test ad 2 text"};
            var ad3 = new Advertisement {Name = "Test ad 2 name", Text = "Test ad 3 text"};

            var localAdList = new List<Advertisement> {ad1, ad2, ad3};

            //	Act
            paper.AddAdvertisements(localAdList);

            //	Assert
            paper.Advertisements.ShouldAllBeEquivalentTo(localAdList,
                                                         "All advertisements added from a list appear in the Newspaper Advertisements collection");
        }

        [Fact]
        public void Advertisements_WithNoAssociatedAdvertisementsAdded_ReturnsAnEmptyAdvertisementsCollection()
        {
            //  Arrange
            var paper = GetEmptyNewspaper();

            //  Act
            //  Assert
            paper.Advertisements.Count.Should().Be(0, "No ads on a paper return an empty collection.");
        }

        [Theory]
        [PropertyData("EmptyAdListWithCount")]
        [PropertyData("SingleAdListWithCount")]
        [PropertyData("FiveAdListWithCount")]
        public void Advertisements_WithPropertySetToAList_ReturnsCorrectCount(List<Advertisement> advertisements, int adCount)
        {
            //  Arrange
            var paper = GetNamedNewspaper();
            var ocList = new ObservableCollection<Advertisement>(advertisements);

            //  Act
            paper.Advertisements = ocList;

            //  Assert
            paper.Advertisements.Count.Should().Be(adCount, "The number of ads in the list match the input list count");
        }

        [Fact]
        public void HasErrors_EmptyObjectCreationWithName_IsFalse()
        {
            //  Arrange
            var paper = GetNamedNewspaper();

            //  Act
            //  Assert
            paper.HasErrors.Should().Be(false, "Any newspaper name is valid");
        }

        [Fact]
        public void HasErrors_EmptyObjectCreationWithoutName_IsTrue()
        {
            //  Arrange
            var paper = GetEmptyNewspaper();

            //  Act
            //  Assert
            paper.HasErrors.Should().Be(true, "Newspapers must have a name");
        }

        [Fact]
        public void HasErrors_ExistingObjectCreatedByKey_IsFalse()
        {
            //	Arrange
            var paper = GetExistingNewspaper();
            
            //	Act
            //	Assert
            paper.HasErrors.Should().Be(false, "Existing models already have a name.");
        }

        private Newspaper GetExistingNewspaper()
        {
            var paper = new Newspaper {Name = TEST_PAPER_NAME};
            paper.DbStatus = DbModificationState.Unchanged;
            return paper;
        }

        [Fact]
        public void HasErrors_WhenInvalidNameSetToNonEmptyString_IsFalse()
        {
            //	Arrange
            var paper = GetUnnamedNewspaper();
            paper.HasErrors.Should().Be(true, "Name is required");

            //	Act
            var newName = TEST_PAPER_NAME;

            paper.Name = newName;
            paper.Name.Should().Be(newName, "Verify name was actually set.");

            //	Assert

            paper.HasErrors.Should().Be(false, "Setting an invalid name to a valid string makes name valid");
        }

        [Fact]
        public void HasErrors_WhenValidNameSetToNull_IsTrue()
        {
            //	Arrange
            var paper = GetNamedNewspaper();
            paper.HasErrors.Should().Be(false, "Verify the name is valid after creation.");

            //	Act

            paper.Name = null;
            paper.Name.Should().Be("", "Verify name was set to null.");

            //	Assert
            paper.HasErrors.Should().Be(true, "Name is required.");
        }

        [Fact]
        public void RemoveAdvertisement_WithANullAdvertisement_DoesNotChangeTheAdvertisementsConnection()
        {
            //  Arrange
            var paper = new Newspaper();
            var ad1 = new Advertisement {Name = "Test Ad 1 Name", Text = "Test Ad 1 Text"};
            var ad2 = new Advertisement {Name = "Test Ad 2 Name", Text = "Test Ad 2 Text"};
            paper.AddAdvertisement(ad1);
            paper.AddAdvertisement(ad2);

            //  act

            paper.RemoveAdvertisement(null);

            //  Assert
            paper.Advertisements.Should().BeEquivalentTo(new List<Advertisement> {ad1, ad2},
                                                         "Removing a null item results in nothing being removed.");
        }

        [Fact]
        public void RemoveAdvertisement_WithAnAdvertisementInTheAdvertisementsCollection_RemovesThatAdvertisementFromTheCollection
            ()
        {
            //  Arrange
            var paper = new Newspaper();
            var ad1 = new Advertisement {Name = "Test Ad 1 Name", Text = "Test Ad 1 Text"};
            var ad2 = new Advertisement {Name = "Test Ad 2 Name", Text = "Test Ad 2 Text"};
            paper.AddAdvertisement(ad1);
            paper.AddAdvertisement(ad2);

            //  act

            paper.RemoveAdvertisement(ad2);

            //  Assert
            paper.Advertisements.Should().BeEquivalentTo(new List<Advertisement> {ad1},
                                                         "Removing the second added Ad results in only the first being left.");
        }

        [Fact]
        public void
            RemoveAdvertisement_WithAnAdvertisementInTheAdvertisementsCollection_RemovesTheNewspaperReferenceFromTheAdvertisementRemoved
            ()
        {
            //	Arrange

            //	Act

            //	Assert
        }

        [Fact]
        public void RemoveAdvertisementsWithANullListOfAdvertisements_DoesNotChangeTheAdvertisementsCollection()
        {
            //  Arrange

            var paper = GetNamedNewspaper();
            var ad1 = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 Text"};
            var ad2 = new Advertisement {Name = "Test ad 2 name", Text = "Test ad 2 Text"};
            var ad3 = new Advertisement {Name = "Test ad 3 name", Text = "Test ad 3 Text"};
            var ad4 = new Advertisement {Name = "Test ad 4 name", Text = "Test ad 4 Text"};
            var ad5 = new Advertisement {Name = "Test ad 5 name", Text = "Test ad 5 Text"};

            paper.Advertisements = new ObservableCollection<Advertisement> {ad1, ad2, ad3, ad4, ad5};
            //  Act

            paper.RemoveAdvertisements(null);

            //  Assert

            paper.Advertisements.ShouldAllBeEquivalentTo(new List<Advertisement> {ad5, ad4, ad3, ad2, ad1});
        }

        [Fact]
        public void RemoveAdvertisements_WithAListOfAdvertisementsInTheCollection_RemovesTheItemsFromTheAdvertisementsCollection()
        {
            //  Arrange

            var paper = GetNamedNewspaper();
            var ad1 = new Advertisement {Name = "Test ad 1 name", Text = "Test ad 1 Text"};
            var ad2 = new Advertisement {Name = "Test ad 2 name", Text = "Test ad 2 Text"};
            var ad3 = new Advertisement {Name = "Test ad 3 name", Text = "Test ad 3 Text"};
            var ad4 = new Advertisement {Name = "Test ad 4 name", Text = "Test ad 4 Text"};
            var ad5 = new Advertisement {Name = "Test ad 5 name", Text = "Test ad 5 Text"};

            paper.Advertisements = new ObservableCollection<Advertisement> {ad1, ad2, ad3, ad4, ad5};
            //  Act

            paper.RemoveAdvertisements(new List<Advertisement> {ad2, ad4});

            //  Assert

            paper.Advertisements.ShouldAllBeEquivalentTo(new List<Advertisement> {ad1, ad3, ad5});
        }

        [Fact]
        public void Name_WhenModified_PerformsPropertyChangedCallback()
        {
            //	Arrange
            const string PAPER_NAME = "Paper 1 name";
            const string NEW_PAPER_NAME = "New paper 1 name";

            var paper = new Newspaper() { Name = PAPER_NAME };
            var eventAssert = new PropertyChangedEventAssert(paper);

            paper.Name.Should().Be(PAPER_NAME, "Ad name set properly");
            eventAssert.ExpectNothing();
            paper.Name = NEW_PAPER_NAME;
            eventAssert.Expect("Name");
        }

        [Fact]
        public void Name_WhenModifiedFromValidToInvalidChangesHasErrors_PerformsErrorsChangedCallback()
        {
            //	Arrange
            const string PAPER_NAME = "Paper 1 name";
            const string NEW_PAPER_NAME = "";
            var errorChangedCalled = false;

            var paper = new Newspaper() { Name = PAPER_NAME };
            paper.ErrorsChanged += delegate(object sender,System.ComponentModel.DataErrorsChangedEventArgs e)
                                   {
                                       errorChangedCalled = true;
                                   };

            paper.Name = NEW_PAPER_NAME;
            errorChangedCalled.Should().Be(true, "Event callback should have occurred.");
        }

        void paper_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Name_WhenModifiedFromValidToInvalidChangesHasErrors_PerformsPropertyChangedCallback()
        {
            //	Arrange
            const string PAPER_NAME = "Paper 1 name";
            const string NEW_PAPER_NAME = "";

            var paper = new Newspaper() { Name = PAPER_NAME };
            var eventAssert = new PropertyChangedEventAssert(paper);

            paper.Name.Should().Be(PAPER_NAME, "Ad name set properly");
            eventAssert.ExpectNothing();
            paper.Name = NEW_PAPER_NAME;
            eventAssert.Expect("Name");
        }
        #endregion

        #region ProperData Values for Inline Test Data

        public static IEnumerable<object[]> EmptyAdListNoCount
        {
            get { return new[] {new[] {(object) (new List<Advertisement>())}}; }
        }

        public static IEnumerable<object[]> EmptyAdListWithCount
        {
            get { return new[] {new[] {(object) (new List<Advertisement>()), 0}}; }
        }

        public static IEnumerable<object[]> FiveAdListNoCount
        {
            get
            {
                return new[]
                       {
                           new[]
                           {
                               (object)
                               (new List<Advertisement>
                                {
                                    new Advertisement {Name = "First Ad Name", Text = "First Ad Text"},
                                    new Advertisement {Name = "Second Ad Name", Text = "Second Ad Text"},
                                    new Advertisement {Name = "Third Ad Name", Text = "Third Ad Text"},
                                    new Advertisement {Name = "Fourth Ad Name", Text = "Fourth Ad Text"},
                                    new Advertisement {Name = "Fifth Ad Name", Text = "Fifth Ad Text"}
                                })
                           }
                       };
            }
        }

        public static IEnumerable<object[]> FiveAdListWithCount
        {
            get
            {
                return new[]
                       {
                           new[]
                           {
                               (object)
                               (new List<Advertisement>
                                {
                                    new Advertisement {Name = "First Ad Name", Text = "First Ad Text"},
                                    new Advertisement {Name = "Second Ad Name", Text = "Second Ad Text"},
                                    new Advertisement {Name = "Third Ad Name", Text = "Third Ad Text"},
                                    new Advertisement {Name = "Fourth Ad Name", Text = "Fourth Ad Text"},
                                    new Advertisement {Name = "Fifth Ad Name", Text = "Fifth Ad Text"}
                                }),
                               5
                           }
                       };
            }
        }

        public static IEnumerable<object[]> SingleAdListNoCount
        {
            get
            {
                return new[]
                       {
                           new[]
                           {
                               (object)
                               (new List<Advertisement> {new Advertisement {Name = "First Ad Name", Text = "First Ad Text"}})
                           }
                       };
            }
        }

        public static IEnumerable<object[]> SingleAdListWithCount
        {
            get
            {
                return new[]
                       {
                           new[]
                           {
                               (object)
                               (new List<Advertisement> {new Advertisement {Name = "First Ad Name", Text = "First Ad Text"}}),
                               1
                           }
                       };
            }
        }

        #endregion ProperData Values for Inline Test Data

        #region Common objects

        #region Instance fields

        #endregion

        #region Class Members

        private static Newspaper GetEmptyNewspaper()
        {
            return new Newspaper();
        }

        private static Newspaper GetNamedNewspaper()
        {
            return new Newspaper() {Name = TEST_PAPER_NAME};
        }

        private Newspaper GetUnnamedNewspaper()
        {
            return new Newspaper() {Name = null};
        }

        #endregion

        #endregion CommonObjects
    }
}