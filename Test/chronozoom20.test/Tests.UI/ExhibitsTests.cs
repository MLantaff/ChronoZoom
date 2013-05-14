﻿using System;
using Application.Helper.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace Tests
{
    [TestClass]
    public class ExhibitsTests : TestBase
    {
        #region Initialize and Cleanup
        public TestContext TestContext { get; set; }

        readonly static ContentItem ContentItem = new ContentItem
        {
            Title = "contentItem",
            Caption = "This is a test item",
            MediaSource = "https://www.youtube.com/watch?v=eVpXkyN0zOE",
            MediaType = "Video",
            Attribution = "Tests Attribution",
            Uri = "https://www.youtube.com/watch?v=eVpXkyN0zOE"

        };

        static readonly Exhibit _exhibit = new Exhibit
        {
            Title = "WebdriverExhibitWithContent",
            ContentItems = new Collection<Chronozoom.Entities.ContentItem> { ContentItem }
        };

        private static Exhibit _newExhibit;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            BrowserStateManager.RefreshState();
            HomePageHelper.OpenSandboxPage();
            HomePageHelper.DeleteAllElementsLocally();

            ExhibitHelper.AddExhibitWithContentItem(_exhibit);
            _newExhibit = ExhibitHelper.GetNewExhibit();

        }

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (ExhibitHelper.IsExhibitFound(_newExhibit))
            {
                ExhibitHelper.DeleteExhibitByJavascript(_newExhibit);
            }

        }

        [TestCleanup]
        public void TestCleanup()
        {
            CreateScreenshotsIfTestFail(TestContext);
        }

        #endregion

        [TestMethod]
        public void new_exhibit_content_shoul_be_correctly()
        {
            for (int i = 0; i < _exhibit.ContentItems.Count; i++)
            {
                Assert.AreEqual(_exhibit.ContentItems[i].Title, _newExhibit.ContentItems[i].Title, "Content items titles are not equal");
                Assert.AreEqual(_exhibit.ContentItems[i].Caption, _newExhibit.ContentItems[i].Caption, "Content items descriptions are not equal");

                Assert.AreEqual(_exhibit.ContentItems[i].MediaType, _newExhibit.ContentItems[i].MediaType, true, "Content items mediaTypes are not equal");
                Assert.AreEqual(_exhibit.ContentItems[i].MediaSource, _newExhibit.ContentItems[i].MediaSource, "Content items mediaSourses are not equal");
                Assert.AreEqual(_exhibit.ContentItems[i].Attribution, _newExhibit.ContentItems[i].Attribution, "Content items attributions are not equal");
            }
        }

        [TestMethod]
        public void new_exhibit_should_have_a_title()
        {
            Assert.AreEqual(_exhibit.Title, _newExhibit.Title, "Titles are not equal");
        }

        [TestMethod]
        public void new_exhibit_should_have_a_correct_url()
        {
            //Bug https://github.com/alterm4nn/ChronoZoom/issues/147
            for (int i = 0; i < _exhibit.ContentItems.Count; i++)
            {
                Assert.AreEqual(_exhibit.ContentItems[i].Uri, _newExhibit.ContentItems[i].Uri,
                                "Content items Urls are not equal");
            }
        }


        [TestMethod]
        public void new_exhibit_should_have_a_content_items()
        {
            Assert.AreEqual(_exhibit.ContentItems.Count, _newExhibit.ContentItems.Count, "Content items count are not equal");
        }

        [TestMethod]
        public void new_exhibit_should_not_have_null_id()
        {
            Assert.IsNotNull(_newExhibit.Id);
        }

        [TestMethod]
        public void new_exhibit_should_be_deleted()
        {
            ExhibitHelper.DeleteExhibit(_newExhibit);
            Assert.IsFalse(ExhibitHelper.IsExhibitFound(_newExhibit));
        }
    }
}