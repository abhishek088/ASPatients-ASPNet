using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPatients.Controllers
{
    [Authorize(Roles = "administrators")]
    public class ASRoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        public ASRoleController(RoleManager<IdentityRole> roleManger, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManger;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var roles = _roleManager.Roles.OrderBy(a => a.Name);
            return View(await roles.ToListAsync());
        }
        //list all users 
        public async Task<IActionResult> ListUsers(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            var users = _userManager.Users;
            List<IdentityUser> notInRole = new List<IdentityUser>();
            foreach (var item in users)
            {
                if (!usersInRole.Contains(item))
                    notInRole.Add(item);
            }
            ViewBag.notInRole = new SelectList(notInRole.OrderBy(a => a.UserName), "UserName", "UserName");

            ViewBag.roleName = roleName;
            return View(usersInRole.OrderBy(a => a.UserName).ToList());
        }
        //create new role
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["message"] = "Role name cannot be null or empty";
                return RedirectToAction("Action");
            }
            roleName = roleName.Trim();
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                TempData["message"] = $"Role name '{roleName}' cannot be null or empty";
                return RedirectToAction("Action");
            }
            try
            {
                IdentityRole role = new IdentityRole(roleName);
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded) 
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                TempData["message"] = $"Role added : '{roleName}'";
            }
            catch (Exception e)
            {
                TempData["message"] = $"Exception adding role '{roleName}': {e.GetBaseException().Message}";
            }
            return RedirectToAction("Index");
        }
        

        public async Task<IActionResult> RemoveFromRole(string roleName, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                TempData["message"] = $"User '{userName}' is not on file";
                return Redirect($"/ASRole/ListUsers?roleName={roleName}");
            }
            try
            {
                IdentityResult result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (!result.Succeeded)
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                TempData["message"] = $"User '{userName}' removed from role '{roleName}'";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"Exception trying to remove {userName}";
            }
            return Redirect($"/ASRole/ListUsers?roleName={roleName}");
        }

        //to add new user
        public async Task<IActionResult> AddNewUser(string roleName, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                TempData["message"] = $"User '{userName}' is not on file";
                return Redirect($"/ASRole/ListUsers?roleName={roleName}");
            }
            try
            {
                IdentityResult result = await _userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                TempData["message"] = $"User '{userName}' added to role '{roleName}'";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"Exception trying to add {userName}";
            }
            return Redirect($"/ASRole/ListUsers?roleName={roleName}");
        }

        // for delete
        public async Task<IActionResult> DeleteRoleAction(string roleName)
        {
            if (roleName == null)
            {
                TempData["message"] = $"No role name is selected to delete";
                return RedirectToAction("Index");
            }
            else if (roleName == "administrators")
            {
                TempData["message"] = $"Role name: {roleName} cannot be deleted.";
                return RedirectToAction("Index");
            }
            else
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                var users = _userManager.Users;
                List<IdentityUser> inRole = new List<IdentityUser>();
                foreach (var item in users)
                {
                    if (usersInRole.Contains(item)) 
                        inRole.Add(item);
                }
                if (!inRole.Any())
                {
                    try
                    {
                        IdentityRole role = await _roleManager.FindByNameAsync(roleName);
                        IdentityResult result = await _roleManager.DeleteAsync(role);
                        if (!result.Succeeded) 
                            throw new Exception(result.Errors.FirstOrDefault().Description);
                        TempData["message"] = $"Role: '{roleName}' removed from the list of roles";
                    }
                    catch (Exception ex)
                    {
                        TempData["message"] = $"Exception trying to remove {roleName}";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.inRole = (inRole.OrderBy(a => a.UserName), "UserName", "UserName");
                    ViewBag.roleName = roleName;
                    return View(usersInRole.OrderBy(a => a.UserName).ToList());
                }
            }
        }

        //confirm delete page
        public async Task<IActionResult> ConfirmDeleteAction(string roleName)
        {
            try
            {
                IdentityRole role = await _roleManager.FindByNameAsync(roleName);
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded) 
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                TempData["message"] = $"Role removed: {roleName}";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"Exception trying to remove {roleName}";
            }
            return RedirectToAction("Index");
        }
    }
}
