using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Api.Core.Interfaces;
using Chat.Api.Core.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Test
{
    [TestClass]
    public class UserUnitTest
    {
        [TestMethod]
        public async Task GetAllUsers_TestMethod()
        {
            var mockDapper = new Mock<IDatabase>();
            string expectedQuery = string.Join(Environment.NewLine, "SELECT [Id],[Email],[FirstName],[LastName],[CreatedOn]",
                "FROM [dbo].[Users]",
                "ORDER BY [FirstName],[LastName]");
            var repo = new UserService(mockDapper.Object);
            var expectedUsers = new List<UserInfoDto> { new() { Id = 1, Email = "a@b.com", FirstName = "a", LastName = "b" } };
            mockDapper.Setup(t => t.QueryAsync<UserInfoDto>(expectedQuery))
                .ReturnsAsync(expectedUsers);

            var users = await repo.GetAllUsersAsync();

            Assert.AreSame(expectedUsers, users);

        }
        [TestMethod]
        public async Task GetUserById_TestMethod()
        {
            var mockDapper = new Mock<IDatabase>();
            string expectedQuery = string.Join(Environment.NewLine, "SELECT [Id],[Email],[FirstName],[LastName],[CreatedOn]",
                "FROM [dbo].[Users] WHERE Id=@Id");
            var repo = new UserService(mockDapper.Object);
            var expectedUsers = new List<UserInfoDto> { new() { Id = 1, Email = "a@b.com", FirstName = "a", LastName = "b" }, new() { Id = 2, Email = "c@d.com", FirstName = "c", LastName = "d" } };
            mockDapper.Setup(t => t.QueryAsync<UserInfoDto>(expectedQuery, It.IsAny<object>()))
                .ReturnsAsync((string sql, object obj) =>
                {
                    int id = (int)obj.GetType().GetProperty("Id")?.GetValue(obj)!;
                    return expectedUsers.Where(u=>u.Id == id);
                });

            var user = await repo.GetUserByIdAsync(1);

            Assert.AreSame(expectedUsers.First(u=>u.Id == 1), user);

        }
    }
}
