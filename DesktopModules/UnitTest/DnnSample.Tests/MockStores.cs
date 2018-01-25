using Christoc.Modules.DnnSampleTestMVC.Components;
using Christoc.Modules.DnnSampleTestMVC.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnnSample.Tests
{
    class MockStores
    {
        public static Mock<IItemManager> MockItemManager()
        {
            var allItems = new List<Item>();
            var mock = new Mock<IItemManager>();

            // void CreateItem(Item t);
            mock.Setup(x => x.CreateItem(It.IsAny<Item>()))
                .Callback((Item i) =>
                {
                    allItems.Add(i);
                });

            // void DeleteItem(int itemId, int moduleId);
            mock.Setup(x => x.DeleteItem(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int id, int mid) =>
                {
                    var remItem = allItems.FirstOrDefault(i => i.ItemId == id && i.ModuleId == mid);
                    allItems.Remove(remItem);
                });

            // void DeleteItem(Item t);
            mock.Setup(x => x.DeleteItem(It.IsAny<Item>()))
                .Callback((Item di) =>
                {
                    var remItem = allItems.FirstOrDefault(i => i.ItemId == di.ItemId);
                    allItems.Remove(remItem);
                });

            // IEnumerable<Item> GetItems(int moduleId);
            mock.Setup(x => x.GetItems(It.IsAny<int>()))
                .Returns((int mid) => allItems.Where(x => x.ModuleId == mid));

            // Item GetItem(int itemId, int moduleId);
            mock.Setup(x => x.GetItem(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int id, int mid) => allItems.FirstOrDefault(i => i.ItemId == id && i.ModuleId == mid));

            // void UpdateItem(Item t);
            mock.Setup(x => x.UpdateItem(It.IsAny<Item>()))
                .Callback((Item i) =>
                {
                    allItems.Add(i);
                });

            return mock;
        }
    }
}
