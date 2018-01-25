using Christoc.Modules.DnnSampleTestMVC.Controllers;
using Christoc.Modules.DnnSampleTestMVC.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DnnSample.Tests
{
    [TestFixture]
    public class TestItem
    {
        [TestCase]
        public void TestMethod()
        {
            // 1 - Arrange
            int moduleId = 2;
            var mockData = MockStores.MockItemManager();
            var modTwoItemCntrl = new ItemController(mockData.Object, moduleId); // Create a controller for the module with moduleId=2.

            // 2 - Act
            var actionResult = (ViewResult)modTwoItemCntrl.Edit(); // Call the edit view with no item Id (Add New).

            // 3 - Assert
            var itemModel = (Item)actionResult.Model;
            Assert.IsTrue(itemModel != null && itemModel.ModuleId == moduleId);
        }
    }
}
