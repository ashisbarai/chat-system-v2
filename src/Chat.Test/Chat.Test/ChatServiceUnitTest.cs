using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Api.Core.Chats;
using Chat.Api.Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Test
{
    [TestClass]
    public class ChatServiceUnitTest
    {
        [TestMethod]
        public async Task GetUsersChatByUserId_TestMethod()
        {
            var mockDapper = new Mock<IDatabase>();
            string expectedQuery = string.Join(Environment.NewLine,
                "SELECT [Id],[Message],[UserId],[FriendId],[MessageType],[CreatedOn]",
                "FROM [dbo].[Chats] WHERE UserId=@UserId AND FriendId=@FriendId",
                "ORDER BY [CreatedOn]");
            var repo = new ChatService(mockDapper.Object);
            var expectedMessages = new List<ChatInfo> { new() { Id = 1, UserId = 1, FriendId = 2, Message = "m1", MessageType = MessageType.Send }, new() { Id = 1, UserId = 1, FriendId = 3, Message = "m2", MessageType = MessageType.Received } };
            mockDapper.Setup(t => t.QueryAsync<ChatInfo>(expectedQuery, It.IsAny<object>()))
                .ReturnsAsync((string sql, object obj) =>
                {
                    int userId = (int)obj.GetType().GetProperty("UserId")?.GetValue(obj)!;
                    int friendId = (int)obj.GetType().GetProperty("FriendId")?.GetValue(obj)!;
                    return expectedMessages.Where(m=>m.UserId == userId && m.FriendId == friendId);
                });

            var messages = await repo.GetUsersChatByUserIdAsync(1, 2);
            var result = expectedMessages.Where(m => m.UserId == 1 && m.FriendId == 2);

            Assert.AreEqual(result.First().Message, messages.First().Message);

        }
    }
}
