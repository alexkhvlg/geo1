using Caliburn.Micro;
using Flurl.Http.Configuration;
using GeoWrapper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Printing;
using System.Threading.Tasks;
using GeoWrapper.Models;

namespace geo1.ViewModels
{
	public class MainViewModel : PropertyChangedBase
	{
		private string _server;
		private string _login;
		private string _password;
		private string _logTextBox;

		public string Server
		{
			get => _server;
			set
			{
				_server = value;
				NotifyOfPropertyChange(() => Server);
				NotifyOfPropertyChange(() => CanSend);
			}
		}

		public string Login
		{
			get => _login;
			set
			{
				_login = value;
				NotifyOfPropertyChange(() => Login);
				NotifyOfPropertyChange(() => CanSend);
			}
		}

		public string Password
		{
			get => _password;
			set
			{
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanSend);
			}
		}

		public string LogTextBox
		{
			get => _logTextBox;
			set
			{
				_logTextBox = value;
				NotifyOfPropertyChange(() => LogTextBox);
			}
		}

		public MainViewModel()
		{
			Server = "http://localhost:8080/geoserver/rest";
			Login = "admin";
			Password = "geoserver";
		}

		public bool CanSend => !string.IsNullOrWhiteSpace(Server) && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);

		private void Print(string s, int tabCount = 0)
		{
			var tabs = string.Empty;
			for (int i = 0; i < tabCount; i++)
			{
				tabs += '\t';
			}

			LogTextBox += tabs + s + Environment.NewLine;
		}

		public async void Send()
		{
			//var factory = new PerBaseUrlFlurlClientFactory();
			//var authDataContainer = new AuthDataContainer(Server, Login, Password);
			//var ws = new WorkspaceService(factory, authDataContainer);
			//await ShowWorkspaceList(ws);
			//await CreateWorkspace(ws);
			//var w = await ShowWorkspaceByName(ws, "test2");
			//w.Name = "test3";
			//await ModifyWorkspace(ws, "test3modified", w);
			//await DeleteWorkspace(ws, "test3");

			//var ds = new DataStoreService(factory, authDataContainer);
			//await ShowDataStore(ds, "sf", "sf");

			//var fs = new FeatureTypeService(factory, authDataContainer);
			//await ShowFeatureTypes(fs);

			await CreateNewLayerScenario();
		}

		private async Task ShowFeatureTypes(FeatureTypeService fs, string workSpaceName, string dataStoreName)
		{
			try
			{
				var featureTypes = await fs.GetFeatureTypes(workSpaceName, dataStoreName);
				if (featureTypes != null)
				{
					foreach (var featureType in featureTypes)
					{
						Print($"Name: {featureType.Name}", 1);
						Print($"Href: {featureType.Href}", 1);
					}
				}
			}
			catch (Exception ex)
			{
				Print(ex.Message);
			}
		}

		private async Task CreateNewLayerScenario()
		{
			try
			{
				const string workspaceName = "nyc";
				const string workspaceUri = "http://geoserver.org/nyc";
				const string dataStoreName = "nyc_buildings";
				const string dataStoreDescription = "nyc buildings data store";
				const string server = "localhost";
				const int port = 5432;
				const string schema = "public";
				const string database = "nyc";
				const string user = "postgres";
				const string password = "postgres";
				const string dbType = "postgisng";

                var dataStoreDetailInfo = new DataStoreDetailInfo
                {
                    Name = dataStoreName,
                    Description = dataStoreDescription,
                    Enabled = true,
                    ConnectionParameters = ConnectionParameters.Create(new DataStoreSqlParams
                    {
                        Server = server,
                        Schema = schema,
                        Database = database,
                        Port = port,
                        User = user,
                        Password = password,
                        DbType = dbType,
                        CreateDb = true,
                        EstimatedExtends = true,
                        LooseBbox = true
                    })
                };

				var featureTypeDetailInfo = new FeatureTypeDetailInfo
				{
					Name = "nyc_feature",
					NativeName = "nyc_feature",
					Title = "nyc_feature",
                    Enabled = true,
					Srs = "EPSG:4326",
                    FeatureTypeAttributes = new FeatureTypeAttributesContainer
                    {
						FeatureTypeAttributes = new List<FeatureTypeAttribute>
                        {
                            //FeatureTypeAttribute.Create("the_geom","org.locationtech.jts.geom.Point"),
                            //FeatureTypeAttribute.Create("the_geom2","org.locationtech.jts.geom.Polygon"),
                            //FeatureTypeAttribute.Create("the_geom3","com.vividsolutions.jts.geom.MultiPolygon"),
                            FeatureTypeAttribute.Create("the_geom","com.vividsolutions.jts.geom.MultiPoint"),
                            FeatureTypeAttribute.Create("name","java.lang.String"),
                            //FeatureTypeAttribute.Create("timestamp","java.util.Date")
                        }
					}
                };
				
				var factory = new PerBaseUrlFlurlClientFactory();
				var authDataContainer = new AuthDataContainer(Server, Login, Password);
				var ws = new WorkspaceService(factory, authDataContainer);
				var ds = new DataStoreService(factory, authDataContainer);
				var fs = new FeatureTypeService(factory, authDataContainer);

                Print("CreateWorkspace..");
                await ws.CreateWorkspace(workspaceName, workspaceUri);
                Print("Done");

                Print("ShowWorkspaceByName:");
                await ShowWorkspaceByName(ws, workspaceName);

                Print("CreateDataStore..");
                await ds.CreateDataStore(workspaceName, dataStoreDetailInfo);
                Print("Done");

                Print("ShowDataStore:");
                await ShowDataStore(ds, workspaceName, dataStoreName);


                Print("CreateFeatureType..");
                await fs.CreateFeatureType(workspaceName, dataStoreName, featureTypeDetailInfo);
                Print("Done");

                Print("ShowFeatureTypes:");
				await ShowFeatureType(fs, workspaceName, dataStoreName, featureTypeDetailInfo.Name);
			}
			catch (Exception ex)
			{
				Print(ex.Message);
			}
			finally
			{
				Print("Done");
			}
		}

        private async Task ShowFeatureType(FeatureTypeService fs, string workspaceName, string dataStoreName, string featureTypeName)
        {
            var featureTypeDetailInfo = await fs.GetFeatureType(workspaceName, dataStoreName, featureTypeName);
            Print($"Name: {featureTypeDetailInfo.Name}", 1);
            Print($"Description: {featureTypeDetailInfo.NativeName}", 1);
            Print($"Enabled: {featureTypeDetailInfo.Enabled}", 1);
            Print($"Name: {featureTypeDetailInfo.Title}", 1);
            Print($"Href: {featureTypeDetailInfo.Srs}", 1);
            Print("NativeBoundingBox:", 1);
            if (featureTypeDetailInfo.NativeBoundingBox != null)
            {
                Print($"MinX: {featureTypeDetailInfo.NativeBoundingBox.MinX}", 2);
                Print($"MaxX: {featureTypeDetailInfo.NativeBoundingBox.MaxX}", 2);
                Print($"MinY: {featureTypeDetailInfo.NativeBoundingBox.MinY}", 2);
                Print($"MaxY: {featureTypeDetailInfo.NativeBoundingBox.MaxY}", 2);
                //Print($"Crs: {featureTypeDetailInfo.NativeBoundingBox.Crs}", 2);
            }
            Print("LatLonBoundingBox:", 1);
            if (featureTypeDetailInfo.LatLonBoundingBox != null)
            {
                Print($"MinX: {featureTypeDetailInfo.LatLonBoundingBox.MinX}", 2);
                Print($"MaxX: {featureTypeDetailInfo.LatLonBoundingBox.MaxX}", 2);
                Print($"MinY: {featureTypeDetailInfo.LatLonBoundingBox.MinY}", 2);
                Print($"MaxY: {featureTypeDetailInfo.LatLonBoundingBox.MaxY}", 2);
                //Print($"Crs: {featureTypeDetailInfo.LatLonBoundingBox.Crs}", 2);
            }
            Print($"OverridingServiceSrs: {featureTypeDetailInfo.OverridingServiceSrs}", 1);
            Print($"SkipNumberMatched: {featureTypeDetailInfo.SkipNumberMatched}", 1);
            Print($"CircularArcPresent: {featureTypeDetailInfo.CircularArcPresent}", 1);
            Print($"keywords: {string.Join(",", featureTypeDetailInfo.KeywordsContainer?.Strings ?? Array.Empty<string>())}", 1);
            Print("Attributes:", 1);
            if (featureTypeDetailInfo.FeatureTypeAttributes != null)
            {
                int i = 1;
                foreach (var featureTypeAttribute in featureTypeDetailInfo.FeatureTypeAttributes.FeatureTypeAttributes)
                {
					Print($"Attr #{i++}:", 2);
                    Print($"{featureTypeAttribute.Name}", 3);
                    Print($"{featureTypeAttribute.MinOccurs}", 3);
                    Print($"{featureTypeAttribute.MaxOccurs}", 3);
                    Print($"{featureTypeAttribute.Nullable}", 3);
                    Print($"{featureTypeAttribute.Binding}", 3);
                }
            }
        }

        private async Task ShowDataStore(DataStoreService ds, string workSpaceName, string dataStoreName)
		{
			try
			{
				var dataStore = await ds.GetDataStore(workSpaceName, dataStoreName);
				Print($"Name: {dataStore.Name}", 1);
				Print($"Description: {dataStore.Description}", 1);
				Print($"Enabled: {dataStore.Enabled}", 1);
				Print("Workspace:", 1);
				Print($"Name: {dataStore.Workspace.Name}", 1);
				Print($"Href: {dataStore.Workspace.Href}", 1);
				Print("ConnectionParameters:", 1);
				if (dataStore.ConnectionParameters?.Entries != null)
				{
					foreach (var connectionParameterEntry in dataStore.ConnectionParameters.Entries)
					{
						Print($"Key: {connectionParameterEntry.Key}", 2);
						Print($"Value: {connectionParameterEntry.Value}", 2);
					}
				}

				Print($"IsDefault: {dataStore.IsDefault}", 1);
				Print($"FeatureTypesUrl: {dataStore.FeatureTypesUrl}", 1);
			}
			catch (Exception ex)
			{
				Print(ex.Message);
			}
		}

		private async Task GetDataStoreList(DataStoreService ds, string workspaceName)
		{
			try
			{
				var dataStores = await ds.GetDateStores(workspaceName);
				if (dataStores != null)
				{
					foreach (var dataStore in dataStores)
					{
						Print($"Name: {dataStore.Name}");
						Print($"Href: {dataStore.Href}");
					}
				}
			}
			catch (Exception ex)
			{
				Print(ex.Message);
			}
		}

		private async Task DeleteWorkspace(WorkspaceService ws, string name)
		{
			try
			{
				await ws.DeleteWorkspace(name);
			}
			catch (Exception ex)
			{
				Print(ex.Message);
			}
		}

		private async Task ModifyWorkspace(WorkspaceService ws, string name, WorkspaceDetailInfo workspaceDetailInfo)
		{
			try
			{
				await ws.UpdateWorkspace(name, workspaceDetailInfo);
			}
			catch (Exception ex)
			{
				Print(ex.Message);
			}
		}

		private async Task<WorkspaceDetailInfo> ShowWorkspaceByName(WorkspaceService ws, string name)
		{
			try
			{
				var workspace = await ws.GetWorkspace(name);
				Print($"Name: {workspace.Name}", 1);
				Print($"Isolated: {workspace.Isolated}", 1);
				Print($"CoverageStores: {workspace.CoverageStores}", 1);
				Print($"DataStores: {workspace.DataStores}", 1);
				Print($"DateCreated: {workspace.DateCreated}", 1);
				Print($"WmsStores: {workspace.WmsStores}", 1);
				Print($"WmtsStores: {workspace.WmtsStores}", 1);
				return workspace;
			}
			catch (Exception ex)
			{
				Print(ex.Message);
				return null;
			}
		}

		private async Task CreateWorkspace(WorkspaceService ws)
		{
			try
			{
				var isCreated = await ws.CreateWorkspace("test3", "http://localhost:8080/geoserver/rest/workspaces/test3.json");
				Print($"Is created: {isCreated}");
			}
			catch (Exception ex)
			{
				Print(ex.Message);
			}
		}

		private async Task ShowWorkspaceList(WorkspaceService ws)
		{
			try
			{
				var workSpaceList = await ws.GetWorkspaces();
				if (workSpaceList != null)
				{
					foreach (var workspace in workSpaceList)
					{
						Print($"Name: {workspace.Name}");
						Print($"Href: {workspace.Href}");
					}
				}
			}
			catch (Exception ex)
			{
				Print(ex.Message);
			}
		}
	}
}