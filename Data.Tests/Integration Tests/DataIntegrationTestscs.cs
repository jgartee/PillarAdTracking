using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Models;
using Xunit.Extensions;

namespace Data.Tests.Integration_Tests
{
    public class DataIntegrationTestscs
    {
        //TODO jlg - Add tests for AdvertisementCache, etc.

        // ReSharper disable InconsistentNaming
        [Theory]
        [PropertyData("FiveNewNewspapersInList")]
        public void SerializerJson_SaveAndRestoreCache_ResultsMatchSavedItems(List<Newspaper> papers)
        {
            //	Arrange
            var serializer = new NewspaperSerializer();
            var cache = new NewspaperCache();
            var repository = new NewspaperRepository(cache, serializer);

            papers.ForEach(repository.Save);

            //	Act
            Directory.SetCurrentDirectory(@"\projects\PillarAdTracking\Data.Tests\bin\Debug");  //  hard-coded only in sample project
            var cacheFileName = Directory.GetFiles(Directory.GetCurrentDirectory()).ToList();
            var fileName = cacheFileName.FirstOrDefault(f => f.EndsWith("NewspaperData.json"));
            //	Assert

            cacheFileName.Should().NotBeNull();

            //  This is two tests because we are using a PropertyData construct and we must deal with each file 
            //  independently.  XUnit runs tests concurrently, and this test must run in isolation.  I could have 
            //  named the file based on the papers.Count, but I didn't.  In real life, I would have.

            //  Arrange
            cache.Clear();
            cache.Values.ToList().Should().BeEmpty("The cache should be empty");

            //  Act
            serializer.RestoreCache(cache);

            //  Assert
            cache.Values.Count.Should().Be(papers.Count, "Only one items should be in the cache");
        }

        #region Properties

        public static IEnumerable<object[]> FiveNewNewspapersInList
        {
            get
            {
                var paper1 = new Newspaper {Name = "New paper 1 odd"};
                var paper2 = new Newspaper {Name = "New paper 2 even"};
                var paper3 = new Newspaper {Name = "New paper 3 odd"};
                var paper4 = new Newspaper {Name = "New paper 4 even"};
                var paper5 = new Newspaper {Name = "New paper 5 odd"};

                paper1.AddAdvertisements(new List<Advertisement>
                                         {
                                                 new Advertisement {Name = "Paper 1 Ad 1", Text = "Paper 1 Text 1"},
                                                 new Advertisement {Name = "Paper 1 Ad 2", Text = "Paper 1 Text 2"},
                                                 new Advertisement {Name = "Paper 1 Ad 3", Text = "Paper 1 Text 3"},
                                                 new Advertisement {Name = "Paper 1 Ad 4", Text = "Paper 1 Text 4"},
                                                 new Advertisement {Name = "Paper 1 Ad 5", Text = "Paper 1 Text 5"},
                                         });

                paper3.AddAdvertisements(new List<Advertisement>
                                         {
                                                 new Advertisement {Name = "Paper 3 Ad 1", Text = "Paper 3 Text 1"},
                                                 new Advertisement {Name = "Paper 3 Ad 2", Text = "Paper 3 Text 2"},
                                                 new Advertisement {Name = "Paper 3 Ad 3", Text = "Paper 3 Text 3"},
                                                 new Advertisement {Name = "Paper 3 Ad 4", Text = "Paper 3 Text 4"},
                                                 new Advertisement {Name = "Paper 3 Ad 5", Text = "Paper 3 Text 5"},
                                         });

                paper5.AddAdvertisements(new List<Advertisement>
                                         {
                                                 new Advertisement {Name = "Paper 5 Ad 1", Text = "Paper 5 Text 1"},
                                                 new Advertisement {Name = "Paper 5 Ad 2", Text = "Paper 5 Text 2"},
                                                 new Advertisement {Name = "Paper 5 Ad 3", Text = "Paper 5 Text 3"},
                                                 new Advertisement {Name = "Paper 5 Ad 4", Text = "Paper 5 Text 4"},
                                                 new Advertisement {Name = "Paper 5 Ad 5", Text = "Paper 5 Text 5"},
                                         });

                return new[] {new[] {(object) (new List<Newspaper> {paper1, paper2, paper3, paper4, paper5})}};
            }
        }

        public static IEnumerable<object[]> SingleNewNewspaperInList
        {
            get
            {
                var paper1 = new Newspaper {Name = "New paper 1 odd"};
                return new[] {new[] {(object) (new List<Newspaper> {paper1})}};
            }
        }

        public static IEnumerable<object[]> ThreeNewNewspapersInList
        {
            get
            {
                var paper1 = new Newspaper {Name = "New paper 1 odd"};
                var paper2 = new Newspaper {Name = "New paper 2 even"};
                var paper3 = new Newspaper {Name = "New paper 3 odd"};
                return new[] {new[] {(object) (new List<Newspaper> {paper1, paper2, paper3})}};
            }
        }

        public static IEnumerable<object[]> TwoNewNewspapersInList
        {
            get
            {
                var paper1 = new Newspaper {Name = "New paper 1 odd"};
                var paper2 = new Newspaper {Name = "New paper 2 even"};

                return new[] {new[] {(object) (new List<Newspaper> {paper1, paper2})}};
            }
        }

        #endregion

        // ReSharper restore InconsistentNaming
    }
}