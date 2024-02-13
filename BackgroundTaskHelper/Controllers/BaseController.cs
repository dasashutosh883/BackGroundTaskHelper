using BackgroundTaskHelper.CacheHelper;
using BackgroundTaskHelper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Threading;

namespace BackgroundTaskHelper.Controllers
{
	public class GlobalListManager<T>
	{
		private readonly List<T> globalList = new List<T>();

		public List<T> GetGlobalList()
		{
			return globalList;
		}

		public void AppendToGlobalList(T data)
		{
			globalList.Add(data);
		}
		public void RemoveGlobalList()
		{
			globalList.Clear();
		}
	}
	public class BaseController : Controller
	{
		private readonly GlobalListManager<LogModel> _listManager;
		public BaseController(GlobalListManager<LogModel> listManager)
		{
			_listManager = listManager;
		}
		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var request = context.HttpContext.Request;
			LogModel log = new LogModel();
			log.Username = request.HttpContext.User.Identity!.Name ?? "Anonymous";
			log.IpAddress = request.HttpContext.Connection.RemoteIpAddress!.ToString();
			log.AccessedURL = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request);
			log.Timestamp = DateTime.Now.ToString();
			if (context.HttpContext.Request.Method == "POST")
			{
				log.RequestType = context.HttpContext.Request.Method;
			}
			else
			{
				log.RequestType = context.HttpContext.Request.Method;
			}
			_listManager.AppendToGlobalList(log);
			await next();
		}

	}
	public class PeriodicHostedService : BackgroundService
	{
		public int TimePeriod { get; set; }
		public bool IsEnabled { get; set; }
		private readonly ILogger<PeriodicHostedService> _logger;
		private readonly IServiceScopeFactory _factory;
		private readonly GlobalListManager<LogModel> _listManager;
		public PeriodicHostedService(ILogger<PeriodicHostedService> logger, IServiceScopeFactory factory, GlobalListManager<LogModel> listManager)
		{
			_logger = logger;
			_factory = factory;
			_listManager = listManager;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			TimeSpan _period = TimeSpan.FromSeconds(15);

			// ExecuteAsync is executed once and we have to take care of a mechanism ourselves that is kept during operation.
			// To do this, we can use a Periodic Timer, which, unlike other timers, does not block resources.
			// But instead, WaitForNextTickAsync provides a mechanism that blocks a task and can thus be used in a While loop.
			using PeriodicTimer timer = new PeriodicTimer(_period);

			// When ASP.NET Core is intentionally shut down, the background service receives information
			// via the stopping token that it has been canceled.
			// We check the cancellation to avoid blocking the application shutdown.
			while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
			{
				try
				{
					if (IsEnabled)
					{
						// We cannot use the default dependency injection behavior, because ExecuteAsync is
						// a long-running method while the background service is running.
						// To prevent open resources and instances, only create the services and other references on a run

						// Create scope, so we get request services
						await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();


						_logger.LogInformation($"Log written successfully");
						List<LogModel> list = _listManager.GetGlobalList();
						_logger.LogInformation(JsonConvert.SerializeObject(list).ToString());
						_listManager.RemoveGlobalList();
					}
					else
					{
						_logger.LogInformation("Skipped PeriodicHostedService");
					}
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
				}
			}
		}


	}


}
