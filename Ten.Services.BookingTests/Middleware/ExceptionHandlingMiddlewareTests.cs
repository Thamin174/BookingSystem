using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Ten.Services.BookingAPI.Middlewares;
using Ten.Services.BookingDomain.Exceptions;

namespace Ten.Services.BookingTests.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_ShouldReturn_NotFound_ForNotFoundException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            var middleware = new ExceptionHandlingMiddleware(
                async (innerHttpContext) => { throw new NotFoundException("Resource not found"); },
                loggerMock.Object
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(context.Response.Body).ReadToEndAsync();

            Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturn_BadRequest_ForArgumentException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            var middleware = new ExceptionHandlingMiddleware(
                async (innerHttpContext) => { throw new ArgumentException("Invalid argument"); },
                loggerMock.Object
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(context.Response.Body).ReadToEndAsync();

            Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturn_InternalServerError_ForGenericException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            var middleware = new ExceptionHandlingMiddleware(
                async (innerHttpContext) => { throw new Exception("Something went wrong"); },
                loggerMock.Object
            );

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = await new StreamReader(context.Response.Body).ReadToEndAsync();

            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

    }
}
