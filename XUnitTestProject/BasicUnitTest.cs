using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using TodoAPI;
using TodoAPI.Controllers;
using TodoAPI.Models;
using Xunit;

namespace XUnitTestProject
{
    /// <summary>
    /// initial the DbContext before running every test
    /// </summary>
    public class DataContextFixture : IDisposable
    {
        public TodoContext Context { get; private set; }
        public TodoController Controller { get; private set; }

        public DataContextFixture()
        {
            var optionBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionBuilder.UseInMemoryDatabase("todoList");
            Context = new TodoContext(optionBuilder.Options);
            Context.Database.EnsureDeleted();

            Controller = new TodoController(Context);
        }

        public void Dispose()
        {
        }
    }

    public class BasicUnitTest : IClassFixture<DataContextFixture>
    {
        readonly DataContextFixture _fixture = null;
        public BasicUnitTest(DataContextFixture fixture)
        {
            _fixture = fixture;

            // clear DB items before every test
            _fixture.Context.Items.RemoveRange(_fixture.Context.Items.ToList());
            _fixture.Context.SaveChanges();
        }

        [Fact]
        public TodoItem CheckTodoItemInitValue()
        {
            var item = new TodoItem();
            Assert.True(item != null, "item should be created");
            Assert.True(item.ID == 0, $"id for the 1st item should be \"0\", but got {item.ID}");
            Assert.True(item.Content.Length == 0, "name should be empty");
            Assert.True(!item.IsCompleted, "item should be is not completed");

            return item;
        }

        [Fact]
        public void CheckGetMethod()
        {
            var all = _fixture.Controller.Get();
            Assert.True(all.Count() == 0, $"Items count should be 0, but get {_fixture.Context.Items.Count()}");
        }

        [Fact]
        public void CheckPOSTSuccessfully()
        {
            var newItem = new TodoItem() { Content = "NewItem", ID = 3, IsCompleted = false };
            IActionResult result = _fixture.Controller.Create(newItem);

            var routeResult = result as CreatedAtRouteResult;
            Assert.NotNull(routeResult);
            Assert.True(routeResult.RouteName == "GetTodo", "check url");
            Assert.True(routeResult.StatusCode == 201, "status code");
            Assert.True(routeResult.Value == newItem, "check item");
        }

        [Fact]
        public void CheckPOSTFailed()
        {
            IActionResult actionResult = _fixture.Controller.Create(null);

            var okResult = actionResult as OkObjectResult;
            Assert.NotNull(okResult);
        }

    }
}
