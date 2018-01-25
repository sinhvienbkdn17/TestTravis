/*
' Copyright (c) 2018 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Linq;
using System.Web.Mvc;
using Christoc.Modules.DnnSampleTestMVC.Components;
using Christoc.Modules.DnnSampleTestMVC.Models;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace Christoc.Modules.DnnSampleTestMVC.Controllers
{
    
    [DnnHandleError]
    public class ItemController : DnnController
    {
        private int _moduleId;
        private IItemManager _itemManager;
        public ItemController(IItemManager itemManager, int moduleId)
        {
            _moduleId = moduleId;
            _itemManager = itemManager;
        }
        public ActionResult Delete(int itemId)
        {
            ItemManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
            return RedirectToDefaultRoute();
        }

        public ActionResult Edit(int itemId = -1)
        {
            // Ignore registration errors for unit tests.
            try { DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins); }
            catch { }

            if (PortalSettings != null)
            {
                var userlist = UserController.GetUsers(PortalSettings.PortalId);
                var users = from user in userlist.Cast<UserInfo>().ToList()
                            select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };

                ViewBag.Users = users;
            }

            if (ModuleContext != null)
            {
                _moduleId = ModuleContext.ModuleId;
            }

            var item = (itemId == -1)
                ? new Item { ModuleId = _moduleId }
                : _itemManager.GetItem(itemId, _moduleId);

            return View(item);
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (item.ItemId == -1)
            {
                item.CreatedByUserId = User.UserID;
                item.CreatedOnDate = DateTime.UtcNow;
                item.LastModifiedByUserId = User.UserID;
                item.LastModifiedOnDate = DateTime.UtcNow;

                ItemManager.Instance.CreateItem(item);
            }
            else
            {
                var existingItem = ItemManager.Instance.GetItem(item.ItemId, item.ModuleId);
                existingItem.LastModifiedByUserId = User.UserID;
                existingItem.LastModifiedOnDate = DateTime.UtcNow;
                existingItem.ItemName = item.ItemName;
                existingItem.ItemDescription = item.ItemDescription;
                existingItem.AssignedUserId = item.AssignedUserId;

                ItemManager.Instance.UpdateItem(existingItem);
            }

            return RedirectToDefaultRoute();
        }

        [ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult Index()
        {
            var items = ItemManager.Instance.GetItems(ModuleContext.ModuleId);
            return View(items);
        }
    }
}
